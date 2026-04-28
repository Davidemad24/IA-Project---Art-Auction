import { getAuctionState } from './time'

export const MIN_BID_INCREMENT = 10

export function getMinimumNextBid(currentBid, initialPrice = 0) {
  const floorPrice = Number(currentBid || initialPrice || 0)
  return floorPrice + MIN_BID_INCREMENT
}

export function isValidBidIncrement(bidAmount, currentBid, initialPrice) {
  return Number(bidAmount) >= getMinimumNextBid(currentBid, initialPrice)
}

export function isAuctionActive(startTime, endTime) {
  return getAuctionState(startTime, endTime) === 'live'
}
