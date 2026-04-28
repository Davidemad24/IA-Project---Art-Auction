import { useEffect } from 'react'
import { useQueryClient } from '@tanstack/react-query'
import { auctionHubService } from '../realtime/auctionHub'

export function useAuctionRealtime(artworkId) {
  const queryClient = useQueryClient()

  useEffect(() => {
    if (!artworkId) return undefined

    let active = true

    const handleBidPlaced = (payload) => {
      if (payload.artworkId !== artworkId) return

      queryClient.invalidateQueries({ queryKey: ['bids', artworkId] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public', artworkId] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
    }

    auctionHubService.subscribe('BidPlaced', handleBidPlaced)
    auctionHubService.joinAuctionRoom(artworkId)

    return () => {
      if (!active) return
      active = false
      auctionHubService.unsubscribe('BidPlaced', handleBidPlaced)
      auctionHubService.leaveAuctionRoom(artworkId)
    }
  }, [artworkId, queryClient])
}
