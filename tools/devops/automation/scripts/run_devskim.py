# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import argparse
import os
import subprocess
import tempfile
from pathlib import Path

import check_devskim
import report_devskim


VERSION = "1.0.100"
SOURCES = ("runtime", "src", "tools", "msbuild", "external", "dotnet", "scripts", "builds", "eng")
REPOSITORY = Path(__file__).resolve().parents[4]


def install(dotnet, tool_directory, output_directory):
    executable = tool_directory / "devskim"
    if not executable.exists():
        tool_directory.mkdir(parents=True, exist_ok=True)
        with tempfile.TemporaryDirectory() as directory:
            with (output_directory / "install.log").open("w", encoding="utf-8") as log:
                result = subprocess.run(
                    [dotnet, "tool", "install", "Microsoft.CST.DevSkim.CLI", "--version", VERSION,
                     "--tool-path", str(tool_directory), "--configfile", str(REPOSITORY / "NuGet.config")],
                    cwd=directory, stdout=log, stderr=subprocess.STDOUT,
                )
            print((output_directory / "install.log").read_text(encoding="utf-8"))
            result.check_returncode()
    result = subprocess.run([str(executable), "--version"], stdout=subprocess.PIPE, stderr=subprocess.STDOUT,
                            text=True, env={**os.environ, "DOTNET_ROLL_FORWARD": "Major"})
    if result.returncode:
        print(result.stdout)
        result.check_returncode()
    if result.stdout.strip().removeprefix("devskim ").split("+")[0] != VERSION:
        raise ValueError(f"Expected DevSkim {VERSION}, but found {result.stdout.strip()} in {tool_directory}")
    return executable


def main(argv=None):
    parser = argparse.ArgumentParser(description="Run the same DevSkim scan and reports locally and in CI.")
    parser.add_argument("--dotnet", default="dotnet")
    parser.add_argument("--tool-directory", type=Path, default=REPOSITORY / "tests/.devskim/tool")
    parser.add_argument("--output-directory", type=Path, default=REPOSITORY / "tests/.devskim/reports")
    parser.add_argument("--install-only", action="store_true")
    parser.add_argument("--report-only", action="store_true")
    parser.add_argument("--scan-status", default="Succeeded")
    args = parser.parse_args(argv)
    output = args.output_directory.resolve()
    output.mkdir(parents=True, exist_ok=True)
    reports = [output / f"{source}.sarif" for source in SOURCES]
    if args.report_only:
        return report_devskim.main(["--output-directory", str(output), "--scan-status", args.scan_status,
                                   *map(str, reports)])
    if not args.install_only:
        for report in reports:
            report.unlink(missing_ok=True)
    executable = install(args.dotnet, args.tool_directory.resolve(), output)
    if args.install_only:
        return 0
    scanner_failed = False
    for source, report in zip(SOURCES, reports):
        print(f"Scanning {source}", flush=True)
        with (output / f"{source}.log").open("w", encoding="utf-8") as log:
            result = subprocess.run(
                [str(executable), "analyze", "--source-code", str(REPOSITORY / source),
                 "--base-path", str(REPOSITORY), "--output-file", str(report), "--file-format", "sarif",
                 "--severity", "Critical,Important,Moderate,BestPractice,ManualReview",
                 "--confidence", "High,Medium,Low", "--disable-supression", "--disable-parallel",
                 "--options-json", str(REPOSITORY / "tools/devops/governance/devskim-options.json")],
                cwd=REPOSITORY, stdout=log, stderr=subprocess.STDOUT,
                env={**os.environ, "DOTNET_ROLL_FORWARD": "Major"},
            )
        if result.returncode:
            scanner_failed = True
            print(f"DevSkim failed for {source} (exit {result.returncode}):\n"
                  + (output / f"{source}.log").read_text(encoding="utf-8"))
    status = "Failed" if scanner_failed else "Succeeded"
    report_devskim.main(["--output-directory", str(output), "--scan-status", status, *map(str, reports)])
    gate = check_devskim.main(list(map(str, reports)))
    return 2 if scanner_failed else gate


if __name__ == "__main__":
    raise SystemExit(main())
