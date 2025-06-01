# PostgreSQL Setup Guide for Spectra

## 🐘 PostgreSQL Installation and Configuration

### Option 1: Using .NET Aspire (Recommended for Development)

The easiest way to run PostgreSQL locally is using .NET Aspire's built-in container orchestration:

```bash
# Simply run the Aspire AppHost - PostgreSQL will be automatically started
dotnet run --project Spectra.AppHost
```

This will:
- Automatically start a PostgreSQL container
- Configure the database connection
- Apply Entity Framework migrations
- Provide monitoring through the Aspire dashboard

### Option 2: Using Homebrew (macOS)

```bash
# Install PostgreSQL
brew install postgresql@16

# Start PostgreSQL service
brew services start postgresql@16

# Create database and user
createdb spectra_db
psql spectra_db

# In psql, create user (if needed)
CREATE USER postgres WITH PASSWORD 'postgres';
GRANT ALL PRIVILEGES ON DATABASE spectra_db TO postgres;
```

### Option 3: Using PostgreSQL.app (macOS)

1. Download PostgreSQL.app from https://postgresapp.com/
2. Install and start the application
3. Create a new database named `spectra_db`

### Option 4: Native Installation (Windows/Linux)

#### Windows:
1. Download from https://www.postgresql.org/download/windows/
2. Run the installer and follow the setup wizard
3. Remember the password you set for the `postgres` user

#### Ubuntu/Debian:
```bash
sudo apt update
sudo apt install postgresql postgresql-contrib
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Create database
sudo -u postgres createdb spectra_db
```

## 🔧 Configuration

### Connection String Format

The application uses the following connection string format:

```
Host=localhost;Database=spectra_db;Username=postgres;Password=postgres;Port=5432
```

### Environment-Specific Configuration

#### Development (appsettings.json):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=spectra_db;Username=postgres;Password=postgres;Port=5432"
  }
}
```

#### Production (Azure PostgreSQL):
```json
{
  "ConnectionStrings": {
    "ProductionConnection": "Host=your-server.postgres.database.azure.com;Database=spectra_db;Username=your-username;Password=your-password;Port=5432;SSL Mode=Require"
  }
}
```

## 🗄️ Database Migrations

### Creating Initial Migration

```bash
# Navigate to the Infrastructure project
cd Spectra.Infrastructure

# Add initial migration
dotnet ef migrations add InitialCreate --startup-project ../Spectra.ApiService

# Apply migration to database
dotnet ef database update --startup-project ../Spectra.ApiService
```

### Managing Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName --startup-project ../Spectra.ApiService

# Update database
dotnet ef database update --startup-project ../Spectra.ApiService

# Remove last migration (if not applied)
dotnet ef migrations remove --startup-project ../Spectra.ApiService

# Generate SQL script
dotnet ef migrations script --startup-project ../Spectra.ApiService
```

## 🔍 Verification

### Test Database Connection

You can test the connection using any PostgreSQL client:

```bash
# Using psql (when using local PostgreSQL installation)
psql -h localhost -p 5432 -U postgres -d spectra_db

# When using .NET Aspire, check the Aspire dashboard for connection details
# The dashboard will show the exact connection string and port
```

### Verify Tables

After running migrations, you should see these tables:

- `asp_net_users` (Identity users)
- `asp_net_roles` (Identity roles)
- `asp_net_user_roles` (User-role relationships)
- `images` (User uploaded images)
- `follows` (Follow relationships)
- `likes` (Image likes)
- `comments` (Image comments)

## 🚀 Azure PostgreSQL Setup

### Creating Azure Database for PostgreSQL

```bash
# Using Azure CLI
az postgres flexible-server create \
  --resource-group your-resource-group \
  --name spectra-postgres \
  --location eastus \
  --admin-user spectradmin \
  --admin-password YourSecurePassword123! \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --storage-size 32

# Configure firewall (allow Azure services)
az postgres flexible-server firewall-rule create \
  --resource-group your-resource-group \
  --name spectra-postgres \
  --rule-name AllowAzureServices \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### Azure Connection String

```
Host=spectra-postgres.postgres.database.azure.com;Database=spectra_db;Username=spectradmin;Password=YourSecurePassword123!;Port=5432;SSL Mode=Require
```

## 🛠️ Troubleshooting

### Common Issues

1. **Connection refused**: Ensure PostgreSQL is running
2. **Authentication failed**: Check username/password
3. **Database does not exist**: Create the database first
4. **Port conflicts**: Change port if 5432 is in use

### Useful Commands

```bash
# Check PostgreSQL status
brew services list | grep postgresql  # macOS
sudo systemctl status postgresql      # Linux

# Stop/Start PostgreSQL
brew services stop postgresql@16      # macOS
sudo systemctl stop postgresql        # Linux

# View PostgreSQL logs
tail -f /usr/local/var/log/postgresql@16.log  # macOS
sudo journalctl -u postgresql                 # Linux
```

## 📊 Performance Optimization

### Recommended PostgreSQL Settings

For development, the default settings are fine. For production, consider:

```sql
-- In postgresql.conf
shared_buffers = 256MB
effective_cache_size = 1GB
maintenance_work_mem = 64MB
checkpoint_completion_target = 0.9
wal_buffers = 16MB
default_statistics_target = 100
random_page_cost = 1.1
effective_io_concurrency = 200
```

### Indexing Strategy

The application automatically creates indexes for:
- User foreign keys
- Created timestamps
- Unique constraints (follows, likes)

## 🔐 Security Best Practices

1. **Use strong passwords** for database users
2. **Enable SSL** in production
3. **Restrict network access** using firewall rules
4. **Regular backups** using pg_dump or Azure backup
5. **Monitor connections** and query performance

---

**Next Steps**: After setting up PostgreSQL, run the application and test the authentication endpoints to ensure everything works correctly.
