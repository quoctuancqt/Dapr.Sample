#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/OrderService/OrderService.Api/OrderService.Api.csproj", "src/OrderService/OrderService.Api/"]
COPY ["src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "src/OrderService/OrderService.Infrastructure/"]
COPY ["src/OrderService/OrderService.Application/OrderService.Application.csproj", "src/OrderService/OrderService.Application/"]
COPY ["src/Common/SharedKernel/SharedKernel.csproj", "src/Common/SharedKernel/"]
RUN dotnet restore "src/OrderService/OrderService.Api/OrderService.Api.csproj"
COPY . .
WORKDIR "/src/src/OrderService/OrderService.Api"
RUN dotnet build "OrderService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]