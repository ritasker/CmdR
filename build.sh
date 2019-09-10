#!/usr/bin/env bash
set -euxo pipefail

echo "${0##*/}": Building...
dotnet build --configuration Release

echo "${0##*/}": Testing...
dotnet test --configuration Release --no-build