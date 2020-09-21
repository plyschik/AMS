FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
WORKDIR /app
COPY AMS.MVC.csproj .
RUN dotnet restore
COPY . .
RUN libman restore
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/output .
ENTRYPOINT ["dotnet", "AMS.MVC.dll"]
