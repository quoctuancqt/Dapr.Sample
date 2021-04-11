helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install redis bitnami/redis

kubectl apply -f zipkin.yaml
kubectl apply -f components/config.yaml

kubectl apply -f seq.yaml

kubectl apply -f components/statestore.yaml
kubectl apply -f components/pubsub.yaml