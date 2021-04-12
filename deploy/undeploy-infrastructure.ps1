kubectl delete  -f zipkin.yaml
kubectl delete  -f components/config.yaml

kubectl delete  -f seq.yaml

kubectl delete -f components/statestore.yaml
kubectl delete -f components/pubsub.yaml

helm uninstall redis