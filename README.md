# SecretMagic.Server
The server of Secret Magic blog 

Local develop run:
1. change database connection string
2. dotnet tool install --global dotnet-ef
3. change to \SecretMagic.Server\SecretMagic.API
4. dotnet ef database update
5. start the project and view swagger API to execute API `/api/Admin/InitialDatabase/{secret}` 

Docker deploy:
1. docker build . -t secretmagic.server:latest
2. docker run -itd --name secretmagic.server -p 5000:80 secretmagic.server
3. browser http://localhost:5000/index.html to view the API.