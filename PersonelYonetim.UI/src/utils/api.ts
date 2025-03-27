import axios from 'axios';

// Axios instance oluşturma
const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Request interceptor - örneğin token eklemek için
api.interceptors.request.use(
  (config) => {
    // Local storage'dan token al
    const token = localStorage.getItem('token');
    
    // Eğer token varsa, request header'a ekle
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor - örneğin hata yönetimi için
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // 401 Unauthorized hatası durumunda kullanıcıyı login sayfasına yönlendir
    if (error.response && error.response.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    
    return Promise.reject(error);
  }
);

export default api;
