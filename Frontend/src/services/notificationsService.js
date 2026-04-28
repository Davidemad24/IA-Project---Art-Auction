import { db, nextId, wait } from '../data/mocks/inMemoryDb'

export async function listNotifications(userId) {
  await wait()

  return db.notifications
    .filter((notification) => notification.userId === userId)
    .sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime())
}

export async function markNotificationAsRead(notificationId) {
  await wait()

  const notification = db.notifications.find((item) => item.id === notificationId)
  if (!notification) {
    throw new Error('Notification not found.')
  }

  notification.isRead = true
  return notification
}

export async function listPurchasedArtworks(userId) {
  await wait()

  return db.purchases
    .filter((purchase) => purchase.userId === userId)
    .sort((a, b) => new Date(b.purchasedAt).getTime() - new Date(a.purchasedAt).getTime())
}

export async function getPaymentNotification(notificationId, userId) {
  await wait()

  const notification = db.notifications.find(
    (item) => item.id === notificationId && item.userId === userId,
  )

  if (!notification) {
    throw new Error('Notification not found.')
  }

  if (notification.type !== 'payment_required') {
    throw new Error('This notification does not require payment.')
  }

  return notification
}

export async function completePayment({
  notificationId,
  userId,
  paymentDetails,
  encryptedPaymentDetails = {},
}) {
  await wait()

  const notification = db.notifications.find(
    (item) => item.id === notificationId && item.userId === userId,
  )

  if (!notification) {
    throw new Error('Notification not found.')
  }

  if (notification.type !== 'payment_required') {
    throw new Error('This notification does not require payment.')
  }

  if (notification.paymentStatus === 'paid') {
    const existingPurchase = db.purchases.find((purchase) => purchase.id === notification.purchaseId)
    return existingPurchase || null
  }

  const artwork = db.artworks.find((item) => item.id === notification.artworkId)

  const purchase = {
    id: nextId('purchase'),
    userId,
    artworkId: notification.artworkId,
    artworkTitle: artwork?.title || 'Artwork',
    artistName: artwork?.artistName || 'Unknown artist',
    imageUrl: artwork?.imageUrl || '',
    amount: notification.amount || artwork?.currentBid || 0,
    purchasedAt: new Date().toISOString(),
    paymentLast4: String(paymentDetails.cardNumber).slice(-4),
    encryptedPaymentDetails,
  }

  db.purchases.push(purchase)

  notification.paymentStatus = 'paid'
  notification.purchaseId = purchase.id
  notification.isRead = true
  notification.message = `Payment completed for "${purchase.artworkTitle}".`

  return purchase
}
