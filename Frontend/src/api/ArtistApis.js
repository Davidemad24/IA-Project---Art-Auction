import { apiClient } from './ApiClient';

// Get unapproved artists API
export const getUnapprovedArtists = async () => {
    try {
        const response = await apiClient.get('/Artist/GetUnapprovedArtists');
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to fetch pending artists";
    }
};

// Get number of approved artists API
export const getApprovedArtists = async () => {
    try {
        const response = await apiClient.get('/Artist/GetApprovedArtists');
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to fetch pending artists";
    }
};

// Approve artist API
export const approveArtist = async (artistId, adminId) => {
    try {
        const response = await apiClient.patch('/Artist/ApproveArtist', null, {
            params: { artistId, adminId }
        });
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to approve artist";
    }
};

// Reject artist API
export const rejectArtist = async (artistId) => {
    try {
        const response = await apiClient.patch('/Artist/RejectArtist', null, {
            params: { artistId }
        });
        return response.data;
    } catch (error) {
        throw error.response?.data || "Failed to reject artist";
    }
};