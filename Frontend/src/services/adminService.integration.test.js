import { beforeEach, describe, expect, it } from 'vitest'
import { resetMockDb } from '../data/mocks/inMemoryDb'
import {
  deleteApprovedArtist,
  getApprovedArtists,
  getPendingArtworks,
  reviewArtwork,
} from './adminService'
import { listPublicArtworks } from './artworksService'

describe('admin and artwork integration', () => {
  beforeEach(() => {
    resetMockDb()
  })

  it('approved pending artwork becomes visible in public listings', async () => {
    const pending = await getPendingArtworks()
    expect(pending.length).toBeGreaterThan(0)

    const targetId = pending[0].id

    const before = await listPublicArtworks()
    await reviewArtwork(targetId, 'approve')
    const after = await listPublicArtworks()

    expect(after.length).toBe(before.length + 1)
  })

  it('deleting an approved artist removes all of their artworks', async () => {
    const approvedArtists = await getApprovedArtists()
    expect(approvedArtists.length).toBeGreaterThan(0)

    const targetArtist = approvedArtists.find((artist) => artist.artworksCount > 0)
    expect(targetArtist).toBeDefined()

    const before = await listPublicArtworks()
    const beforeCountForArtist = before.filter((artwork) => artwork.artistId === targetArtist.id).length

    expect(beforeCountForArtist).toBeGreaterThan(0)

    await deleteApprovedArtist(targetArtist.id)

    const after = await listPublicArtworks()
    const afterCountForArtist = after.filter((artwork) => artwork.artistId === targetArtist.id).length

    expect(afterCountForArtist).toBe(0)
    expect(after.length).toBe(before.length - beforeCountForArtist)
  })
})
