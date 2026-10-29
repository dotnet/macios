# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import contextlib
import io
import json
import os
import tempfile
import unittest
from pathlib import Path
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


class DevSkimCommentTests(unittest.TestCase):
    def test_clean_run_never_posts_or_requires_credentials(self):
        with tempfile.TemporaryDirectory() as directory:
            summary = Path(directory) / "summary.json"
            summary.write_text('{"has_failures": false}', encoding="utf-8")
            with patch.dict(os.environ, {}, clear=True), patch.object(comment_devskim, "urlopen") as post:
                with contextlib.redirect_stdout(io.StringIO()):
                    self.assertEqual(0, comment_devskim.main([str(summary)]))
                post.assert_not_called()

    def test_failure_posts_link_matching_drop_path(self):
        with tempfile.TemporaryDirectory() as directory:
            summary = Path(directory) / "summary.json"
            summary.write_text(json.dumps({"has_failures": True, "banned_count": 145, "error_count": 0}),
                               encoding="utf-8")
            env = {
                "BUILD_REPOSITORY_NAME": "dotnet/macios",
                "SYSTEM_PULLREQUEST_PULLREQUESTNUMBER": "26957",
                "BUILD_BUILDNUMBER": "20261029.1",
                "BUILD_BUILDID": "123",
                "SYSTEM_JOBATTEMPT": "2",
                "SYSTEM_COLLECTIONURI": "https://devdiv.visualstudio.com/",
                "SYSTEM_TEAMPROJECT": "DevDiv",
                "GITHUB_TOKEN": "placeholder-token",
            }
            with patch.dict(os.environ, env, clear=True), patch.object(comment_devskim, "urlopen") as post:
                post.return_value.__enter__.return_value = io.StringIO('{"html_url": "https://github.com/comment"}')
                with contextlib.redirect_stdout(io.StringIO()):
                    self.assertEqual(0, comment_devskim.main([str(summary)]))
                request = post.call_args[0][0]
                self.assertEqual("https://api.github.com/repos/dotnet/macios/issues/26957/comments",
                                 request.full_url)
                self.assertEqual("POST", request.method)
                body = json.loads(request.data)["body"]
                self.assertIn("145 banned-API", body)
                self.assertIn("/macios/devskim/20261029.1/123-2/;/index.html", body)
                self.assertIn("Copilot", body)

    def test_comment_includes_scan_errors(self):
        body = comment_devskim.comment_body({"has_failures": True, "banned_count": 0, "error_count": 9},
                                            "https://report", "https://build")
        self.assertIn("9 scan error(s)", body)
        self.assertIsNone(comment_devskim.comment_body({"has_failures": False}, "", ""))


if __name__ == "__main__":
    unittest.main()
