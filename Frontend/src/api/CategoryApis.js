import { apiClient } from './ApiClient';

// Get categories API
export const getAllCategories = async () => {
    try {
        const response = await apiClient.get('/Category/GetAllCategories');
        return response.data;
    } catch (error) {
        // Log the error for debugging, then throw the message for the UI
        console.error("Error fetching categories:", error);
        throw error.response?.data || "Could not load categories";
    }
};