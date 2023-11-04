/// <reference types="svelte" />
/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_PY_CHAT: string
    // more env variables...
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}