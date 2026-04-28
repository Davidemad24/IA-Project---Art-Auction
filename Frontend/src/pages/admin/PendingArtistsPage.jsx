import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { ApprovalTable } from '../../components/admin/ApprovalTable'
import { getPendingArtists, reviewArtist } from '../../services/adminService'

export function PendingArtistsPage() {
  const queryClient = useQueryClient()

  const { data: artists = [], isLoading } = useQuery({
    queryKey: ['admin', 'pending-artists'],
    queryFn: getPendingArtists,
  })

  const mutation = useMutation({
    mutationFn: ({ artistId, decision }) => reviewArtist(artistId, adminId, decision),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin', 'pending-artists'] })
      queryClient.invalidateQueries({ queryKey: ['admin-dashboard'] })
    },
  })

  const rows = artists.map((artist) => ({
    id: artist.id,
    title: artist.name,
    meta: artist.email,
    status: artist.status,
  }))

  return (
    <ApprovalTable
      title="Pending artist accounts"
      rows={rows}
      isLoading={isLoading}
      onApprove={(artistId) => mutation.mutateAsync({ artistId, adminId, decision: 'approve' })}
      onReject={(artistId) => mutation.mutateAsync({ artistId, adminId, decision: 'reject' })}
    />
  )
}
