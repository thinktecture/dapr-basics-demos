using ConsoleTools;
using Dapr.Client;
using Dapr;
using DaprData;
using System.Net.Http.Json;
using System.Text.Json;

public partial class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Dapr!");

        var menu = new ConsoleMenu(args, level: 0)
          .Add("Web API: Service invocation (Dapr SDK client)", async () => await InvokeServiceWithDaprSdkClient())
          .Add("Web API: Service invocation (Dapr HttpClient)", async () => await InvokeServiceWithDaprHttpClient())
          .Add("Sending emails: Invoke Output binding", async () => await InvokeOutputBinding())
          .Add("Publish new message: Publish-Subscribe ", async () => await PublishEvent())
          .Add("Exit", () => Environment.Exit(0))
          .Configure(config =>
          {
              config.Selector = "--> ";
              config.Title = "Dapr demos";
              config.EnableBreadcrumb = true;
          });

        menu.Show();
    }

    // Feature: service invocation - with Dapr SDK client (respecting Resiliency config, via sidecar)
    private static async Task InvokeServiceWithDaprSdkClient()
    {
        using var client = new DaprClientBuilder().Build();

        var request = client.CreateInvokeMethodRequest(
          HttpMethod.Get,
          "service-daprized",
          "weatherforecast");

        try 
        {
            var response = await client.InvokeMethodAsync<List<WeatherForecast>>(request);

            Console.WriteLine("*** Dapr: SDK invoke result: " + JsonSerializer.Serialize(response, new JsonSerializerOptions{WriteIndented=true}));
        }
        catch(InvocationException dex)
        {
            Console.WriteLine("!!! EXCEPTION: {0}", dex.Message);
        }
    }

    // Feature: service invocation - with Dapr HttpClient (respecting Resiliency config, via sidecar)
    private static async Task InvokeServiceWithDaprHttpClient()
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();

        var weatherForecasts =
            await httpClient.GetFromJsonAsync<List<WeatherForecast>>(
                "http://service-daprized/weatherforecast");

        Console.WriteLine("*** Dapr: HttpClient invoke result: " + JsonSerializer.Serialize(weatherForecasts, new JsonSerializerOptions{WriteIndented=true}));
    }

    // Feature: gRPC service invocation - with Dapr SDK client (respecting Resiliency config, via sidecar)
    /*private static async Task InvokeGrpcServiceWithDaprSdkClient()
    {
        using var client = new DaprClientBuilder().Build();

        try 
        {
            var response = await client.InvokeMethodGrpcAsync<WeatherForecast>("service-daprized", "GetForecasts");

            Console.WriteLine("*** Dapr: SDK invoke result: " + JsonSerializer.Serialize<WeatherForecast>(response));
        }
        catch(InvocationException dex)
        {
            Console.WriteLine("!!! EXCEPTION: {0}", dex.Message);
        }
    }*/

    // Feature: output binding for sending email
    private static async Task InvokeOutputBinding()
    {
        using var client = new DaprClientBuilder().Build();

        var metadata = new Dictionary<string, string> {
          {"emailFrom", "christian.weyer@thinktecture.com"},
          {"emailTo", "christian.weyer@thinktecture.com"},
          {"subject", "An email from Dapr SendGrid binding"}
        };

        var body = "<h1>Hello, you!</h1>Greetings.<br>Bye!";

        await client.InvokeBindingAsync("sendgrid", "create", body, metadata);

        Console.WriteLine("*** Dapr: Email sent!");
    }

    // Feature: publish message with pub-sub
    private static async Task PublishEvent()
    {
        using var client = new DaprClientBuilder().Build();

        var order = new Order(42);

        await client.PublishEventAsync("pubsub", "orders", order);

        Console.WriteLine("Published data: " + order);
        Console.WriteLine("*** Dapr: Order submitted!");
    }
}
