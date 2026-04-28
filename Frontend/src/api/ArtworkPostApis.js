import { apiClient } from './ApiClient';

/**
 * Helper to convert a JS object to FormData
 * Necessary for Create and Update because of the Image file upload
 */
const convertToFormData = (data) => {
    const formData = new FormData();
    Object.keys(data).forEach(key => {
        if (data[key] !== null && data[key] !== undefined) {
            if (key === 'tagIds' && Array.isArray(data[key])) {
                // Backend expects int[] TagIds, append each individually
                data[key].forEach(id => formData.append('TagIds', id));
            } else {
                formData.append(key, data[key]);
            }
        }
    });
    return formData;
};

// Get all artwork posts API
export const getAllArtworkPosts = async () => {
    const response = await apiClient.get('/ArtworkPost/GetAllArtworkPosts');
    return response.data;
};

// Get all artist artwork posts API
export const getAllArtistPosts = async (artistId) => {
    const response = await apiClient.get('/ArtworkPost/GetAllArtistPosts', {
        params: { artistId }
    });
    return response.data;
};

// Get artwork post detailed
export const getPostWithDetails = async (artworkPostId) => {
    const response = await apiClient.get('/ArtworkPost/GetPostWithDetails', {
        params: { artworkPostId }
    });
    return response.data;
};

// Get unapproved artwork posts API
export const getUnapprovedArtworkPosts = async () => {
    const response = await apiClient.get('/ArtworkPost/GetUnapprovedArtworkPosts');
    return response.data;
};

// Create artwork post API
export const createArtworkPost = async (artworkData) => {
    const body = convertToFormData(artworkData);
    // Note: Axios automatically sets 'Content-Type': 'multipart/form-data' for FormData
    const response = await apiClient.post('/ArtworkPost/CreateArtworkPost', body);
    return response.data;
};

// Update artwork post API
export const updateArtworkPost = async (artworkData) => {
    const body = convertToFormData(artworkData);
    const response = await apiClient.put('/ArtworkPost/UpdateArtworkPost', body);
    return response.data;
};

// Delete artwork post API 
export const deleteArtworkPost = async (artworkPostId) => {
    const response = await apiClient.delete('/ArtworkPost/DeleteArtworkPost', {
        params: { artworkPostId }
    });
    return response.data;
};

// Change end date API
export const changeEndDate = async (artworkPostId, endDate) => {
    const response = await apiClient.patch('/ArtworkPost/ChangeEndDate', null, {
        params: { 
            artworkPostId, 
            endDate: endDate.toISOString() // Ensure date is formatted for C#
        }
    });
    return response.data;
};

// Approve artwork post API
export const approveArtworkPost = async (artworkPostId, adminId) => {
    const response = await apiClient.patch('/ArtworkPost/ApproveArtworkPost', null, {
        params: { artworkPostId, adminId }
    });
    return response.data;
};