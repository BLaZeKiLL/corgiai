import { env } from '$env/dynamic/private';

function configure_proxy() {
    return env.TAILPROXY ? {
        host: 'localhost',
        port: 1055
    } : undefined;
}

const _proxy = configure_proxy();

export const proxy = _proxy;