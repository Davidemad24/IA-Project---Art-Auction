import { create } from 'zustand'
import { logout } from '../api/AuthenticationApis'

export const useAuthStore = create((set) => ({
  // Initialize state from localStorage
  user: JSON.parse(localStorage.getItem('user')) || null,

  setSession: (userData) => {
    // Save to localStorage so it survives refreshes
    localStorage.setItem('user', JSON.stringify(userData.user));
    set(userData);
  },

  logout: async () => {
    try {
      await logout();
    } catch (error) {
      console.error("Logout failed:", error);
    } finally {
      // Always clean up local data
      localStorage.removeItem('user');
      set({ user: null });
    }
  },
}));