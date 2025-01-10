@echo off
set PATH=%PATH%;%USERPROFILE%\.dotnet\tools

dotnet-ef dbcontext scaffold "Server=localhost,1433;Database=BudgetManager;User Id=sa;Password=password123!;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -o Models --force --project Data
pause
