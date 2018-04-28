FROM microsoft/dotnet:2.1-runtime-alpine

ENV SERVER_PORT=4000
LABEL name="NosSharp.Login"
LABEL Author="BlowaXD"

# Setup Application Directory
RUN mkdir /nossharp
RUN adduser -S nossharp

ADD --chown=nossharp ./bin/Login /nossharp/bin
ADD --chown=nossharp ./config /nossharp/bin/config
RUN chmod -R +r /nossharp/

USER nossharp
WORKDIR /nossharp/

# SETUP DOCKER HEALTHCHECK
HEALTHCHECK --interval=5s --timeout=5s --start-period=5s --retries=3 CMD [ "scripts/healthcheck.sh" ]

# EXPOSE TCP Port
EXPOSE ${SERVER_PORT}/TCP

# RUN NosSharp Login Server
CMD ["dotnet", "bin/NosSharp.Login.dll"]