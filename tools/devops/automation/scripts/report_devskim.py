# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

import argparse
import html
import json
import re
from collections import Counter
from pathlib import Path
from urllib.parse import quote, unquote, urlsplit

from check_devskim import BANNED_FUNCTION_TAG, banned_findings


def escape(value):
    return html.escape(str(value), quote=True)


def web_url(value):
    parsed = urlsplit(value)
    return value if parsed.scheme in ("https", "http") and parsed.netloc else ""


def source_url(run, uri, line):
    provenance = (run.get("versionControlProvenance") or [{}])[0]
    repository = provenance.get("repositoryUri", "").removesuffix(".git").rstrip("/")
    revision = provenance.get("revisionId", "")
    path = unquote(uri)
    if (not re.fullmatch(r"https://github\.com/[\w.-]+/[\w.-]+", repository)
            or not re.fullmatch(r"[a-fA-F0-9]{40}", revision)
            or urlsplit(path).scheme or path.startswith(("/", "\\"))
            or "\\" in path or ".." in path.split("/")):
        return ""
    return f"{repository}/blob/{revision}/{quote(path, safe='/')}#L{line}"


def collect_reports(paths, scan_status):
    findings = []
    errors = []
    sources = []
    for path in paths:
        try:
            document = json.loads(path.read_text(encoding="utf-8"))
            banned_findings(document)
        except (OSError, ValueError) as error:
            errors.append(f"{path.name}: {error}")
            continue
        sources.append(path.name)
        for run in document["runs"]:
            rules = {rule["id"]: rule for rule in run["tool"]["driver"].get("rules", [])}
            for result in run["results"]:
                rule = rules.get(result["ruleId"], {})
                properties = result["properties"]
                location = (result.get("locations") or [{}])[0].get("physicalLocation", {})
                region = location.get("region", {})
                uri = location.get("artifactLocation", {}).get("uri", path.name)
                line = region.get("startLine", 1)
                findings.append({
                    "banned": BANNED_FUNCTION_TAG in properties["tags"],
                    "rule": result["ruleId"],
                    "severity": properties.get("DevSkimSeverity",
                                               rule.get("properties", {}).get("DevSkimSeverity",
                                                                             result.get("level", "warning"))),
                    "confidence": properties.get("DevSkimConfidence",
                                                 rule.get("properties", {}).get("DevSkimConfidence", "")),
                    "file": uri,
                    "line": line,
                    "url": source_url(run, uri, line),
                    "message": result.get("message", {}).get("text", result["ruleId"]),
                    "snippet": region.get("snippet", {}).get("text", ""),
                    "guidance": rule.get("help", {}).get("text", ""),
                    "help_url": web_url(rule.get("helpUri", "")),
                })
    banned_count = sum(finding["banned"] for finding in findings)
    if scan_status != "Succeeded" and not errors and not banned_count:
        errors.append(f"The scan job status was {scan_status}. Inspect the pipeline and scanner logs.")
    summary = {
        "banned_count": banned_count,
        "finding_count": len(findings),
        "error_count": len(errors),
        "has_failures": bool(banned_count or errors),
        "sources": sources,
        "errors": errors,
    }
    return findings, summary


PAGE = """<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>DevSkim source analysis</title>
<style>
:root { color-scheme: light dark; --bg: #f5f7fb; --surface: #fff; --text: #18243a;
  --muted: #526179; --border: #d7dfeb; --accent: #185abd; --bad: #b42318; --good: #147d45; }
@media (prefers-color-scheme: dark) { :root { --bg: #101725; --surface: #182235; --text: #e7edf7;
  --muted: #b1bed2; --border: #35435c; --accent: #8bbaff; --bad: #ffaba2; --good: #80d5a5; } }
* { box-sizing: border-box; } body { margin: 0; background: var(--bg); color: var(--text);
  font: 15px/1.55 system-ui, sans-serif; } main { max-width: 1540px; margin: auto; padding: 36px 24px; }
h1 { margin: 8px 0; font-size: 32px; } h2 { font-size: 20px; } p { margin: 8px 0 16px; }
a { color: var(--accent); } .eyebrow, .muted { color: var(--muted); } .eyebrow { letter-spacing: .12em; }
.cards { display: flex; gap: 16px; flex-wrap: wrap; margin: 24px 0; }
.card { flex: 1; min-width: 150px; background: var(--surface); padding: 18px 24px;
  border: 1px solid var(--border); border-radius: 12px; }
.number { display: block; font-size: 32px; font-weight: 700; }
.bad { color: var(--bad); } .good { color: var(--good); }
.panel { background: var(--surface); border: 1px solid var(--border); border-radius: 12px;
  padding: 20px; margin-bottom: 20px; } .controls { display: flex; gap: 16px; flex-wrap: wrap; }
label { display: grid; gap: 6px; } input, select { font: inherit; background: var(--surface);
  color: var(--text); border: 1px solid var(--border); border-radius: 6px; padding: 9px 12px; }
input { width: min(440px, 80vw); } .table-wrap { overflow-x: auto; }
table { width: 100%; border-collapse: collapse; } th, td { text-align: left; vertical-align: top;
  padding: 14px; border-bottom: 1px solid var(--border); } th { background: var(--surface); }
td.file { min-width: 270px; overflow-wrap: anywhere; } td.issue { min-width: 320px; }
.badge { font-size: 12px; font-weight: 600; white-space: nowrap; }
pre { white-space: pre-wrap; overflow-wrap: anywhere; font-size: 13px; padding: 10px;
  border: 1px solid var(--border); border-radius: 6px; } details { margin-top: 8px; }
summary { cursor: pointer; color: var(--muted); } footer { margin-top: 24px; color: var(--muted); }
[hidden] { display: none !important; }
</style>
</head>
<body><main>
<div class="eyebrow">SECURITY / SOURCE ANALYSIS</div>
<h1>DevSkim report</h1>
<!--STATUS-->
<p class="muted">Banned-function findings at every severity fail the gate. Other findings are included
for triage; they do not fail this check. Findings are not proof of exploitable vulnerabilities.</p>
<div class="cards"><!--CARDS--></div>
<!--ERRORS-->
<section class="panel"><h2>Scan coverage</h2><p><!--SOURCES--></p>
<p class="muted">Production sources, tooling, templates, and checked-out dependencies.
Tests, documentation, translations, and later-generated/downloaded build outputs are not scanned.</p>
<p><!--SEVERITIES--></p><!--LOGS--></section>
<section class="panel">
<div class="controls">
<label>Search findings<input id="search" type="search" placeholder="File, rule, API, or message"></label>
<label>Finding category<select id="category"><option value="banned">Banned APIs (blocking)</option>
<option value="all">All findings</option><option value="other">Other findings</option></select></label>
<label>Severity<select id="severity"><option value="">All severities</option><!--OPTIONS--></select></label>
</div><p id="visible" class="muted" aria-live="polite"></p>
<noscript><p>JavaScript is disabled. All findings are listed below.</p></noscript>
<div class="table-wrap"><table>
<thead><tr><th>Severity</th><th>Rule</th><th>File / line</th><th>Finding / guidance</th></tr></thead>
<tbody><!--ROWS--></tbody></table></div></section>
<footer>Generated from DevSkim SARIF. No findings have been baselined or suppressed by this report.</footer>
</main>
<script>
const search = document.getElementById('search');
const category = document.getElementById('category');
const severity = document.getElementById('severity');
const rows = Array.from(document.querySelectorAll('tbody tr'));
if (!rows.some(row => row.dataset.banned === 'true')) category.value = 'all';
function filter() {
  const query = search.value.toLowerCase();
  let visible = 0;
  for (const row of rows) {
    const matchesCategory = category.value === 'all' ||
      (row.dataset.banned === 'true') === (category.value === 'banned');
    row.hidden = !matchesCategory || (severity.value && row.dataset.severity !== severity.value) ||
      !row.textContent.toLowerCase().includes(query);
    if (!row.hidden) visible++;
  }
  document.getElementById('visible').textContent = `${visible} of ${rows.length} findings shown`;
}
search.addEventListener('input', filter);
category.addEventListener('change', filter);
severity.addEventListener('change', filter);
filter();
</script></body></html>
"""


def render_html(findings, summary, logs):
    severity_order = {"Critical": 0, "Important": 1, "Moderate": 2, "BestPractice": 3, "ManualReview": 4}
    findings = sorted(findings, key=lambda f: (not f["banned"], severity_order.get(f["severity"], 5),
                                             f["file"], f["line"], f["rule"]))
    rows = []
    for finding in findings:
        file_text = f'{escape(finding["file"])}:{escape(finding["line"])}'
        if finding["url"]:
            file_text = f'<a href="{escape(finding["url"])}">{file_text}</a>'
        guidance = ""
        if finding["snippet"]:
            guidance += f'<pre>{escape(finding["snippet"])}</pre>'
        if finding["guidance"]:
            guidance += f'<p>{escape(finding["guidance"])}</p>'
        if finding["help_url"]:
            guidance += f'<a href="{escape(finding["help_url"])}">Rule documentation</a>'
        if guidance:
            guidance = f"<details><summary>Excerpt and guidance</summary>{guidance}</details>"
        badge = '<div class="badge bad">BANNED API</div>' if finding["banned"] else '<div class="muted">Other</div>'
        rows.append(f'<tr data-banned="{str(finding["banned"]).lower()}" '
                    f'data-severity="{escape(finding["severity"])}">'
                    f'<td>{escape(finding["severity"])}{badge}</td>'
                    f'<td><code>{escape(finding["rule"])}</code>'
                    f'<div class="muted">{escape(finding["confidence"])}</div></td>'
                    f'<td class="file">{file_text}</td>'
                    f'<td class="issue">{escape(finding["message"])}{guidance}</td></tr>')
    errors = ""
    if summary["errors"]:
        errors = '<section class="panel"><h2 class="bad">Scan errors / incomplete coverage</h2><ul>'
        errors += "".join(f"<li>{escape(error)}</li>" for error in summary["errors"]) + "</ul></section>"
    cards = "".join(f'<div class="card"><span class="number">{count}</span>{label}</div>' for count, label in [
        (summary["banned_count"], "Blocking banned-API findings"),
        (summary["finding_count"] - summary["banned_count"], "Other findings"),
        (len(summary["sources"]), "SARIF reports read"),
        (summary["error_count"], "Scan errors"),
    ])
    counts = Counter(f["severity"] for f in findings if f["banned"])
    severities = "Banned findings: " + (", ".join(f"{escape(s)}: {n}" for s, n in sorted(counts.items())) or "none")
    log_links = " &middot; ".join(f'<a href="{quote(name)}">{escape(name)}</a>' for name in logs)
    failed = summary["has_failures"]
    replacements = {
        "STATUS": f'<p class="{"bad" if failed else "good"}"><strong>'
                  f'{"Failed: findings or scan errors require attention" if failed else "Passed: no banned-API findings"}'
                  '</strong></p>',
        "CARDS": cards,
        "ERRORS": errors,
        "SOURCES": ", ".join(escape(source) for source in summary["sources"]) or "No valid SARIF reports.",
        "SEVERITIES": severities,
        "LOGS": f"<p>Scanner logs: {log_links}</p>" if log_links else "",
        "OPTIONS": "".join(f'<option value="{escape(s)}">{escape(s)}</option>'
                           for s in sorted({f["severity"] for f in findings})),
        "ROWS": "\n".join(rows),
    }
    # Replace placeholders in the template only, never in untrusted SARIF text.
    return re.sub(r"<!--([A-Z]+)-->", lambda match: replacements[match[1]], PAGE)


def main(argv=None):
    parser = argparse.ArgumentParser(description="Create a self-contained HTML report from DevSkim SARIF.")
    parser.add_argument("--output-directory", type=Path, required=True)
    parser.add_argument("--scan-status", default="Succeeded")
    parser.add_argument("reports", type=Path, nargs="+")
    args = parser.parse_args(argv)
    findings, summary = collect_reports(args.reports, args.scan_status)
    args.output_directory.mkdir(parents=True, exist_ok=True)
    logs = sorted(path.name for path in args.output_directory.glob("*.log"))
    (args.output_directory / "index.html").write_text(render_html(findings, summary, logs), encoding="utf-8")
    (args.output_directory / "summary.json").write_text(json.dumps(summary, indent=2) + "\n", encoding="utf-8")
    print(f'DevSkim HTML report: {args.output_directory / "index.html"}')
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
