FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base

EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

RUN apt-get update
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

COPY ["ClickUI/ClickUI.csproj", "ClickUI/"]
RUN dotnet restore "ClickUI/ClickUI.csproj"
COPY . .
WORKDIR "/src/ClickUI"
RUN dotnet build "ClickUI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ClickUI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ClickUI.dll"]