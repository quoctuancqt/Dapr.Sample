kubectl delete  -f zipkin.yaml
kubectl delete  -f components/config.yaml

kubectl delete  -f seq.yaml

kubectl apply -f components/statestore.yaml
kubectl apply -f components/pubsub.yaml

kubectl delete  -f orderservice.yaml
kubectl delete  -f productservice.yaml
kubectl delete  -f notificationservice.yaml

helm uninstall redis