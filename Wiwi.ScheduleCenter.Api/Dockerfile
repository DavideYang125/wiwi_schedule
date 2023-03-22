#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Wiwi.ScheduleCenter.Api/Wiwi.ScheduleCenter.Api.csproj", "Wiwi.ScheduleCenter.Api/"]
COPY ["Wiwi.ScheduleCenter.Common/Wiwi.ScheduleCenter.Common.csproj", "Wiwi.ScheduleCenter.Common/"]
COPY ["Wiwi.ScheduleCenter.Core/Wiwi.ScheduleCenter.Core.csproj", "Wiwi.ScheduleCenter.Core/"]
RUN dotnet restore "Wiwi.ScheduleCenter.Api/Wiwi.ScheduleCenter.Api.csproj"
COPY . .
WORKDIR "/src/Wiwi.ScheduleCenter.Api"
RUN dotnet build "Wiwi.ScheduleCenter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wiwi.ScheduleCenter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
ENTRYPOINT ["dotnet", "Wiwi.ScheduleCenter.Api.dll"]