docker run --expose 5432:5432 --name database -d postgres \
    -e POSTGRES_USER="admin" \
    -e POSTGRES_PASSWORD="admin" \
    -e POSTGRES_DB="nossharp" \
docker run --expose 6379:6379 --name redis -d redis
docker run --expose 80:8080 --name pgadmin -d dpage/pgadmin4 \
    -e PGADMIN_DEFAULT_EMAIL="admin@email.com" \
    -e PGADMIN_DEFAULT_PASSWORD="admin" \