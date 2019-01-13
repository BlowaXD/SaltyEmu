###
## Build docker
### 
FROM microsoft/dotnet:2.2-sdk-alpine as builder

WORKDIR /saltyemu/

# Copy everything
COPY ./src/ ./
# build World as publishable application in Release mode
RUN dotnet publish ./Login/Login.csproj -c Release -o /saltyemu/dist/

###
## Runtime Docker
###

## Use alpine as basis
FROM microsoft/dotnet:2.2-runtime-alpine

# Server listening port
ENV SERVER_PORT=4000

LABEL name="saltyemu-login" \
    author="BlowaXD" \
    maintainer="BlowaXD <blowaxd693@gmail.com>"

# Setup Application Directory
RUN mkdir /server && \
    mkdir /server/plugins && \
    mkdir /server/plugins/config && \
    chmod -R +r /server/ && \
    chmod -R +x /server

WORKDIR /server/

COPY --from=builder /saltyemu/dist/ .

VOLUME /server/plugins

# SETUP DOCKER HEALTHCHECK
# Every 5 seconds, it will try to curl
# HEALTHCHECK --interval=15s --timeout=5s --start-period=5s --retries=3 CMD [ "netstat -an | grep $SERVER_PORT > /dev/null; if [ 0 != $? ]; then exit 1; fi;" ]

# Expose port 7777 tcp
EXPOSE 4000

ENTRYPOINT [ "dotnet", "/server/Login.dll" ]