import { db, nextId, wait } from '../data/mocks/inMemoryDb'
import { readUserField } from '../utils/secureData'
import * as artworkPostApis from "../api/ArtworkPostApis"

export async function listArtistArtworks(artistId) {
  return (await artworkPostApis.getAllArtistPosts(artistId));
}

export async function getArtistArtworkById(artworkId, artistId) {
  await wait()

  return db.artworks.find((artwork) => artwork.id === artworkId && artwork.artistId === artistId) || null
}

export async function createArtwork(artistId, payload) {
  await wait()

  const artist = db.users.find((user) => user.id === artistId)
  if (!artist) {
    throw new Error('Artist was not found.')
  }

  const artistName = await readUserField(artist, 'name')

  const artwork = {
    id: nextId('art'),
    artistId,
    artistName,
    title: payload.title,
    description: payload.description,
    initialPrice: Number(payload.initialPrice),
    auctionStartTime: payload.auctionStartTime,
    auctionEndTime: payload.auctionEndTime,
    category: payload.category,
    tags: payload.tags,
    imageUrl: payload.imageUrl,
    moderationStatus: 'pending',
    currentBid: Number(payload.initialPrice),
  }

  db.artworks.push(artwork)
  return artwork
}

export async function updateArtwork(artworkId, artistId, payload) {
  await wait()

  const artwork = db.artworks.find((item) => item.id === artworkId && item.artistId === artistId)
  if (!artwork) {
    throw new Error('Artwork not found.')
  }

  Object.assign(artwork, {
    ...payload,
    initialPrice: Number(payload.initialPrice),
  })

  artwork.tags = Array.isArray(payload.tags) ? payload.tags : artwork.tags
  artwork.currentBid = Math.max(artwork.currentBid, artwork.initialPrice)

  return artwork
}

export async function deleteArtwork(artworkId, artistId) {
  await wait()

  const idx = db.artworks.findIndex((item) => item.id === artworkId && item.artistId === artistId)
  if (idx < 0) {
    throw new Error('Artwork not found.')
  }

  const [removed] = db.artworks.splice(idx, 1)
  return removed
}

export async function extendAuction(artworkId, artistId, newEndTime) {
  await wait()

  const artwork = db.artworks.find((item) => item.id === artworkId && item.artistId === artistId)
  if (!artwork) {
    throw new Error('Artwork not found.')
  }

  artwork.auctionEndTime = newEndTime
  return artwork
}
