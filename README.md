# 🏍️ Gerenciador de Motos .NET

Este projeto é uma solução backend desenvolvida em .NET para o gerenciamento de motos, entregadores e locações. A aplicação foi construída com foco em arquitetura limpa, garantindo um código testável e manutenível. O sistema possui tratamento de erros robusto e utiliza o Swagger para a documentação da sua API RestFul, facilitando a integração e o teste das funcionalidades.

## 🚀 Tecnologias Utilizadas

- **Linguagem**: C#
- **Framework**: .NET 8 (ASP.NET Core)
- **Banco de Dados**: PostgreSQL
- **Mensageria**: RabbitMQ
- **Storage de Ficheiros**: AWS S3 (para fotos da CNH)
- **Containerização**: Docker e Docker Compose

## 📋 Pré-requisitos

Para executar o projeto localmente, você precisa ter as seguintes ferramentas instaladas:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker e Docker Compose](https://docs.docker.com/get-docker/)

## ⚙️ Configuração da AWS S3

A API está configurada para guardar as fotos da CNH num bucket da AWS S3. Para que esta funcionalidade funcione, siga os passos abaixo:

### 1. Criar um Bucket na S3
Crie um novo bucket no serviço S3 da AWS. O nome do bucket deve ser único globalmente. Por questões de segurança, não ative o acesso público no bucket inteiro.

### 2. Criar um Utilizador IAM com Permissões Restritas
Crie um utilizador dedicado para a sua aplicação no serviço IAM (Identity and Access Management) da AWS. Em vez de dar acesso total ao S3, anexe a seguinte política de permissões:

```json
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "AllowCnhImageUpload",
            "Effect": "Allow",
            "Principal": "*",
            "Action": [
                "s3:PutObject",
                "s3:PutObjectAcl"
            ],
            "Resource": "arn:aws:s3:::seu-bucket/cnh-images/*"
        },
        {
            "Sid": "AllowPublicRead",
            "Effect": "Allow",
            "Principal": "*",
            "Action": "s3:GetObject",
            "Resource": "arn:aws:s3:::seu-bucket-mottu/cnh-images/*"
        }
    ]
}
```
> **⚠️ Nota:** Substitua `seu-bucket` pelo nome real do seu bucket.

### 3. Obter as Chaves de Acesso
1. Acesse a seção "Security credentials" do seu usuário IAM
2. Crie uma nova chave de acesso
3. Copie a **Access Key ID** e a **Secret Access Key**
4. Armazene as chaves de forma segura


## 🚀 Como Executar o Projeto

### 1. Preparação do Ambiente
1. Navegue até a raiz da solução onde está o arquivo `docker-compose.yml`
2. Abra seu terminal favorito neste diretório
```bash
cd /caminho/para/solucao
```

### 2. Configuração do Docker Compose
Edite o arquivo `docker-compose.yml` e configure as seguintes variáveis de ambiente:

```yaml
version: '3.8'

services:
  api:
    environment:
      # Ambiente e Portas
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080
      
      # Conexões
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=mottu_db;Username=postgres;Password=your_password
      - ConnectionStrings__RabbitMQ=amqp://guest:guest@rabbitmq:5672
      
      # AWS
      - AWS__Region=sua-regiao-aws
      - AWS__AccessKey=sua-access-key
      - AWS__SecretKey=sua-secret-key
      - AWS__BucketName=seu-bucket
```
### 3. Inicie os Containers
Execute o comando abaixo para construir e iniciar todos os serviços:

```bash
docker-compose up --build
```
### 4. Acesso à API
Após a inicialização bem-sucedida dos containers, você terá acesso aos seguintes endpoints:

| Serviço | URL |
|---------|-----|
| 🌐 API Base | `http://localhost:8080` |
| 📚 Swagger UI | `http://localhost:8080/swagger` |

### 5. Testando a API
O Swagger oferece uma interface interativa completa para exploração e teste da API:

#### Recursos Disponíveis
- ✨ Visualização completa de todos os endpoints
- 🔄 Teste interativo de operações
- 📋 Exemplos de request/response
- 🔍 Documentação detalhada dos schemas
- ⚡ Códigos de resposta e seus significados

