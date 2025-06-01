-- Spectra Database Initialization Script
-- This script runs when the PostgreSQL container starts for the first time

-- Create extensions if needed
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Set timezone
SET timezone = 'UTC';

-- Create any additional database configurations here
-- The actual tables will be created by Entity Framework migrations
