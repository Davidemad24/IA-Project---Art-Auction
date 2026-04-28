import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { ConfirmationModal } from '../../components/ui/ConfirmationModal'
import { deleteApprovedArtist, getApprovedArtists } from '../../services/adminService'

export function AcceptedArtistsPage() {
  const queryClient = useQueryClient()
  const [selectedArtist, setSelectedArtist] = useState(null)

  const { data: artists = [], isLoading } = useQuery({
    queryKey: ['admin', 'approved-artists'],
    queryFn: getApprovedArtists,
  })

  const mutation = useMutation({
    mutationFn: (artistId) => deleteApprovedArtist(artistId),
    onSuccess: () => {
      setSelectedArtist(null)
      queryClient.invalidateQueries({ queryKey: ['admin', 'approved-artists'] })
      queryClient.invalidateQueries({ queryKey: ['admin-dashboard'] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
      queryClient.invalidateQueries({ queryKey: ['bids'] })
      queryClient.invalidateQueries({ queryKey: ['watchlist'] })
    },
  })

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Accepted artists</h1>
      <p className="text-sm text-stone-600">
        Deleting an accepted artist removes their account and all artworks they posted.
      </p>

      {isLoading ? <p className="text-sm text-stone-600">Loading accepted artists...</p> : null}

      {!isLoading && artists.length === 0 ? (
        <p className="rounded-lg border border-dashed border-stone-300 p-4 text-sm text-stone-600">
          No accepted artists found.
        </p>
      ) : null}

      {artists.length > 0 ? (
        <div className="overflow-x-auto rounded-lg border border-[var(--line)]">
          <table className="min-w-full bg-white text-left text-sm">
            <thead className="bg-stone-100 text-xs uppercase tracking-wide text-stone-700">
              <tr>
                <th className="px-4 py-3">Artist</th>
                <th className="px-4 py-3">Email</th>
                <th className="px-4 py-3">Artworks</th>
                <th className="px-4 py-3">Action</th>
              </tr>
            </thead>
            <tbody>
              {artists.map((artist) => (
                <tr key={artist.id} className="border-t border-stone-200">
                  <td className="px-4 py-3 font-semibold text-stone-800">{artist.name}</td>
                  <td className="px-4 py-3 text-stone-700">{artist.email}</td>
                  <td className="px-4 py-3 text-stone-700">{artist.artworksCount}</td>
                  <td className="px-4 py-3">
                    <button
                      type="button"
                      onClick={() => setSelectedArtist(artist)}
                      className="rounded-md bg-rose-600 px-3 py-1.5 text-xs font-semibold text-white"
                    >
                      Delete Artist
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : null}

      <ConfirmationModal
        open={Boolean(selectedArtist)}
        title="Delete accepted artist"
        description={
          selectedArtist
            ? `Delete ${selectedArtist.name}? This will also remove all of their artworks.`
            : ''
        }
        confirmText={mutation.isPending ? 'Deleting...' : 'Delete'}
        cancelText="Cancel"
        onConfirm={() => {
          if (!selectedArtist) return
          mutation.mutate(selectedArtist.id)
        }}
        onCancel={() => {
          if (mutation.isPending) return
          setSelectedArtist(null)
        }}
      />
    </section>
  )
}
