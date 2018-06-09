docker run -p 5432:5432 --name database -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=nossharp -d postgres
docker run -p 6379:6379 --name redis -d redis
docker run -p 8080:80 --name pgadmin -e PGADMIN_DEFAULT_EMAIL=admin@email.com -e PGADMIN_DEFAULT_PASSWORD=admin --link database:database -d dpage/pgadmin4