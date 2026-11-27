#!/usr/bin/env bash
# Fails when the merged Cobertura line coverage is below the threshold (in percent).
# Usage: check-coverage.sh <Cobertura.xml> <threshold>
set -euo pipefail

report="$1"
threshold="$2"

[ -f "$report" ] || { echo "coverage report not found: $report" >&2; exit 1; }

line_rate=$(grep -o -m1 'line-rate="[0-9.]*"' "$report" | head -1 | grep -o '[0-9.]*')
branch_rate=$(grep -o -m1 'branch-rate="[0-9.]*"' "$report" | head -1 | grep -o '[0-9.]*')
line=$(awk -v r="$line_rate" 'BEGIN { printf "%.1f", r * 100 }')
branch=$(awk -v r="$branch_rate" 'BEGIN { printf "%.1f", r * 100 }')

echo "Line coverage: ${line}% | Branch coverage: ${branch}% | Required line coverage: ${threshold}%"

if awk -v c="$line" -v t="$threshold" 'BEGIN { exit !(c < t) }'; then
  echo "::error::Line coverage ${line}% is below the required ${threshold}%"
  exit 1
fi
