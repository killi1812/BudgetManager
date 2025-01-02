# Add the .NET tools directory to the PATH environment variable (adjust if needed)
$env:PATH += ";$HOME\.dotnet\tools"

# Scaffold the DbContext and models using dotnet-ef
dotnet-ef dbcontext scaffold "Name=ConnectionStrings:db" Microsoft.EntityFrameworkCore.SqlServer -o Models --force --project Data
