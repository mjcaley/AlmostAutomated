/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors')

module.exports = {
  content: [
    "./**/*.{fs,html}"
  ],
    plugins: [
        "@tailwindcss/forms"
    ],
}
