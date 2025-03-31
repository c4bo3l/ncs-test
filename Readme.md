# DB Setting
- Type: Postgres
- Username: dbuser
- Password: p4assWord
- Database: ncs_test
  
# Run Backend
## Requirements
- .Net 8
- PostgreSQL server
## Prerequisites
- Change the value of `ConnectionStrings.AppContext` in the `Backend/AppServer/appsettings.json` file to have the connection string of the DB server. Here is the example.
  ```
  "Server=localhost;Database=ncs_test;Port=5432;User Id=dbuser;Password=p4assWord;"
  ```
## How to run
Go to the `AppServer` folder inside the `Backend` folder then run this command on your terminal
```
dotnet run
```

# Run Frontend
## Requirements
- Node 18+
## Prerequisites
- Run `npm i` first inside the `WebApp` folder.
- Change the `.env` file inside the `WebApp` folder. Put your backend URL to `VITE_API_BASE_URL` variable. The default value is
  ```
  VITE_API_BASE_URL=http://localhost:5127
  ```
## How to run
- Run this command on your terminal inside the `WebApp` folder.
  ```
  npm run dev
  ```