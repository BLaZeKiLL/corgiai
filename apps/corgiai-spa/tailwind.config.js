/** @type {import('tailwindcss').Config} */
export default {
  darkMode: 'media',
  content: [
    "./index.html",
    "./src/**/*.{svelte,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Inter', 'system-ui', 'Avenir', 'Helvetica', 'Arial', 'sans-serif']
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
}

