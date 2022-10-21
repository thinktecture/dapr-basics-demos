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
          .Add("Service invocation (Dapr SDK client)", async () => await InvokeServiceWithDaprSdkClient())
          .Add("Service invocation (Dapr HttpClient)", async () => await InvokeServiceWithDaprHttpClient())
          .Add("Invoke Output binding", async () => await InvokeOutputBinding())
          .Add("Publish message for pub-sub", async () => await PublishEvent())
          .Add("Exit", () => Environment.Exit(0))
          .Configure(config =>
          {
              config.Selector = "--> ";
              config.Title = "Dapr demos";
              config.EnableBreadcrumb = true;
          });

        menu.Show();
    }

    // Feature: service invocation - with Dapr SDK client
    private static async Task InvokeServiceWithDaprSdkClient()
    {
        using var client = new DaprClientBuilder().Build();

        var request = client.CreateInvokeMethodRequest(
          HttpMethod.Get,
          "service-daprized",
          "weatherforecast");

        try 
        {
            var response = await client.InvokeMethodWithResponseAsync(request);
            //response.EnsureSuccessStatusCode();

            Console.WriteLine("*** Dapr: SDK invoke result: " + response.Content.ReadAsStringAsync().Result);
        }
        catch(InvocationException dex)
        {
            Console.WriteLine("!!! EXCEPTION: {0}", dex.Message);
        }
    }

    // Feature: service invocation - with Dapr HttpClient
    private static async Task InvokeServiceWithDaprHttpClient()
    {
        var httpClient = DaprClient.CreateInvokeHttpClient();

        var weatherForecasts =
            await httpClient.GetFromJsonAsync<List<WeatherForecast>>(
                "http://service-daprized/weatherforecast");

        Console.WriteLine("*** Dapr: HttpClient invoke result: " + JsonSerializer.Serialize(weatherForecasts));
    }

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
