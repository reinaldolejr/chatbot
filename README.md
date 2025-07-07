# Chatbot API

## Estrutura do Projeto

- **chatbot.Api**
- **chatbot.Application**
- **chatbot.Domain**
- **chatbot.Infrastructure**

## Instruções de configuração

### 1. Configurações

**Ter o Docker instalado em sua máquina**


1. Abra o terminal na pasta do projeto
   ```sh
   cd /caminho/para/sua/pasta/backend
   ```
2. Executar o script `/scripts/setup-docker.sh`
3. Criar a database named `chatbot`
4. Atualizar a connection string no arquivo `chatbot.Api/appsettings.json`:

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


# Chatbot Frontend com Next/React

1. **Abra o terminal na pasta do projeto**
   ```sh
   cd /caminho/para/sua/pasta/frontend
   ```

2. **Construa a imagem Docker:**
   ```sh
   docker build -t frontend .
   ```

3. **Rode o container:**
   ```sh
   docker run -p 3000:3000 frontend
   ```

4. **Acesse no navegador:**
   Abra [http://localhost:3000](http://localhost:3000)
