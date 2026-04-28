import { Link } from 'react-router-dom'
import { APP_ROUTES } from '../../constants/routes'
import { DEFAULT_ARTWORK_IMAGE } from '../../constants/images'
import { formatCurrency } from '../../utils/currency'
import { AuctionTimer } from './AuctionTimer'
import { StatusBadge } from '../ui/StatusBadge'
import { getAuctionState } from '../../utils/time'

export function ArtworkCard({
  artwork,
  showWatchlistAction = false,
  isInWatchlist = false,
  isUpdatingWatchlist = false,
  onToggleWatchlist,
}) {
  const auctionStatus = getAuctionState(artwork.auctionStartTime, artwork.auctionEndTime)

  return (
    <article className="overflow-hidden rounded-2xl border border-[var(--line)] bg-white shadow-sm transition hover:-translate-y-0.5 hover:shadow-lg">
      <img
        src={artwork.image || DEFAULT_ARTWORK_IMAGE}
        alt={artwork.title}
        onError={(event) => {
          event.currentTarget.onerror = null
          event.currentTarget.src = DEFAULT_ARTWORK_IMAGE
        }}
        className="h-52 w-full object-cover"
      />

      <div className="space-y-3 p-4">
        <div className="flex items-start justify-between gap-3">
          <div>
            <h3 className="text-lg font-extrabold text-stone-900">{artwork.title}</h3>
            <p className="text-sm text-stone-600">{artwork.artistName}</p>
          </div>
          <StatusBadge status={auctionStatus} />
        </div>

        <p className="line-clamp-2 text-sm text-stone-600">{artwork.description}</p>

        <div className="flex flex-wrap items-center gap-2 text-xs font-semibold text-stone-600">
          <span className="rounded-md bg-stone-100 px-2 py-1">{artwork.categoryName}</span>
          {artwork.tags.map((tag) => (
            <span key={tag} className="rounded-md border border-stone-200 px-2 py-1">
              #{tag}
            </span>
          ))}
        </div>

        <div className="space-y-1">
          <p className="text-sm text-stone-600">
            Current bid: <span className="font-extrabold text-stone-900">{formatCurrency(artwork.buyNewPrice)}</span>
          </p>
          <AuctionTimer
            startTime={artwork.startDate}
            endTime={artwork.endDate}
            className="text-xs font-semibold text-stone-600"
          />
        </div>

        <div className="flex flex-wrap items-center gap-2">
          <Link
            to={APP_ROUTES.ARTWORKS + `/${artwork.id}`}
            className="inline-flex h-10 min-w-[170px] items-center justify-center rounded-lg bg-stone-900 px-4 py-2 text-sm font-semibold text-white"
          >
            View auction
          </Link>

          {showWatchlistAction ? (
            <button
              type="button"
              onClick={onToggleWatchlist}
              disabled={isUpdatingWatchlist}
              className="inline-flex h-10 min-w-[170px] items-center justify-center rounded-lg border border-stone-300 px-4 py-2 text-sm font-semibold text-stone-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              {isUpdatingWatchlist
                ? 'Updating...'
                : isInWatchlist
                  ? 'Remove from watchlist'
                  : 'Save to watchlist'}
            </button>
          ) : null}
        </div>
      </div>
    </article>
  )
}
