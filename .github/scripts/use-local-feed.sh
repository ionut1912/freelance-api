#!/usr/bin/env bash
# Registers a local folder as a NuGet source for Freelance.Shared.Events in the given nuget.config.
# NuGet source mapping picks the most specific pattern, so the exact "Freelance.Shared.Events" mapping
# to the local feed wins over the "Freelance.*" mapping to the GitHub feed.
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

echo "Using local feed $feed:"
ls "$feed"
