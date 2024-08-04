import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

import fs from 'fs'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    https: {
      key: fs.readFileSync(path.resolve(__dirname, 'cert/server.key.pem')),
      cert: fs.readFileSync(path.resolve(__dirname, 'cert/server.cert.pem'))
    }
  }
})
