import dayjs from 'dayjs'

export function getAuctionState(startTime, endTime) {
  const now = dayjs()
  const start = dayjs(startTime)
  const end = dayjs(endTime)

  if (now.isBefore(start)) {
    return 'upcoming'
  }

  if (now.isAfter(end)) {
    return 'ended'
  }

  return 'live'
}

export function formatDateTime(isoValue) {
  return dayjs(isoValue).format('MMM D, YYYY h:mm A')
}
