#!/usr/bin/env bash
# Fails when a direct or transitive package has a known vulnerability.
# "dotnet list package --vulnerable" exits 0 even when it finds some, so its JSON output is inspected.
#
# Usage: check-vulnerable.sh <solution-or-project>
set -euo pipefail

target="$1"

# nuget.org publishes the advisory data; the private feed does not.
report=$(dotnet list "$target" package --vulnerable --include-transitive \
  --source https://api.nuget.org/v3/index.json --format json)

node -e '
  const report = JSON.parse(require("fs").readFileSync(0, "utf8"));
  // An incomplete scan must not pass.
  const errors = (report.problems ?? []).filter(p => p.level === "error").map(p => `Vulnerability scan failed: ${p.text}`);
  const found = (report.projects ?? []).flatMap(project => (project.frameworks ?? []).flatMap(framework =>
    [...(framework.topLevelPackages ?? []), ...(framework.transitivePackages ?? [])].flatMap(pkg =>
      (pkg.vulnerabilities ?? []).map(v =>
        `Vulnerable package: ${pkg.id} ${pkg.resolvedVersion} [${v.severity}] in ${project.path.split(/[\\/]/).pop()}: ${v.advisoryurl}`))));
  const problems = [...new Set([...errors, ...found])];
  for (const line of problems) console.log(`::error::${line}`);
  if (problems.length) process.exit(1);
' <<< "$report"

echo "No vulnerable packages in $target."
