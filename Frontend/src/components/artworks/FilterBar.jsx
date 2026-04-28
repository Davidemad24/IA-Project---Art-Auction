import { useState } from 'react'
import { parseTagsInput } from '../../utils/filterParser'
import { ARTWORK_CATEGORIES } from '../../constants/categories'

const defaultFilters = {
  artistName: '',
  category: '',
  tags: '',
}

export function FilterBar({ onChange }) {
  const [values, setValues] = useState(defaultFilters)

  const handleFieldChange = (event) => {
    const { name, value } = event.target

    const nextValues = {
      ...values,
      [name]: value,
    }

    setValues(nextValues)
    onChange({
      artistName: nextValues.artistName,
      category: nextValues.category,
      tags: parseTagsInput(nextValues.tags),
    })
  }

  return (
    <div className="grid gap-3 rounded-xl border border-[var(--line)] bg-white p-4 sm:grid-cols-3">
      <label className="text-sm font-semibold text-stone-700">
        Artist
        <input
          name="artistName"
          value={values.artistName}
          onChange={handleFieldChange}
          className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          placeholder="Search by artist"
        />
      </label>

      <label className="text-sm font-semibold text-stone-700">
        Category
        <select
          name="category"
          value={values.category}
          onChange={handleFieldChange}
          className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
        >
          <option value="">All categories</option>
          {ARTWORK_CATEGORIES.map((category) => (
            <option key={category.id} value={category.name}>
              {category.name}
            </option>
          ))}
        </select>
      </label>

      <label className="text-sm font-semibold text-stone-700">
        Tags
        <input
          name="tags"
          value={values.tags}
          onChange={handleFieldChange}
          className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          placeholder="modern, city"
        />
      </label>
    </div>
  )
}
