import { apiClient } from './ApiClient';

// Get tags API
export const getAllTags = async () => {
    try {
        const response = await apiClient.get('/Tag/GetAllTags');
        return response.data;
    } catch (error) {
        console.error("Error fetching tags:", error);
        throw error.response?.data || "Could not load tags";
    }
};