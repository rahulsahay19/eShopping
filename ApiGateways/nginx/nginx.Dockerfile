FROM nginx

COPY ApiGateways/nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY ApiGateways/nginx/id-local.crt /etc/ssl/certs/id-local.eshopping.com.crt
COPY ApiGateways/nginx/id-local.key /etc/ssl/private/id-local.eshopping.com.key