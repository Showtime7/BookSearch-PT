/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./src/**/*.{html,ts}",
    ],
    theme: {
        extend: {
            colors: {
                primary: '#ea580c', // orange-600
                secondary: '#f59e0b', // amber-500
                background: '#fff7ed', // orange-50
            },
            container: {
                center: true,
                padding: '1rem',
            }
        },
    },
    plugins: [],
}
