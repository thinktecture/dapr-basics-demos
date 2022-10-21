using DaprData;
using ProtoBuf.Grpc.Configuration;

namespace DaprSubscriber.Controllers
{
    [Service]
    public interface IWeatherForecast
    {
        IEnumerable<WeatherForecast> GetForecasts();
    }
}