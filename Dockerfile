FROM microsoft/dotnet:2.1-runtime-alpine

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

ADD --chown=nossharp ./bin/Login /nossharp/bin
ADD --chown=nossharp ./config /nossharp/bin/config
ADD --chown=nossharp ./script/unit_test.sh /nossharp/test/unit_test
ADD --chown=nossharp ./script/integration_test.sh /nossharp/test/integration_test
RUN chmod -R +r /nossharp/
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