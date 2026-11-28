# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import argparse
import json
import sys
from pathlib import Path
from urllib.parse import urlsplit


BANNED_FUNCTION_TAG = "API.DangerousAPI.BannedFunction"


def object_field(parent, name):
    value = parent.get(name, {})
    if not isinstance(value, dict):
        raise ValueError(f"Expected a {name} object")
    return value


def object_list(parent, name):
    value = parent.get(name, [])
    if not isinstance(value, list) or not all(isinstance(item, dict) for item in value):
        raise ValueError(f"Expected a {name} array of objects")
    return value


def string_field(parent, name):
    value = parent.get(name, "")
    if not isinstance(value, str):
        raise ValueError(f"Expected a {name} string")
    value.encode("utf-8")
    return value


def banned_findings(document):
    if not isinstance(document, dict) or document.get("version") != "2.1.0":
        raise ValueError("Expected SARIF 2.1.0")
    runs = object_list(document, "runs")
    if not runs:
        raise ValueError("Expected at least one SARIF run")

    findings = []
    for run in runs:
        driver = object_field(object_field(run, "tool"), "driver")
        if driver.get("name") != "devskim":
            raise ValueError("Expected a DevSkim run")
        for invocation in object_list(run, "invocations"):
            if "executionSuccessful" in invocation and not isinstance(invocation["executionSuccessful"], bool):
                raise ValueError("Expected an executionSuccessful boolean")
            if invocation.get("executionSuccessful") is False:
                raise ValueError("DevSkim execution was unsuccessful")
            for notification in object_list(invocation, "toolExecutionNotifications"):
                if string_field(notification, "level") == "error":
                    raise ValueError("DevSkim reported an execution error")
        for provenance in object_list(run, "versionControlProvenance"):
            urlsplit(string_field(provenance, "repositoryUri"))
            string_field(provenance, "revisionId")
        for rule in object_list(driver, "rules"):
            if not string_field(rule, "id"):
                raise ValueError("Expected a rule id")
            urlsplit(string_field(rule, "helpUri"))
            string_field(object_field(rule, "help"), "text")
            properties = object_field(rule, "properties")
            for name in ("DevSkimSeverity", "DevSkimConfidence"):
                string_field(properties, name)
        if "results" not in run:
            raise ValueError("Expected a results array")
        for result in object_list(run, "results"):
            if not string_field(result, "ruleId"):
                raise ValueError("Expected a result with a ruleId")
            properties = object_field(result, "properties")
            tags = properties.get("tags")
            # DevSkim 1.0.100 writes rule tags on results, not driver.rules.
            if not isinstance(tags, list) or not all(isinstance(tag, str) for tag in tags):
                raise ValueError("Expected DevSkim result tags")
            for name in ("DevSkimSeverity", "DevSkimConfidence"):
                string_field(properties, name)
            string_field(result, "level")
            string_field(object_field(result, "message"), "text")
            for location in object_list(result, "locations"):
                physical = object_field(location, "physicalLocation")
                urlsplit(string_field(object_field(physical, "artifactLocation"), "uri"))
                region = object_field(physical, "region")
                line = region.get("startLine", 1)
                if type(line) is not int or line < 1:
                    raise ValueError("Expected a positive startLine integer")
                string_field(object_field(region, "snippet"), "text")
            if BANNED_FUNCTION_TAG in tags:
                findings.append(result)
    return findings


def main(argv=None):
    parser = argparse.ArgumentParser(description="Fail on DevSkim banned-function findings, at any severity.")
    parser.add_argument("reports", type=Path, nargs="+")
    args = parser.parse_args(argv)
    count = 0
    for report in args.reports:
        try:
            findings = banned_findings(json.loads(report.read_text(encoding="utf-8")))
        except (OSError, ValueError) as error:
            print(f"DevSkim report error in {report}: {error}", file=sys.stderr)
            return 2
        for finding in findings:
            location = (finding.get("locations") or [{}])[0].get("physicalLocation", {})
            uri = location.get("artifactLocation", {}).get("uri", str(report))
            line = location.get("region", {}).get("startLine", 1)
            message = finding.get("message", {}).get("text", "Banned API detected")
            print(f"{uri}:{line}: {finding['ruleId']}: {message}")
        count += len(findings)
    print(f"DevSkim: {count} banned-function finding(s). Full results are in the devskim artifact.")
    return 1 if count else 0


if __name__ == "__main__":
    sys.exit(main())
