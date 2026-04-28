import { useQuery } from '@tanstack/react-query'
import {
  getPublicArtworkById,
  listOngoingBidFeed,
  listPublicArtworks,
} from '../services/artworksService'

export function usePublicArtworks(filters) {
  return useQuery({
    queryKey: ['artworks', 'public', filters],
    queryFn: () => listPublicArtworks(filters),
  })
}

export function useOngoingBidFeed() {
  return useQuery({
    queryKey: ['bids', 'ongoing-feed'],
    queryFn: listOngoingBidFeed,
    refetchInterval: 5000,
    refetchIntervalInBackground: true,
  })
}

export function usePublicArtworkDetail(artworkId) {
  const artworkPost = getPublicArtworkById(artworkId);
  
  const query = useQuery({
    queryKey: ['artworks', 'public', artworkId],
    queryFn: () => artworkPost,
    enabled: Boolean(artworkId),
  })

  return query;
}
