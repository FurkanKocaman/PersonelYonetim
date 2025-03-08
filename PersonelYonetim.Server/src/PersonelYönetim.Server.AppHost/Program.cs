var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PersonelYönetim_Server_WebAPI>("cleanarchitecture-2025-webapi");

builder.Build().Run();
