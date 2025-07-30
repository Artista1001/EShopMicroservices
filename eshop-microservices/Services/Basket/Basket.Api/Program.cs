using BuildingBlock.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

// Register services class 
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviours<,>)); // we have added validaton behaviour as pipeline behaviour into MediatR
    config.AddOpenBehavior(typeof(LoggingBehaviours<,>));
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShopingCart>().Identity(x => x.UserName);

}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
// builder.Services.AddScoped<IBasketRepository, CachedBasketRepository>(); 
// we have already registered the BasketRepository from IBasketRepository so, directly registering multiple implementation of IBasketRepository is not feasible
// and it makes problem with direct dependency injection 
// so ASP.NET CORE DI - will consider last registration of class implementing IBasketRepository and avoid prior registrations

// to handle this kind of scenario 
// First solution is to decorate CachedBasketRepository Manualy like below code

/*
 * 
builder.Services.AddScoped<IBasketRepository>(provider =>
{
    var basketRepository = provider.GetRequiredService<BasketRepository>();
    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
});

this manually decoration the CachedBasketRepository can be cumbursum and not scalable and managable
 * 
 */

// second solution is Scrutor libs which simplfy the process of implementation , as below one line code
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Registering Redis cache
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the http request pipeline
app.MapGet("/", () => "Basket API is running....");

app.MapCarter();

app.UseExceptionHandler(opts => {  });

app.Run();
