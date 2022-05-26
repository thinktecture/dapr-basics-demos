using ConsoleTools;
using Dapr.Client;
using DaprData;
using System.Net.Http.Json;
using System.Text.Json;

public partial class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Dapr!");

        var menu = new ConsoleMenu(args, level: 0)
          .Add("Service invocation SDK client", async () => await InvokeServiceWithDaprSdkClient())
          .Add("Service invocation Http client", async () => await InvokeServiceWithDaprHttpClient())
          .Add("Output binding", async () => await InvokeOutputBinding())
          .Add("Publish / subscribe", async () => await PublishEvent())
          .Add("Exit", () => Environment.Exit(0))
          .Configure(config =>
          {
              config.Selector = "--> ";
              config.Title = "Dapr demos";
              config.EnableBreadcrumb = true;
          });

        menu.Show();
    }

    // Feature: service invocation - with Dapr SDK
    private static async Task InvokeServiceWithDaprSdkClient()
    {
        using var client = new DaprClientBuilder().Build();

        var request = client.CreateInvokeMethodRequest(
          HttpMethod.Get,
          "service-daprized",
          "weatherforecast");

        var response = await client.InvokeMethodWithResponseAsync(request);

        Console.WriteLine("*** Dapr: SDK invoke result: " + response.Content.ReadAsStringAsync().Result);
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

    // Feature: output binding
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

    // Feature: pub-sub
    private static async Task PublishEvent()
    {
        using var client = new DaprClientBuilder().Build();

        var order = new Order(42);

        await client.PublishEventAsync("pubsub", "orders", order);

        Console.WriteLine("Published data: " + order);
        Console.WriteLine("*** Dapr: Order submitted!");
    }
}
