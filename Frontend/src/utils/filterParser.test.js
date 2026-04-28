import { describe, expect, it } from 'vitest'
import { filterArtworks, normalizeArtworkFilters, parseTagsInput } from './filterParser'

const artworks = [
  {
    id: '1',
    artistName: 'Lina',
    category: 'Abstract',
    tags: ['modern', 'blue'],
  },
  {
    id: '2',
    artistName: 'Kareem',
    category: 'Portrait',
    tags: ['classic', 'warm'],
  },
]

describe('filterParser', () => {
  it('parses comma-separated tags', () => {
    expect(parseTagsInput('modern, blue , ,warm')).toEqual(['modern', 'blue', 'warm'])
  })

  it('normalizes string filters', () => {
    expect(
      normalizeArtworkFilters({
        artistName: ' Lina ',
        category: ' Abstract ',
        tags: 'modern',
      }),
    ).toEqual({
      artistName: 'lina',
      category: 'abstract',
      tags: ['modern'],
    })
  })

  it('filters artworks by artist, category, and tags', () => {
    const result = filterArtworks(
      artworks,
      normalizeArtworkFilters({ artistName: 'lina', category: 'abstract', tags: 'modern' }),
    )

    expect(result.map((item) => item.id)).toEqual(['1'])
  })
})
