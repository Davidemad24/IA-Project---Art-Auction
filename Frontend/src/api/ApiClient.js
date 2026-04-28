import axios from 'axios'
import { useAuthStore } from '../store/authStore'
import { refreshToken, logout } from './AuthenticationApis';
import Cookies from 'js-cookie';

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5265',
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 60000,
})

// Request Interceptor (Already exists in your code)
apiClient.interceptors.request.use((config) => {
    // 1. Grab the latest token from your Zustand store
    const token = Cookies.get("jwt_token");

    // 2. If the token exists, "stamp" it onto the request headers
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
}, (error) => {
    return Promise.reject(error);
});

// NEW: Response Interceptor for Silent Refresh
apiClient.interceptors.response.use(
    (response) => response, // Standard 2xx response
    async (error) => {
        const originalRequest = error.config;

        // If error is 401 and we haven't tried refreshing yet
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true; // Mark to prevent infinite loops

            try {
                // Call the function you just showed me
                const userData = await refreshToken();
                
                // Update your Zustand store with new data/token
                useAuthStore.getState().login(userData);
                Cookies.set("jwt_token",userData.token)

                // Update the header and retry the original request
                originalRequest.headers.Authorization = `Bearer ${userData.token}`;
                return apiClient(originalRequest);
            } catch (refreshError) {
                // If refreshing fails, the user must log in again
                logout();
                useAuthStore.getState().logout();
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export { apiClient }
