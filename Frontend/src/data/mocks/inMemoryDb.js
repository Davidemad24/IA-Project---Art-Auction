import { ARTWORK_IMAGE_LIBRARY } from '../../constants/images'
import { auctionHubService } from '../../realtime/auctionHub'

const now = Date.now()
const hour = 60 * 60 * 1000

function offset(hours) {
  return new Date(now + hours * hour).toISOString()
}

const seedState = {
  users: [
    {
      id: 'u-admin-1',
      name: 'System Admin',
      email: 'admin@artauction.com',
      password: 'Admin@123',
      role: 'admin',
      status: 'approved',
    },
    {
      id: 'u-artist-1',
      name: 'Mariam Aziz',
      email: 'mariam@artists.com',
      password: 'Artist@123',
      role: 'artist',
      status: 'pending',
    },
    {
      id: 'u-artist-2',
      name: 'Youssef Nader',
      email: 'youssef@artists.com',
      password: 'Artist@123',
      role: 'artist',
      status: 'approved',
    },
    {
      id: 'u-artist-3',
      name: 'Salma Fouad',
      email: 'salma@artists.com',
      password: 'Artist@123',
      role: 'artist',
      status: 'approved',
    },
    {
      id: 'u-artist-4',
      name: 'Omar Khaled',
      email: 'omar@artists.com',
      password: 'Artist@123',
      role: 'artist',
      status: 'approved',
    },
    {
      id: 'u-buyer-1',
      name: 'Hana Ibrahim',
      email: 'hana@buyers.com',
      password: 'Buyer@123',
      role: 'buyer',
      status: 'approved',
    },
    {
      id: 'u-buyer-2',
      name: 'Karim Adel',
      email: 'karim@buyers.com',
      password: 'Buyer@123',
      role: 'buyer',
      status: 'approved',
    },
  ],
  artworks: [
    {
      id: 'art-1',
      artistId: 'u-artist-2',
      artistName: 'Youssef Nader',
      title: 'Dawn on Cairo',
      description: 'Mixed media cityscape inspired by early-morning market movement.',
      initialPrice: 300,
      auctionStartTime: offset(-6),
      auctionEndTime: offset(10),
      category: 'Landscape',
      tags: ['city', 'warm', 'market'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.dawnOnCairo,
      moderationStatus: 'approved',
      currentBid: 420,
    },
    {
      id: 'art-2',
      artistId: 'u-artist-2',
      artistName: 'Youssef Nader',
      title: 'Copper Geometry',
      description: 'Abstract geometric study in burnt orange and deep navy.',
      initialPrice: 450,
      auctionStartTime: offset(-4),
      auctionEndTime: offset(6),
      category: 'Abstract',
      tags: ['geometric', 'modern'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.copperGeometry,
      moderationStatus: 'approved',
      currentBid: 560,
    },
    {
      id: 'art-3',
      artistId: 'u-artist-1',
      artistName: 'Mariam Aziz',
      title: 'Stillness in Indigo',
      description: 'A large-format portrait exploring silence through layered pigments.',
      initialPrice: 500,
      auctionStartTime: offset(6),
      auctionEndTime: offset(20),
      category: 'Portrait',
      tags: ['portrait', 'indigo'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.stillnessInIndigo,
      moderationStatus: 'pending',
      currentBid: 500,
    },
    {
      id: 'art-4',
      artistId: 'u-artist-3',
      artistName: 'Salma Fouad',
      title: 'Golden Alley Echo',
      description: 'Oil on canvas capturing textured evening light through historic alleyways.',
      initialPrice: 380,
      auctionStartTime: offset(-3),
      auctionEndTime: offset(8),
      category: 'Landscape',
      tags: ['gold', 'texture', 'alley'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.goldenAlleyEcho,
      moderationStatus: 'approved',
      currentBid: 490,
    },
    {
      id: 'art-5',
      artistId: 'u-artist-4',
      artistName: 'Omar Khaled',
      title: 'Silent Geometry No. 9',
      description: 'Bold geometric composition balancing contrast, rhythm, and negative space.',
      initialPrice: 620,
      auctionStartTime: offset(-2),
      auctionEndTime: offset(12),
      category: 'Abstract',
      tags: ['geometry', 'minimal', 'contrast'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.silentGeometryNo9,
      moderationStatus: 'approved',
      currentBid: 760,
    },
    {
      id: 'art-6',
      artistId: 'u-artist-3',
      artistName: 'Salma Fouad',
      title: 'Nile Moon Passage',
      description:
        'An atmospheric night-scene composition with reflective river tones and layered brushwork.',
      initialPrice: 320,
      auctionStartTime: offset(-0.5),
      auctionEndTime: offset(5 / 60),
      category: 'Landscape',
      tags: ['night', 'river', 'moonlight'],
      imageUrl: ARTWORK_IMAGE_LIBRARY.nileMoonPassage,
      moderationStatus: 'approved',
      currentBid: 410,
    },
  ],
  bids: [
    {
      id: 'bid-1',
      artworkId: 'art-1',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 350,
      timestamp: offset(-3),
    },
    {
      id: 'bid-2',
      artworkId: 'art-1',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 420,
      timestamp: offset(-1),
    },
    {
      id: 'bid-3',
      artworkId: 'art-2',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 560,
      timestamp: offset(-2),
    },
    {
      id: 'bid-4',
      artworkId: 'art-4',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 430,
      timestamp: offset(-2.5),
    },
    {
      id: 'bid-5',
      artworkId: 'art-4',
      bidderId: 'u-buyer-2',
      bidderName: 'Karim Adel',
      price: 490,
      timestamp: offset(-1.1),
    },
    {
      id: 'bid-6',
      artworkId: 'art-5',
      bidderId: 'u-buyer-2',
      bidderName: 'Karim Adel',
      price: 700,
      timestamp: offset(-1.8),
    },
    {
      id: 'bid-7',
      artworkId: 'art-5',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 760,
      timestamp: offset(-0.6),
    },
    {
      id: 'bid-8',
      artworkId: 'art-6',
      bidderId: 'u-buyer-1',
      bidderName: 'Hana Ibrahim',
      price: 390,
      timestamp: offset(-0.35),
    },
    {
      id: 'bid-9',
      artworkId: 'art-6',
      bidderId: 'u-buyer-2',
      bidderName: 'Karim Adel',
      price: 410,
      timestamp: offset(-0.2),
    },
  ],
  watchlists: {
    'u-buyer-1': ['art-1', 'art-2', 'art-4', 'art-6'],
    'u-buyer-2': ['art-5', 'art-6'],
  },
  notifications: [
    {
      id: 'note-1',
      userId: 'u-buyer-1',
      title: 'Auction reminder',
      message: 'Dawn on Cairo ends in less than 12 hours.',
      timestamp: offset(-1),
      isRead: false,
      type: 'info',
    },
    {
      id: 'note-2',
      userId: 'u-buyer-2',
      title: 'Outbid alert',
      message: 'A higher bid was placed on Golden Alley Echo.',
      timestamp: offset(-0.4),
      isRead: false,
      type: 'info',
    },
  ],
  purchases: [],
}

function clone(data) {
  return JSON.parse(JSON.stringify(data))
}

export let db = clone(seedState)
let idCounter = 100
let settledAuctionIds = new Set()

function exposeDbForDebugging() {
  if (typeof window !== 'undefined') {
    window.__ART_AUCTION_DB__ = db
  }
}

function emitWinnerNotificationRealtime(notificationPayload) {
  auctionHubService.emitLocal('WinnerNotified', notificationPayload)
}

exposeDbForDebugging()

export function nextId(prefix) {
  idCounter += 1
  return `${prefix}-${idCounter}`
}

function processEndedAuctions() {
  const nowMs = Date.now()

  db.artworks.forEach((artwork) => {
    if (artwork.moderationStatus !== 'approved') return
    if (new Date(artwork.auctionEndTime).getTime() > nowMs) return
    if (settledAuctionIds.has(artwork.id)) return

    const artworkBids = db.bids
      .filter((bid) => bid.artworkId === artwork.id)
      .sort((a, b) => {
        if (b.price !== a.price) {
          return b.price - a.price
        }

        return new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()
      })

    const highestBid = artworkBids[0]

    if (highestBid) {
      const winnerNotification = {
        id: nextId('note'),
        userId: highestBid.bidderId,
        title: 'Auction won',
        message: `You won "${artwork.title}" with a bid of $${highestBid.price}.`,
        timestamp: new Date().toISOString(),
        isRead: false,
        type: 'payment_required',
        artworkId: artwork.id,
        amount: highestBid.price,
        paymentStatus: 'pending',
      }

      db.notifications.push(winnerNotification)
      emitWinnerNotificationRealtime(winnerNotification)
    }

    settledAuctionIds.add(artwork.id)
  })
}

export function wait(ms = 180) {
  return new Promise((resolve) => {
    setTimeout(() => {
      processEndedAuctions()
      resolve()
    }, ms)
  })
}

export function resetMockDb() {
  db = clone(seedState)
  idCounter = 100
  settledAuctionIds = new Set()
  exposeDbForDebugging()
}
