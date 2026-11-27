#!/usr/bin/env bash
# Registers a local folder as a NuGet source for Freelance.Shared.Events in the given nuget.config.
# NuGet source mapping picks the most specific pattern, so the exact "Freelance.Shared.Events" mapping
# to the local feed wins over the "Freelance.*" mapping to the GitHub feed.
#
# The package is rebuilt on every run, so its hash never matches the committed packages.lock.json files.
# Only that one hash is rewritten (in the CI checkout), which keeps every other package strictly locked.
#
# Usage: use-local-feed.sh <path/to/nuget.config> <feed-directory>
set -euo pipefail

config="$1"
feed="$2"

[ -f "$config" ] || { echo "nuget.config not found: $config" >&2; exit 1; }
ls "$feed"/*.nupkg > /dev/null 2>&1 || { echo "no .nupkg found in $feed" >&2; exit 1; }

if ! grep -q 'key="local"' "$config"; then
  sed -i "s|    <clear />|    <clear />\n    <add key=\"local\" value=\"$feed\" />|" "$config"
  sed -i "s|  <packageSourceMapping>|  <packageSourceMapping>\n    <packageSource key=\"local\">\n      <package pattern=\"Freelance.Shared.Events\" />\n    </packageSource>|" "$config"
fi

# A copy cached by an earlier run has the same version number and would be used instead of the new build.
rm -rf "${NUGET_PACKAGES:-$HOME/.nuget/packages}/freelance.shared.events"

package=$(ls "$feed"/Freelance.Shared.Events.*.nupkg | head -1)
hash=$(openssl dgst -sha512 -binary "$package" | base64 | tr -d '\n')
find "$(dirname "$config")" -name packages.lock.json -print0 | while IFS= read -r -d '' lock; do
  node -e '
    const fs = require("fs");
    const [file, hash] = process.argv.slice(1);
    const lock = JSON.parse(fs.readFileSync(file, "utf8").replace(/^﻿/, ""));
    let changed = false;
    for (const framework of Object.values(lock.dependencies)) {
      const entry = framework["Freelance.Shared.Events"];
      if (entry?.contentHash) { entry.contentHash = hash; changed = true; }
    }
    if (changed) fs.writeFileSync(file, JSON.stringify(lock, null, 2));
  ' "$lock" "$hash"
done

echo "Using local feed $feed:"
ls "$feed"
