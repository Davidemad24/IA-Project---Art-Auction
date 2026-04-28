import { useEffect, useState } from 'react'
import dayjs from 'dayjs'
import { cn } from '../../utils/cn'

function formatRemaining(target) {
  const now = dayjs()
  const diffSeconds = Math.max(target.diff(now, 'second'), 0)
  const hours = Math.floor(diffSeconds / 3600)
  const minutes = Math.floor((diffSeconds % 3600) / 60)
  const seconds = diffSeconds % 60

  if (hours > 0) {
    return `${hours}h ${minutes}m`
  }

  if (minutes > 0) {
    return `${minutes}m ${seconds}s`
  }

  return `${seconds}s`
}

function getLabel(startTime, endTime) {
  const now = dayjs()
  const start = dayjs(startTime)
  const end = dayjs(endTime)

  if (now.isBefore(start)) {
    return `Starts in ${formatRemaining(start)}`
  }

  if (now.isAfter(end)) {
    return 'Auction ended'
  }

  return `Ends in ${formatRemaining(end)}`
}

export function AuctionTimer({ startTime, endTime, className }) {
  const [label, setLabel] = useState(getLabel(startTime, endTime))

  useEffect(() => {
    const interval = setInterval(() => {
      setLabel(getLabel(startTime, endTime))
    }, 1000)

    return () => clearInterval(interval)
  }, [startTime, endTime])

  return <p className={cn('text-sm text-stone-700', className)}>{label}</p>
}
