# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import contextlib
import io
import json
import subprocess
import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch

import run_devskim
from test_devskim import finding, report


class DevSkimRunnerTests(unittest.TestCase):
    def test_scan_replaces_stale_reports_and_generates_details_on_failure(self):
        for exit_code, results in [(0, [finding()]), (1, [])]:
            with self.subTest(exit_code=exit_code), tempfile.TemporaryDirectory() as directory:
                output = Path(directory)
                (output / "runtime.sarif").write_text(json.dumps(report([])), encoding="utf-8")

                def scan(command, **kwargs):
                    path = Path(command[command.index("--output-file") + 1])
                    self.assertFalse(path.exists())
                    if not exit_code:
                        path.write_text(json.dumps(report(results)), encoding="utf-8")
                    return subprocess.CompletedProcess(command, exit_code)

                with patch.object(run_devskim, "install", return_value=Path("/devskim")), \
                        patch.object(run_devskim.subprocess, "run", side_effect=scan), \
                        contextlib.redirect_stdout(io.StringIO()), contextlib.redirect_stderr(io.StringIO()):
                    self.assertNotEqual(0, run_devskim.main(["--output-directory", directory]))
                summary = json.loads((output / "summary.json").read_text())
                self.assertTrue(summary["has_failures"])
                self.assertEqual(9 if exit_code else 0, summary["error_count"])
                self.assertTrue((output / "report.md").exists())
                self.assertTrue((output / "index.html").exists())

    def test_install_uses_root_config_outside_repository(self):
        with tempfile.TemporaryDirectory() as directory:
            output = Path(directory)
            with patch.object(run_devskim.subprocess, "run") as run:
                run.return_value = subprocess.CompletedProcess([], 0, stdout=run_devskim.VERSION + "+commit\n")
                run_devskim.install("/dotnet", output / "tool", output)
            install = run.call_args_list[0]
            command = install.args[0]
            self.assertEqual(str(run_devskim.REPOSITORY / "NuGet.config"),
                             command[command.index("--configfile") + 1])
            self.assertNotEqual(run_devskim.REPOSITORY, Path(install.kwargs["cwd"]))


if __name__ == "__main__":
    unittest.main()
