# Project Builder
FROM microsoft/dotnet:2.1-sdk-alpine as builder

WORKDIR /nossharp

# Copy everything and build
COPY . ./
RUN dotnet publish src/NosSharp.World/ -c Release -o ../../dist/

## Use alpine as basis
FROM microsoft/dotnet:2.1-runtime-alpine

ENV SERVER_PORT=1337
ENV SERVER_OUTPUT_PORT=4000
ENV SERVER_OUTPUT_IP=127.0.0.1

ENV PLUGINS_GIT_URL=https://pluginrepo.com/plugins.git
ENV PLUGINS_GIT_USERNAME=pluginRepo
ENV PLUGINS_GIT_PASSWORD=pluginRepoPassword

LABEL Name="NosSharp.World"
LABEL Author="BlowaXD"
LABEL MAINTAINER BlowaXD <blowaxd693@gmail.com>

RUN apk update && apk upgrade && \
    apk add --no-cache git

# Setup Application Directory
RUN mkdir /nossharp && \
    adduser -S nossharp

WORKDIR /nossharp/

COPY --from=builder /nossharp/dist/World/linux/ bin
COPY config bin/plugins/config
COPY scripts/run.sh run.sh

# execute rights
RUN chmod -R +r /nossharp/ && \
    chmod +x /nossharp/bin/NosSharp.World

USER nossharp
WORKDIR /nossharp/

# SETUP DOCKER HEALTHCHECK
HEALTHCHECK --interval=5s --timeout=5s --start-period=5s --retries=3 CMD [ "scripts/healthcheck.sh" ]

# EXPOSE TCP Port
EXPOSE ${SERVER_PORT}/TCP

# RUN NosSharp World Server
ENTRYPOINT [ "run.sh" ]