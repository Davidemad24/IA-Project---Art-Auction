import { useMemo, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import { formatDateTime } from '../../utils/time'
import { extendAuction, listArtistArtworks } from '../../services/artistService'

export function AuctionExtensionPage() {
  const user = useAuthStore((state) => state.user)
  const queryClient = useQueryClient()

  const [artworkId, setArtworkId] = useState('')
  const [newEndTime, setNewEndTime] = useState('')
  const [message, setMessage] = useState('')

  const { data: artworks = [], isLoading } = useQuery({
    queryKey: ['artist-artworks', user?.id],
    queryFn: () => listArtistArtworks(user.id),
    enabled: Boolean(user?.id),
  })

  const effectiveArtworkId = useMemo(() => {
    if (!artworks.length) return ''
    const exists = artworks.some((artwork) => artwork.id === artworkId)
    return exists ? artworkId : artworks[0].id
  }, [artworks, artworkId])

  const selectedArtwork = useMemo(
    () => artworks.find((artwork) => artwork.id === effectiveArtworkId),
    [artworks, effectiveArtworkId],
  )

  const mutation = useMutation({
    mutationFn: () => extendAuction(effectiveArtworkId, user.id, new Date(newEndTime).toISOString()),
    onSuccess: () => {
      setMessage('Auction end time updated successfully.')
      queryClient.invalidateQueries({ queryKey: ['artist-artworks', user.id] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public', effectiveArtworkId] })
    },
    onError: (error) => setMessage(error.message),
  })

  const onSubmit = (event) => {
    event.preventDefault()
    setMessage('')

    if (!effectiveArtworkId) {
      setMessage('Please select an artwork first.')
      return
    }

    if (!newEndTime) {
      setMessage('Please select a new end time.')
      return
    }

    if (selectedArtwork && new Date(newEndTime) <= new Date(selectedArtwork.auctionEndTime)) {
      setMessage('New end time must be after the current auction end time.')
      return
    }

    mutation.mutate()
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Extend auction end time</h1>
      <form onSubmit={onSubmit} className="space-y-3 rounded-xl border border-stone-200 bg-white p-4">
        <label className="block text-sm font-semibold text-stone-700">
          Select artwork
          <select
            value={effectiveArtworkId}
            onChange={(event) => setArtworkId(event.target.value)}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
            disabled={isLoading || artworks.length === 0}
          >
            {isLoading ? <option>Loading artworks...</option> : null}
            {!isLoading && artworks.length === 0 ? (
              <option value="">No uploaded artworks found</option>
            ) : null}
            {artworks.map((artwork) => (
              <option key={artwork.id} value={artwork.id}>
                {artwork.title} ({artwork.id})
              </option>
            ))}
          </select>
        </label>

        {selectedArtwork ? (
          <div className="rounded-lg border border-stone-200 bg-stone-50 px-3 py-2 text-xs text-stone-700">
            Current end time: {formatDateTime(selectedArtwork.auctionEndTime)}
          </div>
        ) : null}

        <label className="block text-sm font-semibold text-stone-700">
          New auction end time
          <input
            type="datetime-local"
            value={newEndTime}
            onChange={(event) => setNewEndTime(event.target.value)}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        {message ? <p className="text-sm font-semibold text-stone-700">{message}</p> : null}

        <button
          type="submit"
          className="rounded-lg bg-stone-900 px-4 py-2 text-sm font-semibold text-white"
          disabled={mutation.isPending || artworks.length === 0}
        >
          {mutation.isPending ? 'Updating...' : 'Extend'}
        </button>
      </form>
    </section>
  )
}
