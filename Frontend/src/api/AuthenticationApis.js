// Imports
import { json } from 'zod';
import {apiClient} from './ApiClient';
import Cookies from 'js-cookie';

// Save data in cookie
const processAuthDto = (data) => {
    // Store tokens in cookies
    // Note: If your backend sets 'Set-Cookie' headers, you don't even need this JS logic!
    // But if you're doing it manually in the frontend:
    Cookies.set('jwt_token', data.token, { expires: new Date(data.expiresOn), secure: true, sameSite: 'strict' });
    Cookies.set('refresh_token', data.refreshToken, { expires: 7, secure: true, sameSite: 'strict' });

    // Return the user data to be stored in your Zustand/Redux store
    return {
        id: data.id,
        name: data.fullName,
        email: data.email,
        role: data.role,
        adminId: data.adminId
    };
};

// Buyer registration API
export const registerBuyer = async (buyerData) => {
    try {
        const response = await apiClient.post('/Authentication/BuyerRegistration', buyerData);
        return processAuthDto(response.data); 
    } catch (error) {
        throw error.response?.data || "Registration failed";
    }
};

// Artist registration API
export const registerArtist = async (artistData) => {
    try {
        const response = await apiClient.post('/Authentication/ArtistRegistration', artistData);
        return processAuthDto(response.data);
    } catch (error) {
        throw error.response?.data || "Registration failed";
    }
};

// Login API
export const login = async (loginData) => {
    try {
        const response = await apiClient.post('/Authentication/Login', loginData);
        return processAuthDto(response.data);
    } catch (error) {
        throw error.response?.data || "Login failed";
    }
};

// Refresh token API
export const refreshToken = async () => {
    try {
        const currentRefreshToken = Cookies.get('refresh_token');
        if (!currentRefreshToken) throw new Error("No refresh token found");

        // Sending raw string as JSON body for your C# [FromBody] string
        const response = await apiClient.post('/Authentication/RefreshToken', JSON.stringify(currentRefreshToken));
        return processAuthDto(response.data);
    } catch (error) {
        throw error.response?.data || "Session expired";
    }
};


// Logout 
export const logout = async () => {
    const currentRefreshToken = Cookies.get('refresh_token');
    if (!currentRefreshToken) throw new Error("No refresh token found");

    // Calling logout API missing
    const response = await apiClient.post("/Authentication/Logout", {refreshToken : currentRefreshToken});

    // Remove cookie data
    if (response.message !== null)
        Cookies.remove('jwt_token');
        Cookies.remove('refresh_token');
};