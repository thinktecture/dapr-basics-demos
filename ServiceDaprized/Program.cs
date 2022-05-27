using Dapr;
using DaprData;
using Man.Dapr.Sidekick;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDaprSidekick(options =>
{
    options.Sidecar = new DaprSidecarOptions
    {
        AppId = "service-daprized",
        BinDirectory = "../../..",
        CopyProcessFile = true,
        ConfigFile = "./../configuration/config.yaml",
        ComponentsDirectory = "./../components"
    };
});

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapPost("/orders",
    [Topic("pubsub", "orders")] (Order order) =>
    {
        Console.WriteLine("Subscriber received : " + order);
        return Results.Ok(order);
    })
.WithName("OrdersService");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
