import { Link } from 'react-router-dom'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import { deleteArtwork, listArtistArtworks } from '../../services/artistService'
import { StatusBadge } from '../../components/ui/StatusBadge'
import { APP_ROUTES } from '../../constants/routes'

export function MyArtworksPage() {
  const user = useAuthStore((state) => state.user)
  const queryClient = useQueryClient()

  const { data: artworks = [], isLoading } = useQuery({
    queryKey: ['artist-artworks', user?.id],
    queryFn: () => listArtistArtworks(user.id),
    enabled: Boolean(user?.id),
  })

  const deleteMutation = useMutation({
    mutationFn: (artworkId) => deleteArtwork(artworkId, user.id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['artist-artworks', user.id] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
    },
  })

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading artworks...</p>
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">My artworks</h1>

      <div className="space-y-3">
        {artworks.map((artwork) => (
          <article
            key={artwork.id}
            className="flex flex-col gap-3 rounded-xl border border-stone-200 bg-white p-4 sm:flex-row sm:items-center sm:justify-between"
          >
            <div>
              <p className="font-bold text-stone-900">{artwork.title}</p>
              <p className="text-sm text-stone-600">{artwork.categoryName}</p>
            </div>
            <div className="flex items-center gap-3">
              <StatusBadge status={artwork.adminId != 0 ? "approved" : "pending"} />
              <Link
                to={APP_ROUTES.ARTIST_ARTWORK_EDIT.replace(':artworkId', artwork.id)}
                className="rounded-lg border border-stone-300 px-3 py-1.5 text-xs font-semibold text-stone-700"
              >
                Edit
              </Link>
              <button
                type="button"
                onClick={() => deleteMutation.mutate(artwork.id)}
                className="rounded-lg border border-rose-200 px-3 py-1.5 text-xs font-semibold text-rose-700"
              >
                Delete
              </button>
            </div>
          </article>
        ))}
      </div>
    </section>
  )
}
