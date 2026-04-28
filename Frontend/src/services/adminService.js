import { db, wait } from '../data/mocks/inMemoryDb'
import { readUserField } from '../utils/secureData'
import * as artworkPostApis from "../api/ArtworkPostApis"
import * as artistApis from "../api/ArtistApis"

async function toArtistView(artist) {
  return {
    ...artist,
    name: await readUserField(artist, 'name'),
    email: await readUserField(artist, 'email'),
  }
}

export async function getAdminDashboardStats() {
  const acceptedArtists = (await artistApis.getApprovedArtists()).length;
  const pendingArtists = (await artistApis.getUnapprovedArtists()).length;
  const liveAuctions = (await artworkPostApis.getAllArtworkPosts()).length;
  const pendingArtworks = (await artworkPostApis.getUnapprovedArtworkPosts()).length;

  return {
    pendingArtists,
    acceptedArtists,
    pendingArtworks,
    liveAuctions,
  }
}

export async function getPendingArtists() {
  const pendingArtists = await artistApis.getUnapprovedArtists();
  return Promise.all(pendingArtists.map((artist) => toArtistView(artist)))
}

export async function getApprovedArtists() {
  const approvedArtists = await artistApis.getApprovedArtists();
  const mappedArtists = await Promise.all(
    approvedArtists.map(async (artist) => {
      const ownedArtworks = await artworkPostApis.getAllArtistPosts(artist.id);
      const hydratedArtist = await toArtistView(artist)

      return {
        ...hydratedArtist,
        artworksCount: ownedArtworks.length,
      }
    }),
  )

  return mappedArtists
}

export async function reviewArtist(artistId, decision) {
  // check decision
    let status = false;
    if (decision === "approve"){
      status = await artistApis.approveArtist(artistId);
    } else {
      status = await artistApis.rejectArtist(artistId);
    }
  
  if (status === false){
    throw new Error('Artist not found.')
  }
}

export async function deleteApprovedArtist(artistId) {
  await wait()

  const artistIndex = db.users.findIndex(
    (user) => user.id === artistId && user.role === 'artist' && user.status === 'approved',
  )

  if (artistIndex < 0) {
    throw new Error('Approved artist not found.')
  }

  const [removedArtist] = db.users.splice(artistIndex, 1)

  const removedArtworkIds = db.artworks
    .filter((artwork) => artwork.artistId === removedArtist.id)
    .map((artwork) => artwork.id)

  db.artworks = db.artworks.filter((artwork) => artwork.artistId !== removedArtist.id)
  db.bids = db.bids.filter((bid) => !removedArtworkIds.includes(bid.artworkId))

  Object.keys(db.watchlists).forEach((userId) => {
    db.watchlists[userId] = db.watchlists[userId].filter(
      (artworkId) => !removedArtworkIds.includes(artworkId),
    )
  })

  db.notifications = db.notifications.filter((notification) => notification.userId !== removedArtist.id)

  delete db.watchlists[removedArtist.id]

  return {
    removedArtist,
    removedArtworksCount: removedArtworkIds.length,
  }
}

export async function getPendingArtworks() {
  return (await artworkPostApis.getUnapprovedArtworkPosts());
}

export async function reviewArtwork(artworkId, decision) {
  await wait()

  const artwork = db.artworks.find((item) => item.id === artworkId)
  if (!artwork) {
    throw new Error('Artwork not found.')
  }

  artwork.moderationStatus = decision === 'approve' ? 'approved' : 'rejected'
  return artwork
}
