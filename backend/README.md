# Chatbot API

## Estrutura do Projeto

- **chatbot.Api**
- **chatbot.Application**
- **chatbot.Domain**
- **chatbot.Infrastructure**

## Instruções de configuração

### 1. Configurações

**Ter o Docker instalado em sua máquina**

1. Executar o script `/scripts/setup-docker.sh`
2. Criar a database named `chatbot`
3. Atualizar a connection string no arquivo `chatbot.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=chatbot;Username=your_username;Password=your_password"
  }
}
```

### 2. Applicar o migration


#### Configuração inicial 
```bash
# instalar EF Core globalmente
dotnet tool install --global dotnet-ef

# commando para aplicar os migrations no banco
dotnet ef database update --project chatbot.Infrastructure --startup-project chatbot.Api
```
