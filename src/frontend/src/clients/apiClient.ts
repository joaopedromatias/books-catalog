import axios from 'axios'

export const apiClient = axios.create({
  baseURL: 'http://localhost:5124/api/Books',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
  },
})
