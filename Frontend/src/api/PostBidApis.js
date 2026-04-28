import { apiClient } from './ApiClient';

// Get post bids API
export const getAllPostBids = async (artworkPostId) => {
    try {
        const response = await apiClient.get('/PostBid/GetAllPostBids', {
            params: { artworkPostId }
        });
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to fetch your bids";
    }
};

// Get buyer bids API
export const getBuyerPostBids = async (buyerId) => {
    try {
        const response = await apiClient.get('/PostBid/GetBuyerPostBids', {
            params: { buyerId }
        });
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to fetch your bids";
    }
};

// Create bid API
export const createPostBid = async (bidData) => {
    try {
        const response = await apiClient.post('/PostBid/CreatePostBid', bidData);
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to place bid";
    }
};