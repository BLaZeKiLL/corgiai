const CLASSES = [
    'variant-filled-primary',
    'variant-filled-secondary',
    'variant-filled-tertiary',
    'variant-filled-success',
    'variant-filled-warning',
    'variant-filled-error'
];

export const variant_random = () => {
    const rng = [0,1,2,3,4,5];

    let last = -1;

    return () => {
        const next_idx = Math.floor(Math.random() * rng.length)
        const next = rng[next_idx];
        const css = CLASSES[next];

        rng.splice(next_idx, 1);

        if (last != -1) rng.push(last);

        last = next;

        return css;
    }
}

export const variant_hash = () => {
    const hash = (s: string): number => {
        let h = 0, i = 0;
        const l = s.length;
        if ( l > 0 )
            while (i < l)
                h = (h << 5) - h + s.charCodeAt(i++) | 0;
        return h;
    };

    return (name: string) => CLASSES[Math.abs(hash(name)) % CLASSES.length];
}