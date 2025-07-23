var builder = WebApplication.CreateBuilder(args);

// Add service to the container
/*
 * Here Carter and MediatR both are added/Intallation in BuildingBlock ClassLib Project and we want to use it in our catalog.api project
 * thats why carter is scaning ICarterModule into BuildingBlock classlib due to intallatoin happend in that project but we define endpoint in catalog.api microservices 
 * thats why carter not able to scan catalog.api microservice and can not find ICarterModule interface
 * But MediatR also in same situation, intalled in BuildingBlock and used in Catalog.api, but here we have config the MediatR services with using RegisterServicesFromAssembly with Program.cs assembly so it worked
 * so is it possible for he carter, -> No, because it does not have config for setting the assembly from outside like in MediatR
 * */
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions(); // There are many option for marten operation config, here we have choose UseLightweightSessions as per our requirement
// Other session behavious can be added here and different types of db operation behaviour can be achieved. ( Read Marten Documentation )


var app = builder.Build();

// Add Http Request Pipeline
app.MapCarter(); // This will look for thie ICarterModule implementation to add all routes of this app. ( map the defined httpMethods )

app.Run();
