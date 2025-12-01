#!/bin/bash
set -e

if [ -z "$GH_TOKEN" ]; then echo "Error: GH_TOKEN no definido"; exit 1; fi

cd /home/docker/actions-runner

if [ ! -f ".runner" ]; then
  echo "Configurando runner..."
  ./config.sh --unattended \
    --url "https://github.com/${GH_OWNER}/${GH_REPOSITORY}" \
    --token "${GH_TOKEN}" \
    --name "${RUNNER_NAME:-docker-runner}" \
    --labels "${RUNNER_LABELS:-self-hosted,linux,docker,unity-webgl}" \
    --work _work \
    --replace
  touch .runner
fi

echo "Runner activo y escuchando jobs..."
exec ./run.sh