echo CORGI APP STARTUP

/app/tailscaled --tun=userspace-networking --socks5-server=localhost:1055 --outbound-http-proxy-listen=localhost:1055 &
/app/tailscale login --hostname=azure-corgiai
/app/tailscale up 

API_BASE_URL=http://$(/app/tailscale ip -4 codeblazeserver)

API_PY_CHAT=$API_BASE_URL:8200 API_QUIZ_NET=$API_BASE_URL:8300 TAILPROXY=true node -r dotenv/config build