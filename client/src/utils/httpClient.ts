import axios from 'axios'

const httpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URI,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json'
  }
})

const privateHttpClient = axios.create({
  baseURL: import.meta.env.VITE_API_URI,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json'
  }
})

export { httpClient, privateHttpClient }
