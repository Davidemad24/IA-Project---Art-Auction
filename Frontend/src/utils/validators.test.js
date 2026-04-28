import { describe, expect, it } from 'vitest'
import {
  MIN_BID_INCREMENT,
  getMinimumNextBid,
  isAuctionActive,
  isValidBidIncrement,
} from './validators'

describe('bid validators', () => {
  it('enforces minimum increment', () => {
    expect(MIN_BID_INCREMENT).toBe(10)
    expect(getMinimumNextBid(100, 0)).toBe(110)
    expect(isValidBidIncrement(109, 100, 0)).toBe(false)
    expect(isValidBidIncrement(110, 100, 0)).toBe(true)
  })

  it('detects active auction window', () => {
    const now = Date.now()
    const start = new Date(now - 60_000).toISOString()
    const end = new Date(now + 60_000).toISOString()

    expect(isAuctionActive(start, end)).toBe(true)
  })
})
