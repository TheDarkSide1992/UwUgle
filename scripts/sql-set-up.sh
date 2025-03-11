#!/bin/bash
set -e  # Stop the script on any error

echo "♦️ Setting Up database"

echo "🔮️ Setting Up database"
# PostgreSQL connection details
DB_USER="postgres"
POSTGRES_PASSWORD="postgres"
DB_NAME="UwUgleDB"

# Path to SQL folder
SQL_FOLDER="/SQL/PostgresSQL"

export PGPASSWORD="$DB_PASSWORD"

# Run specific SQL files
echo "🪄️ Setting PostgresSQL tables"
psql -U "$DB_USER" -d "$DB_NAME" -f "$SQL_FOLDER/Create.sql"

echo "🪄️ Setting PostgresSQL Mock Data"
psql -U "$DB_USER" -d "$DB_NAME" -f "$SQL_FOLDER/MockData.sql"

echo "✅ Database setup complete!"
