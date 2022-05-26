using System.Text.Json.Serialization;

namespace DaprData;

public record Order([property: JsonPropertyName("orderId")] int OrderId);
