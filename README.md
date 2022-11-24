# Dapr basics demos
Simple demos of Dapr's features.

## Start applications (self-hosted)
### .NET service / subscriber
```bash
cd ServiceDaprized
dapr run --app-id service-daprized --app-port 6001 --dapr-http-port 3601 --dapr-grpc-port 60001 --components-path ../components --enable-api-logging dotnet run
```

### .NET client / publisher
```bash
cd ClientDaprized
dapr run --app-id client-daprized --config ../configuration/config.yaml --components-path ./../components --enable-api-logging dotnet run
```

### Java subscriber
```bash
dapr run --components-path ../components --app-id service-daprized --app-port 8080 -- java -jar target/java-springboot-service-daprized-0.0.1-SNAPSHOT.jar com.tt.example.javaspringbootservicedaprized.JavaSpringbootServiceDaprizedApplication
```

## Dapr Dashboard

```bash
dapr dashboard
```

Navigate to http://localhost:8080

## URLs

* Zipkin
  * [http://localhost:9411](http://localhost:9411)
