package com.tt.example.javaspringbootservicedaprized;

import com.fasterxml.jackson.databind.ObjectMapper;
import io.dapr.Topic;
import io.dapr.client.domain.CloudEvent;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import reactor.core.publisher.Mono;

@RestController
public class SubscriberController {
  private static final ObjectMapper OBJECT_MAPPER = new ObjectMapper();

  /**
   * Handles a registered publish endpoint on this app.
   * @param cloudEvent The cloud event received.
   * @return A message containing the time.
   */
  @Topic(name = "orders", pubsubName = "pubsub")
  @PostMapping(path = "/orders")
  public Mono<Void> handleMessage(@RequestBody(required = false) CloudEvent<Order> cloudEvent) {
    return Mono.fromRunnable(() -> {
      try {
        System.out.println("Subscriber got: " + cloudEvent.getData());
        System.out.println("Subscriber got: " + OBJECT_MAPPER.writeValueAsString(cloudEvent));
      } catch (Exception e) {
        throw new RuntimeException(e);
      }
    });
  }
}