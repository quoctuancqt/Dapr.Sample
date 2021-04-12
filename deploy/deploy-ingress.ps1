kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v0.41.2/deploy/static/provider/cloud/deploy.yaml

kubectl delete -A ValidatingWebhookConfiguration ingress-nginx-admission
kubectl delete -A ValidatingWebhookConfiguration nginx-ingress-ingress-nginx-admission

kubectl apply -f ingress.yaml