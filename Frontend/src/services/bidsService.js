import { db, nextId, wait } from '../data/mocks/inMemoryDb'
import { auctionHubService } from '../realtime/auctionHub'
import { getMinimumNextBid, isAuctionActive, isValidBidIncrement } from '../utils/validators'

export async function placeBid({ artworkId, bidder, amount }) {
  await wait()

  if (!bidder || bidder.role !== 'buyer') {
    throw new Error('Only logged in buyers can place bids.')
  }

  const artwork = db.artworks.find((entry) => entry.id === artworkId)

  if (!artwork || artwork.moderationStatus !== 'approved') {
    throw new Error('Artwork is not available for bidding.')
  }

  if (!isAuctionActive(artwork.auctionStartTime, artwork.auctionEndTime)) {
    throw new Error('Auction is not active.')
  }

  if (!isValidBidIncrement(amount, artwork.currentBid, artwork.initialPrice)) {
    const min = getMinimumNextBid(artwork.currentBid, artwork.initialPrice)
    throw new Error(`Bid must be at least $${min}.`)
  }

  const bid = {
    id: nextId('bid'),
    artworkId,
    bidderId: bidder.id,
    bidderName: bidder.name,
    price: Number(amount),
    timestamp: new Date().toISOString(),
  }

  db.bids.push(bid)
  artwork.currentBid = Number(amount)

  auctionHubService.emitLocal('BidPlaced', {
    artworkId,
    bid,
  })

  return bid
}

export async function listBuyerBids(buyerId) {
  await wait()

  return db.bids
    .filter((bid) => bid.bidderId === buyerId)
    .map((bid) => {
      const artwork = db.artworks.find((item) => item.id === bid.artworkId)

      return {
        ...bid,
        artworkTitle: artwork?.title || bid.artworkId,
      }
    })
    .sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime())
}
