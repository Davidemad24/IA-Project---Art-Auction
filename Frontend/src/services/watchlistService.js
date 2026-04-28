import { db, wait } from '../data/mocks/inMemoryDb'

export async function listWatchlist(userId) {
  await wait()

  const entries = db.watchlists[userId] || []

  return db.artworks.filter(
    (artwork) => entries.includes(artwork.id) && artwork.moderationStatus === 'approved',
  )
}

export async function addToWatchlist(userId, artworkId) {
  await wait()

  if (!db.watchlists[userId]) {
    db.watchlists[userId] = []
  }

  const exists = db.watchlists[userId].includes(artworkId)
  if (!exists) {
    db.watchlists[userId].push(artworkId)
  }

  return db.watchlists[userId]
}

export async function removeFromWatchlist(userId, artworkId) {
  await wait()

  if (!db.watchlists[userId]) {
    db.watchlists[userId] = []
  }

  db.watchlists[userId] = db.watchlists[userId].filter((id) => id !== artworkId)
  return db.watchlists[userId]
}

export async function toggleWatchlist(userId, artworkId) {
  await wait()

  if (!db.watchlists[userId]) {
    db.watchlists[userId] = []
  }

  const exists = db.watchlists[userId].includes(artworkId)

  if (exists) {
    db.watchlists[userId] = db.watchlists[userId].filter((id) => id !== artworkId)
  } else {
    db.watchlists[userId].push(artworkId)
  }

  return db.watchlists[userId]
}
