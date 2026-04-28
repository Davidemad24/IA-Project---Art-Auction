import { useQuery } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import { listArtistArtworks } from '../../services/artistService'

export function ArtistDashboardPage() {
  const user = useAuthStore((state) => state.user)

  const { data: artworks = [] } = useQuery({
    queryKey: ['artist-artworks', user?.id],
    queryFn: () => listArtistArtworks(user.id),
    enabled: Boolean(user?.id),
  })

  console.log(artworks);
  const pendingCount = artworks.filter((artwork) => artwork.adminId === 0).length

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Artist dashboard</h1>

      <div className="grid gap-3 sm:grid-cols-3">
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Total artworks</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{artworks.length}</p>
        </article>
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Pending moderation</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{pendingCount}</p>
        </article>
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Approved</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{artworks.length - pendingCount}</p>
        </article>
      </div>
    </section>
  )
}
