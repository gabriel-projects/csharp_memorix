import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    dedupe: ['react', 'react-dom'],
  },
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5076',
        changeOrigin: true,
      },
    },
  },
  optimizeDeps: {
    include: ['axios', 'react-router-dom'],
  },
})
