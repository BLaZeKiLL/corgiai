echo CORGI APP STARTUP

/app/tailscaled --tun=userspace-networking --socks5-server=localhost:1055 --outbound-http-proxy-listen=localhost:1055 &
/app/tailscale login --hostname=azure-corgiai
/app/tailscale up 

ALL_PROXY=socks5://localhost:1055/ HTTP_PROXY=http://localhost:1055/ http_proxy=http://localhost:1055/ API_PY_CHAT=http://$(/app/tailscale ip -4 codeblazeserver):8200 TAILPROXY=true node build