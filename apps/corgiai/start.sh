/app/tailscale login --qr --hostname=azure-corgiai
/app/tailscale up 

API_PY_CHAT=http://$(/app/tailscale ip -4 codeblazeserver)

echo API_PY_CHAT

node -r dotenv/config build