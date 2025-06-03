var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var database = postgres.AddDatabase("spectra-db");

// Add API service with database reference
var apiService = builder.AddProject<Projects.Spectra_ApiService>("apiservice")
    .WithReference(database);



// Add web frontend with API reference
builder.AddProject<Projects.Spectra_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
