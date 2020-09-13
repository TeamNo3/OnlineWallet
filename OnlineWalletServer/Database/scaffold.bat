cd %~dp0
dotnet ef dbcontext scaffold "Server=localhost;Database=onlinewallet;User=root;Password=Paul.4899;" "Pomelo.EntityFrameworkCore.MySql" -c WalletDbContext -f -d --no-build -p ".\Database.csproj"
pause
