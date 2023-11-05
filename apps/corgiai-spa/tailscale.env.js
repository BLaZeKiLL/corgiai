import ps from 'child_process';
import fs from 'fs';

const buffer = ps.spawnSync('tailscale', ['ip']);

const ip = buffer.stdout.toString('utf8').split('\n')[0];

const env = `
MODE=Tailscale

VITE_PY_CHAT=http://${ip}:8200
`;

fs.writeFileSync('.env.tailscale', env.trim());