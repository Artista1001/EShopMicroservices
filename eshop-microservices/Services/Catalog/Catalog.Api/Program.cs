using Catalog.Api.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add service to the container
/*
 * Here Carter and MediatR both are added/Intallation in BuildingBlock ClassLib Project and we want to use it in our catalog.api project
 * thats why carter is scaning ICarterModule into BuildingBlock classlib due to intallatoin happend in that project but we define endpoint in catalog.api microservices 
 * thats why carter not able to scan catalog.api microservice and can not find ICarterModule interface
 * But MediatR also in same situation, intalled in BuildingBlock and used in Catalog.api, but here we have config the MediatR services with using RegisterServicesFromAssembly with Program.cs assembly so it worked
 * so is it possible for he carter, -> No, because it does not have config for setting the assembly from outside like in MediatR
 * */
var connectionString = builder.Configuration.GetConnectionString("Database");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'Database' is missing.");
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>)); // we have added validaton behaviour as pipeline behaviour into MediatR
    config.AddOpenBehavior(typeof(LoggingBehaviours<,>));
});
builder.Services.AddCarter();
builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString);
}).UseLightweightSessions(); // There are many option for marten operation config, here we have choose UseLightweightSessions as per our requirement
// Other session behavious can be added here and different types of db operation behaviour can be achieved. ( Read Marten Documentation )

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Registering Global CustomException handling using IExceptionHandler from asp.net core 
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
       .AddNpgSql(connectionString);

var app = builder.Build();

app.MapGet("/", () => "Catalog API is running....");

// Add Http Request Pipeline
app.MapCarter(); // This will look for thie ICarterModule implementation to add all routes of this app. ( map the defined httpMethods )


//// Global Exception handling using inline code better way for large project of microservices is to use IExceptionHandler from asp.ner core libs as services
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if (exception == null) return;
//        var problemDetails = new ProblemDetails
//        {
//            Title = exception.Message,
//            Status = StatusCodes.Status500InternalServerError,
//            Detail = exception.StackTrace 
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";
//        await context.Response.WriteAsJsonAsync(problemDetails);
//    });
//});


// adding pipe line of global exception hadling after registering our CustomExceptionHandler
app.UseExceptionHandler(option => { });

app.UseHealthChecks("/healths",new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
