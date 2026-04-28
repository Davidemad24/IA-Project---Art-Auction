export function parseTagsInput(tagValue = '') {
  return tagValue
    .split(',')
    .map((tag) => tag.trim())
    .filter(Boolean)
}

export function normalizeArtworkFilters(rawFilters = {}) {
  return {
    artistName: rawFilters.artistName?.trim().toLowerCase() || '',
    category: rawFilters.category?.trim().toLowerCase() || '',
    tags: Array.isArray(rawFilters.tags)
      ? rawFilters.tags
          .map((tag) => tag.trim().toLowerCase())
          .filter(Boolean)
      : parseTagsInput(rawFilters.tags || '').map((tag) => tag.toLowerCase()),
  }
}

export function filterArtworks(artworks, normalizedFilters) {
  return artworks.filter((artwork) => {
    const artistMatch =
      !normalizedFilters.artistName ||
      artwork.artistName.toLowerCase().includes(normalizedFilters.artistName)

    const categoryMatch =
      !normalizedFilters.category ||
      artwork.categoryName.toLowerCase() === normalizedFilters.category

    const tagsMatch =
      normalizedFilters.tags.length === 0 ||
      normalizedFilters.tags.every((wantedTag) =>
        artwork.tags.map((tag) => tag.toLowerCase()).includes(wantedTag),
      )

    return artistMatch && categoryMatch && tagsMatch
  })
}
