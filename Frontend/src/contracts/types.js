/**
 * @typedef {Object} User
 * @property {string} id
 * @property {string} name
 * @property {string} email
 * @property {'admin'|'artist'|'buyer'} role
 * @property {'approved'|'pending'|'rejected'} status
 */

/**
 * @typedef {Object} AuctionWindow
 * @property {string} startTime
 * @property {string} endTime
 */

/**
 * @typedef {Object} Artwork
 * @property {string} id
 * @property {string} artistId
 * @property {string} artistName
 * @property {string} title
 * @property {string} description
 * @property {number} initialPrice
 * @property {string} auctionStartTime
 * @property {string} auctionEndTime
 * @property {string} category
 * @property {string[]} tags
 * @property {string} imageUrl
 * @property {'pending'|'approved'|'rejected'} moderationStatus
 * @property {number} currentBid
 */

/**
 * @typedef {Object} Bid
 * @property {string} id
 * @property {string} artworkId
 * @property {string} bidderId
 * @property {string} bidderName
 * @property {number} price
 * @property {string} timestamp
 */

/**
 * @typedef {Object} WatchlistItem
 * @property {string} userId
 * @property {string} artworkId
 */

/**
 * @typedef {Object} Notification
 * @property {string} id
 * @property {string} userId
 * @property {string} title
 * @property {string} message
 * @property {string} timestamp
 * @property {boolean} isRead
 */

export const CONTRACTS = {}
