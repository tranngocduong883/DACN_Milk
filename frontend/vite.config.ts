import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  // Backend ASP.NET Core (backend/) chạy ở profile "http" mặc định: http://localhost:5014
  // Đổi bằng biến API_TARGET trong file .env.local hoặc biến môi trường
  const env = loadEnv(mode, '.', '')
  const proxy = {
    '/api': { target: env.API_TARGET || 'http://localhost:5014', changeOrigin: true },
  }

  return {
    plugins: [react()],
    server: { port: 5173, proxy },
    preview: { proxy },
  }
})
