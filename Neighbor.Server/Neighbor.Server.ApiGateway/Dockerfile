FROM nginx:latest
ARG EnvName
COPY ./etc_nginx_confd/default.conf /etc/nginx/conf.d/default.conf
COPY "./etc_nginx_confd/neighbor-$EnvName.conf" "/etc/nginx/conf.d/neighbor-$EnvName.conf"
COPY ./etc_ssl/	/etc/ssl/