import { useQuery } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import { listPurchasedArtworks } from '../../services/notificationsService'
import { DEFAULT_ARTWORK_IMAGE } from '../../constants/images'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/time'

export function BoughtArtworksPage() {
  const user = useAuthStore((state) => state.user)

  const { data: purchases = [], isLoading } = useQuery({
    queryKey: ['purchases', user?.id],
    queryFn: () => listPurchasedArtworks(user.id),
    enabled: Boolean(user?.id),
    refetchInterval: 5000,
    refetchIntervalInBackground: true,
  })

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading bought artworks...</p>
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Bought artworks</h1>

      {purchases.length === 0 ? (
        <p className="rounded-lg border border-dashed border-stone-300 p-4 text-sm text-stone-600">
          No completed purchases yet.
        </p>
      ) : (
        <div className="space-y-3">
          {purchases.map((purchase) => (
            <article
              key={purchase.id}
              className="flex flex-col gap-3 rounded-xl border border-stone-200 bg-white p-4 sm:flex-row"
            >
              <img
                src={purchase.imageUrl || DEFAULT_ARTWORK_IMAGE}
                alt={purchase.artworkTitle}
                onError={(event) => {
                  event.currentTarget.onerror = null
                  event.currentTarget.src = DEFAULT_ARTWORK_IMAGE
                }}
                className="h-24 w-full rounded-lg object-cover sm:w-24"
              />
              <div className="min-w-0 flex-1">
                <h3 className="font-bold text-stone-900">{purchase.artworkTitle}</h3>
                <p className="text-sm text-stone-600">by {purchase.artistName}</p>
                <p className="mt-1 text-sm font-semibold text-stone-800">
                  Paid: {formatCurrency(purchase.amount)}
                </p>
                <p className="text-xs text-stone-500">{formatDateTime(purchase.purchasedAt)}</p>
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}
