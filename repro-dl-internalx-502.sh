#!/bin/bash
# Repro script for dl.internalx.com 502 timeout under concurrent load
#
# Root cause: The dl.internalx.com service performs sequential KeyVault
# secret scanning for token validation (GitHub.fs:inKeyVault). Under
# concurrent load (like 20+ CI jobs), the service can't keep up and
# exceeds the Azure App Service 120s request timeout → 502.
#
# Usage:
#   ./repro-dl-internalx-502.sh <token> sequential    # 3 requests, one at a time (works)
#   ./repro-dl-internalx-502.sh <token> parallel      # 20 requests, all at once (502s)
#   ./repro-dl-internalx-502.sh <token> parallel 5    # 5 requests, all at once
#   ./repro-dl-internalx-502.sh <token> both          # run sequential then parallel
#
# To get the token:
#   az keyvault secret show --vault-name xamarin-secrets \
#     --name "github--pat--vs-mobiletools-engineering-service2" --query "value" -o tsv

set -euo pipefail

if [[ $# -lt 1 || "$1" == "-h" || "$1" == "--help" ]]; then
    echo "Usage: $0 <github-token> {sequential|parallel|both} [count]"
    exit 1
fi

TOKEN="$1"
shift

URL="https://dl.internalx.com/provisionator/664bd334021e3102cdef1af66c4fc9f1b2ecd2a21b47419e80d08da1f6c61c2a/latest/version"
MAX_TIME=130

run_sequential() {
    local count=${1:-3}
    echo "============================================"
    echo "  SEQUENTIAL TEST ($count requests)"
    echo "============================================"
    echo ""
    for i in $(seq 1 "$count"); do
        echo -n "  Request $i: "
        curl -s -o /dev/null -w "HTTP %{http_code}  (%{time_total}s)\n" \
            -H "Authorization: token $TOKEN" "$URL" --max-time $MAX_TIME
    done
    echo ""
}

run_parallel() {
    local count=${1:-20}
    echo "============================================"
    echo "  PARALLEL TEST ($count concurrent requests)"
    echo "============================================"
    echo "  Simulates $count CI jobs hitting Provisionator simultaneously"
    echo ""

    local tmpdir
    tmpdir=$(mktemp -d)

    for i in $(seq 1 "$count"); do
        (
            result=$(curl -s -o /dev/null -w "%{http_code} %{time_total}" \
                -H "Authorization: token $TOKEN" "$URL" --max-time $MAX_TIME)
            echo "$result" > "$tmpdir/$i"
        ) &
    done

    echo "  Waiting for all $count requests to complete..."
    echo "  (this may take up to ${MAX_TIME}s if they timeout)"
    echo ""
    wait

    local pass=0 fail=0
    for i in $(seq 1 "$count"); do
        read -r code time < "$tmpdir/$i"
        status="✅"
        if [[ "$code" -ge 400 ]]; then
            status="❌"
            ((fail++))
        else
            ((pass++))
        fi
        printf "  Job %2d: HTTP %s  (%ss) %s\n" "$i" "$code" "$time" "$status"
    done

    echo ""
    echo "  Results: $pass passed, $fail failed out of $count"
    rm -rf "$tmpdir"
    echo ""
}

mode=${1:-both}
count=${2:-20}

if [[ ${#TOKEN} -lt 10 ]]; then
    echo "ERROR: Token looks too short. Provide a valid GitHub PAT."
    exit 1
fi

echo ""
echo "dl.internalx.com 502 Repro Script"
echo "Token: ${TOKEN:0:8}... (vs-mobiletools-engineering-service2)"
echo "URL:   ${URL:0:60}..."
echo ""

case "$mode" in
    sequential)
        run_sequential "$count"
        ;;
    parallel)
        run_parallel "$count"
        ;;
    both)
        run_sequential 3
        echo "--- Sequential works fine. Now testing parallel (like CI)... ---"
        echo ""
        run_parallel "$count"
        ;;
    *)
        echo "Usage: $0 {sequential|parallel|both} [count]"
        exit 1
        ;;
esac
