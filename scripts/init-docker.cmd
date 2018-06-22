docker run -p 1433:1433 -e ACCEPT_EULA=Y -e SA_PASSWORD=DevNos#2018 --name database -d mssql-server-linux:latest
docker run -p 8000:6379 --name redis-session -d redis:latest
docker run -p 8500:6379 --name redis-language -d redis:latest