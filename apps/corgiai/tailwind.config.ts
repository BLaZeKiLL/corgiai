import { join } from 'path';
import type { Config } from 'tailwindcss';

// 1. Import the Skeleton plugin
import { skeleton } from '@skeletonlabs/tw-plugin';

const config = {
	// 2. Opt for dark mode to be handled via the class method
	darkMode: 'media',
	content: [
		'./src/**/*.{html,js,svelte,ts}',
    // join(require.resolve('flowbite-svelte'),'../**/*.{html,js,svelte,ts}'),
		// 3. Append the path to the Skeleton package
		join(require.resolve('@skeletonlabs/skeleton'),'../**/*.{html,js,svelte,ts}')
	],
	theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'system-ui', 'Avenir', 'Helvetica', 'Arial', 'sans-serif']
      }
    },
	},
	plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography'),
		// 4. Append the Skeleton plugin (after other plugins)
    skeleton({
      themes: { preset: [ "skeleton" ] }
    })
	]
} satisfies Config;

export default config;