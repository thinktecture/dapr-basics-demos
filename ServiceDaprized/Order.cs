using System.Text.Json.Serialization;

public record Order([property: JsonPropertyName("orderId")] int OrderId);
