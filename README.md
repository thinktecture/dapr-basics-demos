# Dapr basics demos
Simple demos of Dapr's features.

## Start applications (self-hosted)
```bash
cd ServiceDaprized
dapr run --enable-api-logging --app-id service-daprized --app-port 6001 --dapr-http-port 3601 --dapr-grpc-port 60001 dotnet run
```

```bash
cd ClientDaprized
dapr run --enable-api-logging --app-id client-daprized --components-path ./../components dotnet run
```

## Dapr Dashboard

```bash
dapr dashboard
```

Navigate to http://localhost:8080

## URLs

* Zipkin
  * [http://localhost:9411](http://localhost:9411)
