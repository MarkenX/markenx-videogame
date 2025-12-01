#!/bin/bash
set -e

UNITY_VERSION=$1

echo "Installing Unity modules for version $UNITY_VERSION"

unityhub install --version "$UNITY_VERSION" --changeset d3d30d158480 --childModules WebGL
