echo CORGI APP STARTUP

/app/tailscaled &
/app/tailscale login --hostname=azure-corgiai
/app/tailscale up 

API_PY_CHAT=http://$(/app/tailscale ip -4 codeblazeserver):8200 node build