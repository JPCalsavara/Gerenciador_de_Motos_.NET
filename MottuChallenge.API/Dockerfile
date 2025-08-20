FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copia todos os ficheiros do projeto para o container.
# O .dockerignore irá excluir as pastas bin e obj.
COPY . .

# Executa o publish. Este comando irá restaurar as dependências e compilar
# a aplicação de uma só vez, o que é mais robusto em ambientes Docker.
RUN dotnet publish "MottuChallenge.API.csproj" -c Release -o /app/publish

# Estágio 2: Final - Usamos a imagem de runtime, que é menor e mais segura
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MottuChallenge.API.dll"]