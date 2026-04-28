import { apiClient } from './ApiClient';

// Get buyer watch list API
export const getWatchListForBuyer = async (buyerId) => {
    try {
        const response = await apiClient.get('/WatchList/GetWatchListForBuyer', {
            params: { buyerId }
        });
        return response.data; // Returns array of artworks in watchlist
    } catch (error) {
        throw error.response?.data || "Failed to fetch watchlist";
    }
};

// Add to watch list API
export const createWatchList = async (watchData) => {
    try {
        // Uses [FromBody], so we send watchData as a JSON object
        const response = await apiClient.post('/WatchList/CreateWatchList', watchData);
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to add to watchlist";
    }
};

// Delete from watch list API
export const deleteWatchList = async (buyerId, artworkPostId) => {
    try {
        const response = await apiClient.delete('/WatchList/DeleteWatchList', {
            params: { buyerId, artworkPostId }
        });
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to remove from watchlist";
    }
};