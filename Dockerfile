###
## Docker used to build
### 
# Project Builder
FROM microsoft/dotnet:2.0-sdk as builder

WORKDIR /nossharp

# Copy everything and build
COPY . ./
RUN dotnet restore
RUN dotnet publish . -c Release -o ../../dist/


###
## Real Docker
###

## Use alpine as basis
FROM microsoft/dotnet:2.0-runtime

# Output Server Port (that will be sent to IServerApiService)
ENV SERVER_PORT=7777 \
    SERVER_IP=127.0.0.1 \
    SERVER_WORLDGROUP=NosWings

LABEL name="nossharp-world" \
    author="BlowaXD" \
    maintainer="BlowaXD <blowaxd693@gmail.com>"

# Setup Application Directory
RUN mkdir /nossharp && \
    mkdir /nossharp/plugins && \
    mkdir /nossharp/plugins/config && \
    chmod -R +r /nossharp/ && \
    chmod -R +x /nossharp

WORKDIR /nossharp/

COPY --from=builder /nossharp/dist/ .

VOLUME /nossharp/plugins

# SETUP DOCKER HEALTHCHECK
# HEALTHCHECK --interval=5s --timeout=5s --start-period=5s --retries=3 CMD [ "" ]


# RUN NosSharp World Server
# COPY scripts/docker-entrypoint.sh /usr/local/bin/
# ENTRYPOINT [ "docker-entrypoint.sh" ]

# EXPOSE TCP Port
EXPOSE 1337

CMD ["dotnet", "NosSharp.World.dll"]