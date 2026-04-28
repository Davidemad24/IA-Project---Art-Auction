import { useEffect, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate, useParams } from 'react-router-dom'
import { z } from 'zod'
import { useAuthStore } from '../../store/authStore'
import { ARTWORK_CATEGORIES } from '../../constants/categories'
import { DEFAULT_ARTWORK_IMAGE } from '../../constants/images'
import { APP_ROUTES } from '../../constants/routes'
import { createArtwork, getArtistArtworkById, updateArtwork } from '../../services/artistService'
import { ImageUploadWithPreview } from '../../components/artist/ImageUploadWithPreview'
import { parseTagsInput } from '../../utils/filterParser'

const schema = z.object({
  title: z.string().min(3, 'Title is required.'),
  description: z.string().min(20, 'Description should be at least 20 characters.'),
  initialPrice: z.coerce.number().positive('Initial price must be greater than 0.'),
  auctionStartTime: z.string().min(1, 'Start time is required.'),
  auctionEndTime: z.string().min(1, 'End time is required.'),
  category: z.string().min(1, 'Category is required.'),
  tags: z.string().min(1, 'At least one tag is required.'),
})

function toDateTimeLocalValue(value) {
  if (!value) return ''

  const date = new Date(value)
  if (Number.isNaN(date.getTime())) {
    return String(value).slice(0, 16)
  }

  const pad = (num) => String(num).padStart(2, '0')

  return [
    date.getFullYear(),
    '-',
    pad(date.getMonth() + 1),
    '-',
    pad(date.getDate()),
    'T',
    pad(date.getHours()),
    ':',
    pad(date.getMinutes()),
  ].join('')
}

export function ArtworkFormPage() {
  const { artworkId } = useParams()
  const navigate = useNavigate()
  const user = useAuthStore((state) => state.user)
  const queryClient = useQueryClient()
  const isEditMode = Boolean(artworkId)

  const [form, setForm] = useState({
    title: '',
    description: '',
    initialPrice: 100,
    auctionStartTime: '',
    auctionEndTime: '',
    category: ARTWORK_CATEGORIES[0],
    tags: '',
    imageUrl: '',
  })
  const [error, setError] = useState('')

  const { data: artworkToEdit, isLoading: isLoadingArtwork } = useQuery({
    queryKey: ['artist-artwork', user?.id, artworkId],
    queryFn: () => getArtistArtworkById(artworkId, user.id),
    enabled: Boolean(isEditMode && user?.id),
  })

  useEffect(() => {
    if (!isEditMode || !artworkToEdit) return

    // eslint-disable-next-line react-hooks/set-state-in-effect
    setForm({
      title: artworkToEdit.title || '',
      description: artworkToEdit.description || '',
      initialPrice: artworkToEdit.initialPrice || 100,
      auctionStartTime: toDateTimeLocalValue(artworkToEdit.auctionStartTime),
      auctionEndTime: toDateTimeLocalValue(artworkToEdit.auctionEndTime),
      category: artworkToEdit.category || ARTWORK_CATEGORIES[0],
      tags: Array.isArray(artworkToEdit.tags) ? artworkToEdit.tags.join(', ') : '',
      imageUrl: artworkToEdit.imageUrl || '',
    })
  }, [isEditMode, artworkToEdit])

  const mutation = useMutation({
    mutationFn: (payload) =>
      isEditMode ? updateArtwork(artworkId, user.id, payload) : createArtwork(user.id, payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['artist-artworks', user.id] })
      queryClient.invalidateQueries({ queryKey: ['artist-artwork', user.id, artworkId] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
      setError('')

      if (isEditMode) {
        navigate(APP_ROUTES.ARTIST_ARTWORKS)
        return
      }

      setForm({
        title: '',
        description: '',
        initialPrice: 100,
        auctionStartTime: '',
        auctionEndTime: '',
        category: ARTWORK_CATEGORIES[0],
        tags: '',
        imageUrl: '',
      })
    },
    onError: (err) => {
      setError(err.message)
    },
  })

  const onChange = (event) => {
    const { name, value } = event.target
    setForm((current) => ({ ...current, [name]: value }))
  }

  const onSubmit = (event) => {
    event.preventDefault()
    setError('')

    const parsed = schema.safeParse(form)
    if (!parsed.success) {
      setError(parsed.error.issues[0].message)
      return
    }

    mutation.mutate({
      ...parsed.data,
      tags: parseTagsInput(parsed.data.tags),
      imageUrl: form.imageUrl || DEFAULT_ARTWORK_IMAGE,
    })
  }

  if (isEditMode && isLoadingArtwork) {
    return <p className="text-sm text-stone-600">Loading artwork for editing...</p>
  }

  if (isEditMode && !artworkToEdit) {
    return (
      <div className="rounded-xl border border-dashed border-stone-300 p-6 text-sm text-stone-600">
        Artwork not found or you do not have access to edit it.
      </div>
    )
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">
        {isEditMode ? 'Edit artwork post' : 'Create artwork post'}
      </h1>

      <form onSubmit={onSubmit} className="grid gap-4 rounded-xl border border-stone-200 bg-white p-4 sm:grid-cols-2">
        <label className="text-sm font-semibold text-stone-700 sm:col-span-2">
          Title
          <input
            name="title"
            value={form.title}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700 sm:col-span-2">
          Description
          <textarea
            name="description"
            value={form.description}
            onChange={onChange}
            rows={4}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Initial price
          <input
            type="number"
            name="initialPrice"
            value={form.initialPrice}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Auction start
          <input
            type="datetime-local"
            name="auctionStartTime"
            value={form.auctionStartTime}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Auction end
          <input
            type="datetime-local"
            name="auctionEndTime"
            value={form.auctionEndTime}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Category
          <select
            name="category"
            value={form.category}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          >
            {ARTWORK_CATEGORIES.map((category) => (
              <option key={category.id} value={category.name}>
                {category.name}
              </option>
            ))}
          </select>
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Tags (comma separated)
          <input
            name="tags"
            value={form.tags}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <div className="sm:col-span-2">
          <ImageUploadWithPreview
            value={form.imageUrl}
            onChange={({ previewUrl }) => setForm((current) => ({ ...current, imageUrl: previewUrl }))}
          />
        </div>

        {error ? <p className="sm:col-span-2 text-sm font-semibold text-rose-700">{error}</p> : null}

        <button
          type="submit"
          disabled={mutation.isPending}
          className="sm:col-span-2 rounded-lg bg-[var(--primary)] px-4 py-2.5 text-sm font-semibold text-white disabled:opacity-70"
        >
          {mutation.isPending
            ? 'Saving...'
            : isEditMode
              ? 'Save artwork changes'
              : 'Submit artwork for approval'}
        </button>
      </form>
    </section>
  )
}
