import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import fs from 'fs';

// https://vite.dev/config/
export default defineConfig((configEnv) => {
  const isDev = configEnv.command === 'serve'
  return {
    plugins: [
      react(),
      tailwindcss()
    ],
    server: isDev? {
        https: {
        key: fs.readFileSync("localhost-key.pem"),
        cert: fs.readFileSync("localhost.pem"),
      }
     }: undefined,
      proxy:{
        "/notification": {
          target: "https://localhost:7101",
          changeOrigin:true,
          secure: false,
          ws: true,
        },
        "/chat": {
          target: "https://localhost:7101",
          changeOrigin:true,
          secure: false,
          ws: true,
        }
      }
    } 
})
