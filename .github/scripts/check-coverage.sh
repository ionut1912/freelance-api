#!/usr/bin/env bash
# Fails when the merged Cobertura line coverage, overall or of ANY class (source file), is below the threshold.
# Usage: check-coverage.sh <Cobertura.xml> <threshold-percent>
set -euo pipefail

report="$1"
threshold="$2"

[ -f "$report" ] || { echo "coverage report not found: $report" >&2; exit 1; }

line_rate=$(grep -o -m1 'line-rate="[0-9.]*"' "$report" | head -1 | grep -o '[0-9.]*')
branch_rate=$(grep -o -m1 'branch-rate="[0-9.]*"' "$report" | head -1 | grep -o '[0-9.]*')
line=$(awk -v r="$line_rate" 'BEGIN { printf "%.1f", r * 100 }')
branch=$(awk -v r="$branch_rate" 'BEGIN { printf "%.1f", r * 100 }')
echo "Line coverage: ${line}% | Branch coverage: ${branch}% | Required per class and overall: ${threshold}%"

# One line per class below the threshold: "<percent> <class> (<file>)".
below=$(awk -v t="$threshold" '
  /<class / {
    match($0, /name="[^"]*"/); name = substr($0, RSTART + 6, RLENGTH - 7)
    match($0, /filename="[^"]*"/); file = substr($0, RSTART + 10, RLENGTH - 11)
    match($0, /line-rate="[0-9.]*"/); rate = substr($0, RSTART + 11, RLENGTH - 12) * 100
    if (rate < t) printf "%.1f%% %s (%s)\n", rate, name, file
  }' "$report")

status=0
if awk -v c="$line" -v t="$threshold" 'BEGIN { exit !(c < t) }'; then
  echo "::error::Overall line coverage ${line}% is below the required ${threshold}%"
  status=1
fi
if [ -n "$below" ]; then
  while IFS= read -r entry; do echo "::error::Class below ${threshold}% line coverage: $entry"; done <<< "$below"
  status=1
else
  echo "Every class has at least ${threshold}% line coverage."
fi
exit $status
