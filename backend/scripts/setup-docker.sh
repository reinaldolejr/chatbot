#!/bin/bash

# Script de configuração do Chatbot API com Docker

echo "Configurando o Chatbot API com Docker..."

# Verifica se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "Docker não está rodando. Por favor, inicie o Docker e tente novamente."
    exit 1
fi

# Verifica se o Docker Compose está disponível
if ! command -v docker compose &> /dev/null; then
    echo "Docker Compose não está instalado. Por favor, instale o Docker Compose e tente novamente."
    exit 1
fi

echo "Docker e Docker Compose estão disponíveis"

echo "Parando containers existentes..."
docker compose down

echo "Build e iniciar os serviços..."
docker compose up --build -d

echo "Aguardando o PostgreSQL estar pronto..."
until docker compose exec -T postgres pg_isready -U postgres -d chatbot; do
    echo "Aguardando o PostgreSQL..."
    sleep 10
done

echo "PostgreSQL está pronto!"

echo "Aguardando aAPI estar pronta..."
until curl -f http://localhost:5120/health > /dev/null 2>&1; do
    echo "Aguardando a API..."
    sleep 5
done

echo "API está pronta!"

echo ""
echo "🎉 Setup complete! Services are running:"
echo "   📊 PostgreSQL: localhost:5432"
echo "   🌐 API: http://localhost:5120"
echo ""
echo "📝 Next steps:"
echo "   1. Run Entity Framework migrations:"
echo "      docker compose exec api dotnet ef database update"
echo ""
echo "   2. Test the API using the .http file in your IDE"
echo "      or use curl commands from the README"
echo ""
echo "   3. View logs: docker compose logs -f"
echo "   4. Stop services: docker compose down"
echo "" 