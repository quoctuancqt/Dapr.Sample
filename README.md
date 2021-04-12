# An example microservice using Dapr

## Dapr Overview
Dapr enables developers using any language or framework to easily write microservices. It addresses many of the challenges found that come along with distributed applications, such as:
- How can distributed services discover each other and communicate synchronously?
- How an they implement asynchronous messaging?
- How can they maintain contextual information across a transaction?
- How can they become resilient to failure?
- How can they scale to meet fluctuating demand?
- How are they monitored and observed?

If you are new to Dapr, you may want to review following resources first:
- [Getting started with Dapr](https://docs.dapr.io/getting-started/).
- [Dapr overview](https://docs.dapr.io/concepts/overview/).
- [Dapr quickstarts](https://github.com/dapr/quickstarts) - a collection of simple tutorials covering Dapr's main capabilities.

## Prerequisites
[Docker](https://www.docker.com/)

## Services
This example defintion has the following containerized services:
- orderservice
- orderservice-darp // Order service Dapr Sidecar
- productservice
- productservice-dapr // Product service Dapr Sidecar
- notificationservice
- notificationservice-dapr // Notification service Dapr Sidecar
- placement // Dapr's placement service
- redis
- zipkin 
- seq

## Tracing
Navigate to [Zipkin](http://localhost:9411)

![image](https://user-images.githubusercontent.com/26458668/114176036-da1c5000-9964-11eb-9992-5d9cdf7fceb0.png)

## Logging
Navigate to [Seq](http://localhost:6500/) to see all log.

![image](https://user-images.githubusercontent.com/26458668/114175999-d38dd880-9964-11eb-8081-190d817956c3.png)

## Testing
- Install [RestClient](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) into visual studio code.
- Use `product.test.http` to test `product service`.
- Use `order.test.http` to test `order service`.

## Dapr with Docker-compose

### Overview
This sample demonstrates how to get Dapr running locally with Docker Compose. 

### Prerequisites
[Docker-compose](https://docs.docker.com/compose/install/)

### Networking
Each of these services is deployed to the `darp-sample` Docker network and have their own IP on that network. Each of these services is sharing a network namespace with their associated app service by using network_mode. This means that the app and the sidecars are able to communicate over their localhost interface.

> Ports are still bound on the host machine, therefore, we need to ensure we avoid port conflicts.

### Volumes
In order to get Dapr to load the redis statestore and pubsub components, you need to mount the `./components` directory to the default working directory. These component definitions have been modified to talk to redis using a DNS name redis rather than localhost. This resolves on the Docker network to the IP of the container running redis.

### Deploy the Docker Compose Definition
To deploy the above `docker-compose.yml` you can run:
```
docker-compose up
```

### Clean up
```
docker-compose down
```

### Additional Resources:
[Overview of Docker Compose](https://docs.docker.com/compose/)

## Dapr with Tye
Under development

## Dapr with Kubernetes

### Prerequisites
Please follow this [article](https://andrewlock.net/running-kubernetes-and-the-dashboard-with-docker-desktop/) to enable Kubernetes on Docker Desktop.

### Deploy on Kubernetes
- Make sure you enabled Kubernetes mode on your Docker Desktop.
- Run `deploy-dapr.ps1` to install dapr to your cluster.
- Run `deploy-infrastructure.ps1` to install dapr components and config component.
- Run `deploy-app.ps1` to install `order service`, `product service` and `notification service`.
- You need to expose `order service` and `product service` to external IP address in order to test with `Post man` or `Rest Client`.

```
kubectl port-forward service/orderservice 5001:80
```
```
kubectl port-forward service/productservice 5002:80
```

***Note:***
- *You need to expose zipkin and seq as well if you want to view tracing and logging*.
```
kubectl port-forward service/zipkin 9411:9411
```
```
kubectl port-forward service/seq 6500:80
```

### Undeploy
- Run `undeploy.ps1` to uninstall all component and services.