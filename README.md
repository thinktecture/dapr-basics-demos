# Dapr basics demos
Simple demos of Dapr's features.

## Start applications (self-hosted)
```bash
    cd ServiceDaprized
	dapr run --app-id service-daprized --app-port 6001 --dapr-http-port 3601 --dapr-grpc-port 60001 dotnet run
```

```bash
    cd ClientDaprized
	dapr run --app-id client-daprized --components-path ./../components dotnet run
```
