# Chatbot API

## Project Structure

- **chatbot.Api**
- **chatbot.Application**
- **chatbot.Domain**
- **chatbot.Infrastructure**

## Setup Instructions

### 1. Configuration

**Make sure Docker is installed on your machine**

1. Open the terminal in the project folder
   ```sh
   cd /path/to/your/project/backend
   ```
2. Run the script `/scripts/setup-docker.sh`
3. Create a database named `chatbot`
4. Update the connection string in the file `chatbot.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=chatbot;Username=your_username;Password=your_password"
  }
}
```

### 2. Apply the migration

#### Initial setup
```bash
# install EF Core globally
dotnet tool install --global dotnet-ef

# command to apply migrations to the database
dotnet ef database update --project chatbot.Infrastructure --startup-project chatbot.Api
```

# Chatbot Frontend with Next/React

1. **Open the terminal in the project folder**
   ```sh
   cd /path/to/your/project/frontend
   ```

2. **Build the Docker image:**
   ```sh
   docker build -t frontend .
   ```

3. **Run the container:**
   ```sh
   docker run -p 3000:3000 frontend
   ```

4. **Access in your browser:**
   Open [http://localhost:3000](http://localhost:3000)
