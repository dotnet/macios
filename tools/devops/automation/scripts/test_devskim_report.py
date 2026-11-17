# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import contextlib
import io
import json
import os
import tempfile
import unittest
from pathlib import Path
from urllib.error import HTTPError
from unittest.mock import patch

import comment_devskim
import report_devskim
from test_devskim import finding, report


class DevSkimReportTests(unittest.TestCase):
    def create_report(self, directory, results):
        path = Path(directory) / "runtime.sarif"
        document = report(results)
        document["runs"][0]["versionControlProvenance"] = [{
            "repositoryUri": "https://github.com/dotnet/macios",
            "revisionId": "a" * 40,
        }]
        path.write_text(json.dumps(document), encoding="utf-8")
        return path

    def test_summary_separates_blocking_and_other_findings(self):
        with tempfile.TemporaryDirectory() as directory:
            path = self.create_report(directory, [finding(), finding(["Other.Tag"])])
            findings, summary = report_devskim.collect_reports([path], "Failed")
            self.assertEqual(2, summary["finding_count"])
            self.assertEqual(1, summary["banned_count"])
            self.assertEqual(0, summary["error_count"])
            self.assertTrue(summary["has_failures"])
            rendered = report_devskim.render_html(findings, summary, [])
            self.assertEqual(2, rendered.count("<tr data-banned="))
            self.assertIn('data-banned="true"', rendered)
            self.assertIn('data-banned="false"', rendered)

    def test_nonblocking_findings_do_not_fail(self):
        with tempfile.TemporaryDirectory() as directory:
            path = self.create_report(directory, [finding(["Other.Tag"])])
            findings, summary = report_devskim.collect_reports([path], "Succeeded")
            self.assertFalse(summary["has_failures"])
            self.assertIn("Passed: no banned-API findings", report_devskim.render_html(findings, summary, []))

    def test_source_links_and_severity(self):
        with tempfile.TemporaryDirectory() as directory:
            result = finding()
            result["properties"]["DevSkimSeverity"] = "Moderate"
            result["locations"] = [{
                "physicalLocation": {
                    "artifactLocation": {"uri": "runtime/file%20name.c"},
                    "region": {"startLine": 42, "snippet": {"text": "printf"}},
                },
            }]
            path = self.create_report(directory, [result])
            findings, summary = report_devskim.collect_reports([path], "Succeeded")
            rendered = report_devskim.render_html(findings, summary, [])
            self.assertIn("/blob/" + "a" * 40 + "/runtime/file%20name.c#L42", rendered)
            self.assertIn("Moderate", rendered)
            self.assertIn("<pre>printf</pre>", rendered)

    def test_banned_api_is_visible_outside_collapsed_details(self):
        cases = [
            ("Banned C function detected", "printf", "Banned C function detected: printf"),
            ("Banned C function detected", "snprintf (buffer, size, format)",
             "Banned C function detected: snprintf"),
            ("Banned C function detected (strcpy)", "strcpy(dst, src)", "Banned C function detected (strcpy)"),
            ("Banned C function detected", "", "Banned C function detected"),
        ]
        for message, snippet, expected in cases:
            with self.subTest(snippet=snippet), tempfile.TemporaryDirectory() as directory:
                result = finding()
                result["message"] = {"text": message}
                result["locations"] = [{
                    "physicalLocation": {"region": {"snippet": {"text": snippet}}},
                }]
                path = self.create_report(directory, [result])
                findings, summary = report_devskim.collect_reports([path], "Succeeded")
                self.assertEqual(expected, findings[0]["message"])
                rendered = report_devskim.render_html(findings, summary, [])
                self.assertIn(f'<td class="issue">{expected}', rendered)

    def test_untrusted_sarif_is_escaped(self):
        with tempfile.TemporaryDirectory() as directory:
            result = finding()
            result["message"] = {"text": '<script>alert("bad")</script><!--ROWS-->'}
            result["locations"] = [{
                "physicalLocation": {
                    "artifactLocation": {"uri": 'javascript:alert("bad")'},
                    "region": {"startLine": 1, "snippet": {"text": "</pre><img src=x onerror=alert(1)>"}},
                },
            }]
            path = self.create_report(directory, [result])
            document = json.loads(path.read_text())
            document["runs"][0]["tool"]["driver"]["rules"] = [{
                "id": "DS154189",
                "helpUri": "javascript:alert(1)",
                "help": {"text": "<script>guidance</script>"},
            }]
            path.write_text(json.dumps(document), encoding="utf-8")
            findings, summary = report_devskim.collect_reports([path], "Succeeded")
            rendered = report_devskim.render_html(findings, summary, [])
            self.assertNotIn('<script>alert("bad")', rendered)
            self.assertNotIn('<a href="javascript:', rendered)
            self.assertNotIn("<img src=x", rendered)
            self.assertIn("&lt;script&gt;", rendered)
            self.assertIn("&lt;!--ROWS--&gt;", rendered)

    def test_unsafe_source_paths_are_not_linked(self):
        run = {"versionControlProvenance": [{
            "repositoryUri": "https://github.com/dotnet/macios", "revisionId": "a" * 40,
        }]}
        for uri in ["../secret", "%2e%2e/secret", "/absolute", "https://evil.example/file", "file\\name"]:
            with self.subTest(uri=uri):
                self.assertEqual("", report_devskim.source_url(run, uri, 1))

    def test_missing_and_invalid_reports_remain_visible(self):
        with tempfile.TemporaryDirectory() as directory:
            path = self.create_report(directory, [finding()])
            malformed = Path(directory) / "bad.sarif"
            malformed.write_text("{", encoding="utf-8")
            missing = Path(directory) / "missing.sarif"
            findings, summary = report_devskim.collect_reports([missing, malformed, path], "Failed")
            self.assertEqual(1, len(findings))
            self.assertEqual(2, summary["error_count"])
            self.assertEqual(["runtime.sarif"], summary["sources"])
            self.assertTrue(summary["has_failures"])
            self.assertIn("incomplete coverage", report_devskim.render_html(findings, summary, []))

    def test_failed_job_with_clean_sarif_is_not_reported_as_success(self):
        with tempfile.TemporaryDirectory() as directory:
            path = self.create_report(directory, [])
            _, summary = report_devskim.collect_reports([path], "Failed")
            self.assertTrue(summary["has_failures"])
            self.assertEqual(1, summary["error_count"])

    def test_generates_html_and_machine_readable_summary(self):
        with tempfile.TemporaryDirectory() as directory:
            path = self.create_report(directory, [])
            (Path(directory) / "install.log").write_text("install error", encoding="utf-8")
            with contextlib.redirect_stdout(io.StringIO()):
                self.assertEqual(0, report_devskim.main(["--output-directory", directory, str(path)]))
            self.assertFalse(json.loads((Path(directory) / "summary.json").read_text())["has_failures"])
            self.assertIn('href="install.log"', (Path(directory) / "index.html").read_text())
            self.assertIn("Other findings (non-blocking)", (Path(directory) / "report.md").read_text())

    def test_markdown_preserves_details_without_rendering_html(self):
        with tempfile.TemporaryDirectory() as directory:
            result = finding()
            result["message"] = {"text": "Banned | API"}
            result["locations"] = [{
                "physicalLocation": {
                    "artifactLocation": {"uri": "runtime/probe.c"},
                    "region": {"startLine": 3, "snippet": {"text": "printf(\"<script>\");"}},
                },
            }]
            path = self.create_report(directory, [result])
            findings, summary = report_devskim.collect_reports([path], "Failed")
            rendered = report_devskim.render_markdown(findings, summary)
            self.assertIn("runtime/probe.c:3", rendered)
            self.assertIn("Banned &#124; API", rendered)
            self.assertIn("printf(&quot;&lt;script&gt;&quot;);", rendered)
            self.assertNotIn("<script>", rendered)


class DevSkimCommentTests(unittest.TestCase):
    def setUp(self):
        self.directory = tempfile.TemporaryDirectory()
        self.addCleanup(self.directory.cleanup)
        self.summary = Path(self.directory.name) / "summary.json"
        self.summary.write_text(json.dumps({"has_failures": True, "banned_count": 145, "error_count": 0}),
                                encoding="utf-8")
        self.summary.with_name("report.md").write_text("# Detailed report", encoding="utf-8")
        self.env = {
            "BUILD_REPOSITORY_NAME": "dotnet/macios",
            "SYSTEM_PULLREQUEST_PULLREQUESTNUMBER": "26957",
            "BUILD_BUILDNUMBER": "20261029.1",
            "BUILD_BUILDID": "123",
            "SYSTEM_JOBATTEMPT": "2",
            "SYSTEM_COLLECTIONURI": "https://devdiv.visualstudio.com/",
            "SYSTEM_TEAMPROJECT": "DevDiv",
            "GITHUB_TOKEN": "placeholder-token",
            "DEVSKIM_DROP_UPLOADED": "True",
            "DEVSKIM_REPORT_URL": "https://vsdrop.corp.microsoft.com/file/v1/macios/devskim/20261029.1/123-2/;/index.html",
        }

    def test_clean_run_never_posts_or_requires_credentials(self):
        with tempfile.TemporaryDirectory() as directory:
            summary = Path(directory) / "summary.json"
            summary.write_text('{"has_failures": false}', encoding="utf-8")
            with patch.dict(os.environ, {}, clear=True), patch.object(comment_devskim, "urlopen") as post:
                with contextlib.redirect_stdout(io.StringIO()):
                    self.assertEqual(0, comment_devskim.main([str(summary)]))
                post.assert_not_called()

    def test_failure_creates_secret_gist_then_posts_pr_link(self):
        with patch.dict(os.environ, self.env, clear=True), patch.object(comment_devskim, "urlopen") as post:
            response = post.return_value.__enter__
            response.side_effect = [
                io.StringIO('{"html_url": "https://gist.github.com/example"}'),
                io.StringIO('{"html_url": "https://github.com/comment"}'),
            ]
            with contextlib.redirect_stdout(io.StringIO()):
                self.assertEqual(0, comment_devskim.main([str(self.summary)]))
            gist_request, comment_request = [call.args[0] for call in post.call_args_list]
            self.assertEqual("https://api.github.com/gists", gist_request.full_url)
            gist = json.loads(gist_request.data)
            self.assertFalse(gist["public"])
            self.assertEqual("# Detailed report", gist["files"]["devskim-report.md"]["content"])
            self.assertEqual("https://api.github.com/repos/dotnet/macios/issues/26957/comments",
                             comment_request.full_url)
            body = json.loads(comment_request.data)["body"]
            self.assertIn("145 banned-API", body)
            self.assertIn("https://gist.github.com/example", body)
            self.assertIn("/macios/devskim/20261029.1/123-2/;/index.html", body)
            self.assertIn("Copilot", body)

    def test_branch_failure_comments_on_commit_even_when_drop_upload_failed(self):
        del self.env["SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"]
        self.env.update(BUILD_SOURCEVERSION="a" * 40, DEVSKIM_DROP_UPLOADED="False")
        with patch.dict(os.environ, self.env, clear=True), patch.object(comment_devskim, "post_json") as post:
            post.return_value = {"html_url": "https://gist.github.com/example"}
            with contextlib.redirect_stdout(io.StringIO()):
                self.assertEqual(0, comment_devskim.main([str(self.summary)]))
            endpoint, payload = post.call_args.args
            self.assertEqual("repos/dotnet/macios/commits/" + "a" * 40 + "/comments", endpoint)
            self.assertNotIn("vsdrop", payload["body"])
            self.assertIn("https://gist.github.com/example", payload["body"])

    def test_gist_failure_is_not_ignored_and_does_not_post_comment(self):
        with patch.dict(os.environ, self.env, clear=True), patch.object(comment_devskim, "urlopen") as post:
            post.side_effect = HTTPError("https://api.github.com/gists", 403, "Forbidden", {}, None)
            with self.assertRaises(HTTPError):
                comment_devskim.main([str(self.summary)])
            self.assertEqual(1, post.call_count)

    def test_pr_build_without_number_does_not_fall_back_to_merge_commit(self):
        del self.env["SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"]
        self.env.update(BUILD_REASON="PullRequest", BUILD_SOURCEVERSION="a" * 40)
        with patch.dict(os.environ, self.env, clear=True), patch.object(comment_devskim, "urlopen") as post:
            with self.assertRaisesRegex(ValueError, "PR number"):
                comment_devskim.main([str(self.summary)])
            post.assert_not_called()

    def test_comment_includes_scan_errors(self):
        body = comment_devskim.comment_body({"has_failures": True, "banned_count": 0, "error_count": 9},
                                            "https://report", "https://build")
        self.assertIn("9 scan error(s)", body)
        self.assertIsNone(comment_devskim.comment_body({"has_failures": False}, "", ""))


if __name__ == "__main__":
    unittest.main()
