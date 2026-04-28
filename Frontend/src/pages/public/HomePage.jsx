import { data, Link } from 'react-router-dom'
import { PageHeader } from '../../components/ui/PageHeader'
import { APP_ROUTES } from '../../constants/routes'
import { DEFAULT_ARTWORK_IMAGE } from '../../constants/images'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/time'
import { AuctionTimer } from '../../components/artworks/AuctionTimer'
import { useOngoingBidFeed } from '../../hooks/useArtworks'

export function HomePage() {
  const { data: ongoingBids = [], isLoading } = useOngoingBidFeed()

  return (
    <section className="space-y-6">
      <div className="rounded-3xl border border-[var(--line)] bg-[var(--surface)] p-8 shadow-sm sm:p-10">
        <PageHeader
          title="Bid on original art in real time"
          description="Discover approved artwork auctions, place competitive bids, and follow your watchlist across live events."
          action={
            <Link
              to={APP_ROUTES.ARTWORKS}
              className="inline-flex rounded-lg bg-[var(--primary)] px-5 py-2.5 text-sm font-semibold text-white"
            >
              Browse auctions
            </Link>
          }
        />
      </div>

      <div className="rounded-3xl border border-[var(--line)] bg-[var(--surface)] p-8 shadow-sm sm:p-10">
        <h2 className="text-2xl font-extrabold text-stone-900">Ongoing Bids</h2>
        <p className="mt-1 text-sm text-stone-600">
          All live auctions and their active bid streams are listed below.
        </p>

        {isLoading ? <p className="mt-4 text-sm text-stone-600">Loading ongoing bids...</p> : null}

        {!isLoading && ongoingBids.length === 0 ? (
          <p className="mt-4 rounded-xl border border-dashed border-stone-300 bg-white p-4 text-sm text-stone-600">
            There are no live auctions right now.
          </p>
        ) : null}

        <div className="mt-6 grid gap-4 lg:grid-cols-2">
          {ongoingBids.map((artwork) => (
            <article
              key={artwork.id}
              className="rounded-2xl border border-stone-200 bg-white p-4 shadow-sm"
            >
              <div className="flex gap-3">
                <img
                  src={artwork.imageUrl || DEFAULT_ARTWORK_IMAGE}
                  alt={artwork.title}
                  onError={(event) => {
                    event.currentTarget.onerror = null
                    event.currentTarget.src = DEFAULT_ARTWORK_IMAGE
                  }}
                  className="h-24 w-24 rounded-xl object-cover"
                />

                <div className="min-w-0 flex-1">
                  <Link
                    to={`${APP_ROUTES.ARTWORKS}/${artwork.id}`}
                    className="text-lg font-extrabold text-stone-900 hover:text-[var(--primary)]"
                  >
                    {artwork.title}
                  </Link>
                  <p className="text-sm text-stone-600">by {artwork.artistName}</p>
                  <p className="mt-1 text-sm text-stone-700">
                    Current bid: <span className="font-extrabold">{formatCurrency(artwork.buyNewPrice)}</span>
                  </p>
                  <AuctionTimer
                    startTime={artwork.startDate}
                    endTime={artwork.endDate}
                    className="text-xs font-semibold text-stone-600"
                  />
                  <Link
                    to={`${APP_ROUTES.ARTWORKS}/${artwork.id}`}
                    className="mt-3 inline-flex rounded-lg bg-stone-900 px-3 py-1.5 text-xs font-semibold text-white"
                  >
                    View Artwork
                  </Link>
                </div>
              </div>

              <div className="mt-4 rounded-xl border border-stone-200 bg-stone-50 p-3">
                <h3 className="text-sm font-bold text-stone-800">Live bid feed</h3>

                {artwork.postBids.length === 0 ? (
                  <p className="mt-2 text-xs text-stone-600">No bids placed yet.</p>
                ) : (
                  <ul className="mt-2 space-y-2">
                    {artwork.postBids.map((bid) => (
                      <li
                        className="flex items-center justify-between rounded-lg bg-white px-3 py-2 text-xs"
                      >
                        <span className="font-semibold text-stone-700">{bid.buyerName}</span>
                        <span className="font-bold text-stone-900">{formatCurrency(bid.buyerPrice)}</span>
                      </li>
                    ))}
                  </ul>
                )}
              </div>
            </article>
          ))}
        </div>
      </div>
    </section>
  )
}
