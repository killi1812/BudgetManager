#!/bin/bash
export PATH="$PATH:/home/fran/.dotnet/tools"
#dotnet-ef dbcontext scaffold "Name=ConnectionStrings:MySqlDb" Microsoft.EntityFrameworkCore -o Models --force --project WebApp
#dotnet-ef dbcontext scaffold "Name=ConnectionStrings:MySqlDb" MySql.EntityFrameworkCore -o Models --force --project WebApi
dotnet-ef dbcontext scaffold "Server=localhost;Database=BudgetManager;Uid=sa;Pwd=password123!;Encrypt=False;TrustServerCertificate=False" Microsoft.EntityFrameworkCore.SqlServer -o Models --force --project Data --no-build
