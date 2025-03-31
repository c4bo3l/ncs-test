# DB Setting
- Type: Postgres
- Username: dbuser
- Password: p4assWord
- Database: ncs_test
```
CREATE DATABASE ncs_test;
CREATE USER dbuser WITH ENCRYPTED PASSWORD 'p4assWord';
GRANT ALL PRIVILEGES ON DATABASE ncs_test TO dbuser;
GRANT ALL ON SCHEMA public TO dbuser;
```

# Add migration
```
dotnet ef migrations add <migration_name> -p Infrastructure.Database -s AppServer
```

# Remove migration
```
dotnet ef migrations remove -p Infrastructure.Database -s AppServer
```

# Apply migration
```
dotnet ef database update -p Infrastructure.Database -s AppServer
```

# Install dev certificate
```
dotnet dev-certs https --verbose --trust
```