import { apiClient } from './ApiClient';

// Get all post sold API for admin
export const getAllPostSold = async () => {
    try {
        const response = await apiClient.get('/PostSold/GetAllPostSold');
        return response.data; // Returns Array of PostSoldDto
    } catch (error) {
        throw error.response?.data || "Failed to fetch sales records";
    }
};

// Get all buyer unpaid post API
export const getUnpaidPostForBuyer = async (buyerId) => {
    try {
        const response = await apiClient.get('/PostSold/GetUnpaidPostForBuyer', {
            params: { buyerId }
        });
        return response.data; // Returns Array of UnpaidPostSoldDto
    } catch (error) {
        throw error.response?.data || "Failed to fetch unpaid artworks";
    }
};

// Get all posts for buyer 
export const getPostSoldForBuyer = async (buyerId) => {
    try {
        const response = await apiClient.get('/PostSold/GetPostSoldForBuyer', {
            params: { buyerId }
        });
        return response.data; // Returns Array of BuyerPostSoldDto
    } catch (error) {
        throw error.response?.data || "Failed to fetch your purchase history";
    }
};

// Mark unpaid post API
export const markAsPaid = async (paymentData) => {
    try {
        // Uses [FromBody], so we send paymentData as a JSON object
        const response = await apiClient.patch('/PostSold/MarkAsPaid', paymentData);
        return response.data;
    } catch (error) {
        throw error.response?.data || "Payment processing failed";
    }
};