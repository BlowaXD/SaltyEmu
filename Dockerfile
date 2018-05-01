# Project Builder
FROM microsoft/dotnet:2.1-sdk-alpine as builder

WORKDIR /nossharp

# Copy everything and build
COPY . ./
RUN dotnet publish src/NosSharp.World/ -c Release -o ../../dist/
## Use alpine as basis
FROM microsoft/dotnet:2.1-runtime-alpine

# Local Server Port
ENV SERVER_PORT=1337

# Output Server Port (that will be sent to IServerApiService)
ENV SERVER_OUTPUT_PORT=4000
ENV SERVER_OUTPUT_IP=127.0.0.1

# Env variables used to pull repository
ENV PLUGINS_GIT_URL=ssh@git.gitlab.com/plugins.git
ENV PLUGINS_GIT_SSH_KEY=

LABEL name="nossharp-world"
LABEL author="BlowaXD"
LABEL maintainer="BlowaXD <blowaxd693@gmail.com>"

RUN apk add --no-cache git

# Setup Application Directory
RUN mkdir /nossharp && \
    adduser -S nossharp

WORKDIR /nossharp/

COPY --from=builder /nossharp/dist/ bin
COPY config bin/plugins/config
COPY scripts/run.sh run.sh

# execute rights
RUN chmod -R +r /nossharp/ && \
    chmod +x /nossharp/bin/

USER nossharp
WORKDIR /nossharp/

# SETUP DOCKER HEALTHCHECK
HEALTHCHECK --interval=5s --timeout=5s --start-period=5s --retries=3 CMD [ "netstat -an | grep ${SERVER_PORT} > /dev/null; if [ 0 != $? ]; then exit 1; fi;" ]

# EXPOSE TCP Port
EXPOSE ${SERVER_PORT}/TCP

# RUN NosSharp World Server
ENTRYPOINT [ "run.sh" ]