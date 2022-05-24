using Dapr.Client;
using DaprData;
using System.Net.Http.Json;

Console.WriteLine("Hello, Dapr!");

using var client = new DaprClientBuilder().Build();

// Feature: service invocation - with Dapr SDK
var request = client.CreateInvokeMethodRequest(
  HttpMethod.Get, 
  "service-daprized", 
  "weatherforecast");

var response = await client.InvokeMethodWithResponseAsync(request);
Console.WriteLine("*** Dapr: SDK invoke result: " + response.Content.ReadAsStringAsync().Result);


// Feature: service invocation - with Dapr HttpClient
var httpClient = DaprClient.CreateInvokeHttpClient();

var weatherForecasts =
    await httpClient.GetFromJsonAsync<List<WeatherForecast>>(
        "http://service-daprized/weatherforecast");
Console.WriteLine("*** Dapr: HttpClient invoke result: " + weatherForecasts);


// Feature: output binding
var metadata = new Dictionary<string, string> {
    {"emailFrom", "christian.weyer@thinktecture.com"},
    {"emailTo", "christian.weyer@thinktecture.com"},
    {"subject", "An email from Dapr SendGrid binding"}
  };

var body =  "<h1>Hello, Techorama!</h1>Greetings.<br>Bye!";

await client.InvokeBindingAsync("sendgrid", "create", body, metadata);
Console.WriteLine("*** Dapr: Email sent!");


Console.ReadLine();
