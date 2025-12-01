FROM ubuntu:22.04

ENV DEBIAN_FRONTEND=noninteractive

# Todo como root
RUN apt-get update && apt-get install -y \
    curl jq wget unzip git sudo ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Crea usuario docker con sudo sin contraseña
RUN useradd -m -s /bin/bash docker \
    && echo 'docker ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers

# Descarga e instala el runner como root
ARG RUNNER_VERSION=2.317.0
WORKDIR /home/docker
RUN mkdir actions-runner && cd actions-runner \
    && curl -O -L https://github.com/actions/runner/releases/download/v${RUNNER_VERSION}/actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz \
    && tar xzf ./actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz \
    && rm ./actions-runner-linux-x64-${RUNNER_VERSION}.tar.gz \
    && ./bin/installdependencies.sh

# CAMBIO CLAVE: cambia propietario de toda la carpeta actions-runner al usuario docker
RUN chown -R docker:docker /home/docker/actions-runner

# Copia entrypoint y hazlo ejecutable
COPY entrypoint.sh /home/docker/entrypoint.sh
RUN chmod +x /home/docker/entrypoint.sh \
    && chown docker:docker /home/docker/entrypoint.sh

# Cambia a usuario docker
USER docker
WORKDIR /home/docker

ENTRYPOINT ["/home/docker/entrypoint.sh"]