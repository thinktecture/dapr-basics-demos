# Dapr basics demos
Simple demos of Dapr's features.

## Start applications (self-hosted)
```bash
cd ServiceDaprized
dapr run --app-id service-daprized --app-port 6001 --dapr-http-port 3601 --dapr-grpc-port 60001 --enable-api-logging dotnet run
```

```bash
cd ClientDaprized
dapr run --app-id client-daprized --config ../configuration/config.yaml --components-path ./../components --enable-api-logging dotnet run
```

## Dapr Dashboard

```bash
dapr dashboard
```

Navigate to http://localhost:8080

## URLs

* Zipkin
  * [http://localhost:9411](http://localhost:9411)
