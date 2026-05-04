# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/1 - Presentation/ToDoListChallenge.Presentation.API/ToDoListChallenge.Presentation.API.csproj", "src/1 - Presentation/ToDoListChallenge.Presentation.API/"]
COPY ["src/2 - Application/ToDoListChallenge.Application/ToDoListChallenge.Application.csproj", "src/2 - Application/ToDoListChallenge.Application/"]
COPY ["src/3 - Domain/ToDoListChallenge.Domain/ToDoListChallenge.Domain.csproj", "src/3 - Domain/ToDoListChallenge.Domain/"]
COPY ["src/3 - Domain/ToDoListChallenge.Domain.Core/ToDoListChallenge.Domain.Core.csproj", "src/3 - Domain/ToDoListChallenge.Domain.Core/"]
COPY ["src/4 - Infra/4.1 - Data/ToDoListChallenge.Infra.Data/ToDoListChallenge.Infra.Data.csproj", "src/4 - Infra/4.1 - Data/ToDoListChallenge.Infra.Data/"]
COPY ["src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.Identity/ToDoListChallenge.CrossCutting.Identity.csproj", "src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.Identity/"]
COPY ["src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.IoC/ToDoListChallenge.CrossCutting.IoC.csproj", "src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.IoC/"]
COPY ["src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.Bus/ToDoListChallenge.CrossCutting.Bus.csproj", "src/4 - Infra/4.2 - CrossCutting/ToDoListChallenge.CrossCutting.Bus/"]
RUN dotnet restore "./src/1 - Presentation/ToDoListChallenge.Presentation.API/ToDoListChallenge.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/1 - Presentation/ToDoListChallenge.Presentation.API"
RUN dotnet build "./ToDoListChallenge.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ToDoListChallenge.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoListChallenge.Presentation.API.dll"]