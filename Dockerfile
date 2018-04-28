# Project Builder
FROM microsoft/dotnet:2.1-sdk-alpine as builder

USER root
RUN mkdir /nossharp
COPY . /nossharp
RUN chmod +x /nossharp/scripts/publish.sh
RUN /nossharp/scripts/publish.sh


## Use alpine as basis
FROM alpine:latest

ENV SERVER_PORT=1337
ENV PLUGINS_GIT_URL=""
ENV PLUGINS_GIT_USERNAME=""
ENV PLUGINS_GIT_PASSWORD=""

LABEL Name="NosSharp.World"
LABEL Author="BlowaXD"
LABEL MAINTAINER BlowaXD <blowaxd693@gmail.com>

RUN apk update && apk upgrade && \
    apk add --no-cache git

# Setup Application Directory
RUN mkdir /nossharp
RUN adduser -S nossharp

COPY --from=builder --chown=nossharp /nossharp/dist/World/linux /nossharp/bin
COPY --from=builder --chown=nossharp /nossharp/config /nossharp/bin/config
COPY --from=builder --chown=nossharp /nossharp/script/unit_test.sh /nossharp/test/unit_test
COPY --from=builder --chown=nossharp /nossharp/script/integration_test.sh /nossharp/test/integration_test
RUN chmod -R +r /nossharp/
RUN chmod +x /nossharp/bin/NosSharp.World
RUN chmod +x /nossharp/test/unit_test
RUN chmod +x /nossharp/test/integration_test

USER nossharp
WORKDIR /nossharp/

# SETUP DOCKER HEALTHCHECK
HEALTHCHECK --interval=5s --timeout=5s --start-period=5s --retries=3 CMD [ "scripts/healthcheck.sh" ]

# EXPOSE TCP Port
EXPOSE ${SERVER_PORT}/TCP

# RUN NosSharp World Server
ENTRYPOINT [ "scripts/run.sh" ]