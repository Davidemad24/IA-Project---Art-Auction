import { useQuery } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import { listBuyerBids } from '../../services/bidsService'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/time'

export function MyBidsPage() {
  const user = useAuthStore((state) => state.user)

  const { data: bids = [], isLoading } = useQuery({
    queryKey: ['buyer-bids', user?.id],
    queryFn: () => listBuyerBids(user.id),
    enabled: Boolean(user?.id),
  })

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading your bids...</p>
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">My bids</h1>

      {bids.length === 0 ? (
        <p className="rounded-lg border border-dashed border-stone-300 p-4 text-sm text-stone-600">
          You have not placed bids yet.
        </p>
      ) : (
        <div className="overflow-x-auto rounded-lg border border-[var(--line)]">
          <table className="min-w-full bg-white text-left text-sm">
            <thead className="bg-stone-100 text-xs uppercase tracking-wide text-stone-700">
              <tr>
                <th className="px-4 py-3">Artwork</th>
                <th className="px-4 py-3">Price</th>
                <th className="px-4 py-3">Time</th>
              </tr>
            </thead>
            <tbody>
              {bids.map((bid) => (
                <tr key={bid.id} className="border-t border-stone-200">
                  <td className="px-4 py-3 font-semibold text-stone-800">{bid.artworkTitle}</td>
                  <td className="px-4 py-3 text-stone-700">{formatCurrency(bid.price)}</td>
                  <td className="px-4 py-3 text-stone-600">{formatDateTime(bid.timestamp)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </section>
  )
}
