import { filterArtworks, normalizeArtworkFilters } from '../utils/filterParser'
import * as artworkPostapis from "../api/ArtworkPostApis"
import * as postBidApis from "../api/PostBidApis"

export async function listPublicArtworks(filters = {}) {
  // 1. Fetch all posts from the API
  const allArtworks = await artworkPostapis.getAllArtworkPosts();

  // 2. Filter for approved artworks (where adminId is not null)
  const approved = allArtworks.filter((artwork) => artwork.adminId !== null);

  // 3. Apply the rest of your utility filters and return
  return filterArtworks(approved, normalizeArtworkFilters(filters));
}

export async function listOngoingBidFeed() {
  const artworkPosts = await artworkPostapis.getAllArtworkPosts();

  const artworkPostsWithBids = artworkPosts
    .map(async (artwork) => {
      const postBids = await postBidApis.getAllPostBids(artwork.id);

      return {
        ...artwork,
        postBids,
      }
    });

  return Promise.all(artworkPostsWithBids);
}

export async function getArtworkById(artworkId) {

  const artworkPost = artworkPostapis.getPostWithDetails(artworkId);
  return artworkPost.find((artwork) => artwork.id === artworkId) || null
}

export async function getPublicArtworkById(artworkId) {
  const artworkPost = await artworkPostapis.getPostWithDetails(artworkId);

  return artworkPost
}
