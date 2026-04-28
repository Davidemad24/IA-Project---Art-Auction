import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { ApprovalTable } from '../../components/admin/ApprovalTable'
import { getPendingArtworks, reviewArtwork } from '../../services/adminService'

export function PendingArtworksPage() {
  const queryClient = useQueryClient()

  const { data: artworks = [], isLoading } = useQuery({
    queryKey: ['admin', 'pending-artworks'],
    queryFn: getPendingArtworks,
  })

  const mutation = useMutation({
    mutationFn: ({ artworkId, decision }) => reviewArtwork(artworkId, decision),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin', 'pending-artworks'] })
      queryClient.invalidateQueries({ queryKey: ['admin-dashboard'] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
    },
  })

  const rows = artworks.map((artwork) => ({
    id: artwork.id,
    title: artwork.title,
    meta: `${artwork.artistName} - ${artwork.categoryName}`,
    status: "pending",
  }))

  return (
    <ApprovalTable
      title="Pending artwork submissions"
      rows={rows}
      isLoading={isLoading}
      onApprove={(artworkId) => mutation.mutateAsync({ artworkId, decision: 'approve' })}
      onReject={(artworkId) => mutation.mutateAsync({ artworkId, decision: 'reject' })}
    />
  )
}
