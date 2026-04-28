import { useMemo } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { useAuthStore } from '../../store/authStore'
import { BidHistoryTable } from '../../components/artworks/BidHistoryTable'
import { AuctionTimer } from '../../components/artworks/AuctionTimer'
import { usePublicArtworkDetail } from '../../hooks/useArtworks'
import { formatCurrency } from '../../utils/currency'
import { APP_ROUTES } from '../../constants/routes'
import { ROLES } from '../../constants/roles'
import { DEFAULT_ARTWORK_IMAGE } from '../../constants/images'
import { BidForm } from '../../components/bids/BidForm'
import { placeBid } from '../../services/bidsService'
import { useAuctionRealtime } from '../../hooks/useAuctionRealtime'
import { listWatchlist, toggleWatchlist } from '../../services/watchlistService'

export function ArtworkDetailPage() {
  const { artworkId } = useParams()
  const user = useAuthStore((state) => state.user)

  const queryClient = useQueryClient();

  const { data: artwork, isLoading } = usePublicArtworkDetail(artworkId)
  const { data: watchlist = [] } = useQuery({
    queryKey: ['watchlist', user?.id],
    queryFn: () => listWatchlist(user.id),
    enabled: Boolean(user?.id && user.role === ROLES.BUYER),
  })

  useAuctionRealtime(artworkId)

  const bidMutation = useMutation({
    mutationFn: ({ amount }) => placeBid({ artworkId, bidder: user, amount }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['bids', artworkId] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public', artworkId] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
    },
  })

  const watchlistMutation = useMutation({
    mutationFn: () => toggleWatchlist(user.id, artworkId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['watchlist', user.id] })
    },
  })

  const canBid = user?.role === "Buyer"
  const isSavedToWatchlist = watchlist.some((item) => item.id === artworkId)

  const bidHint = useMemo(() => {
    if (!user) return 'Login as buyer to place bids.'
    if (user.role !== ROLES.BUYER) return 'Only buyers can place bids.'
    return null
  }, [user])

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading artwork...</p>
  }

  if (!artwork) {
    return (
      <div className="rounded-xl border border-dashed border-stone-300 p-6 text-sm text-stone-600">
        Artwork unavailable.
      </div>
    )
  }

  return (
    <section className="space-y-6">
      <Link to={APP_ROUTES.ARTWORKS} className="text-sm font-semibold text-[var(--primary)]">
        ? Back to auctions
      </Link>

      <div className="grid gap-6 lg:grid-cols-[minmax(0,1.1fr)_minmax(0,0.9fr)]">
        <img
          src={artwork.imageUrl || DEFAULT_ARTWORK_IMAGE}
          alt={artwork.title}
          onError={(event) => {
            event.currentTarget.onerror = null
            event.currentTarget.src = DEFAULT_ARTWORK_IMAGE
          }}
          className="h-full min-h-[320px] w-full rounded-2xl border border-[var(--line)] object-cover"
        />

        <div className="space-y-5 rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-5">
          <div>
            <h1 className="text-3xl font-extrabold text-stone-900">{artwork.title}</h1>
            <p className="text-sm text-stone-600">by {artwork.artistName}</p>
          </div>

          <p className="text-sm leading-relaxed text-stone-700">{artwork.description}</p>

          <div className="grid gap-2 rounded-xl bg-white p-4 text-sm text-stone-700">
            <p>
              Current bid: <span className="font-extrabold">{formatCurrency(artwork.buyNewPrice)}</span>
            </p>
            <p>Initial price: {formatCurrency(artwork.initialPrice)}</p>
            <AuctionTimer startTime={artwork.startDate} endTime={artwork.endDate} />
          </div>

          {user?.role === ROLES.BUYER ? (
            <button
              type="button"
              onClick={() => watchlistMutation.mutate()}
              disabled={watchlistMutation.isPending}
              className="rounded-lg border border-stone-300 px-4 py-2 text-sm font-semibold text-stone-700 disabled:cursor-not-allowed disabled:opacity-60"
            >
              {watchlistMutation.isPending
                ? 'Updating...'
                : isSavedToWatchlist
                  ? 'Remove from watchlist'
                  : 'Save to watchlist'}
            </button>
          ) : null}

          <BidForm
            currentBid={artwork.buyNewPrice}
            initialPrice={artwork.initialPrice}
            onSubmit={(amount) => bidMutation.mutate({ amount })}
            canBid={canBid}
            hint={bidHint}
            isSubmitting={bidMutation.isPending}
            errorMessage={bidMutation.error?.message}
          />
        </div>
      </div>

      <section className="space-y-3">
        <h2 className="text-xl font-extrabold text-stone-900">Bid history</h2>
        <BidHistoryTable bids={artwork.postBid} />
      </section>
    </section>
  )
}
