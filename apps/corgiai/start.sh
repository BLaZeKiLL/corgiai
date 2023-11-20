/app/tailscaled --tun=userspace-networking --socks5-server=localhost:1055 &
/app/tailscale login --hostname=azure-corgiai
/app/tailscale up 

API_PY_CHAT=http://$(/app/tailscale ip -4 codeblazeserver)

echo $API_PY_CHAT

node -r dotenv/config build