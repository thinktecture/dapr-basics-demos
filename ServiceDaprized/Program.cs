using Dapr;
using DaprData;
using DaprSubscriber.Controllers;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCodeFirstGrpc();

var app = builder.Build();

app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapPost("/orders",
    //[Topic("pubsub", "orders")]
    (Order order) =>
    {
        Console.WriteLine("Subscriber received: " + order);
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

app.MapGrpcService<WeatherForecastController>();

app.Run();
