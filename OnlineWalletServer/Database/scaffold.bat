cd %~dp0
dotnet ef dbcontext scaffold "Server=localhost;Database=onlinewallet;User=root;Password=1111;" "Pomelo.EntityFrameworkCore.MySql" -c WalletDbContext -f -d --no-build -p ".\Database.csproj"
pause
