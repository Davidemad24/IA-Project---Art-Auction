import { useId, useMemo, useState } from 'react'

export function ImageUploadWithPreview({ value, onChange }) {
  const [selectedPreview, setSelectedPreview] = useState('')

  const inputId = useId()
  const preview = useMemo(() => selectedPreview || value || '', [selectedPreview, value])

  const handleChange = (event) => {
    const file = event.target.files?.[0]
    if (!file) return

    const reader = new FileReader()

    reader.onload = () => {
      const previewUrl = typeof reader.result === 'string' ? reader.result : ''
      if (!previewUrl) return

      setSelectedPreview(previewUrl)
      onChange({ file, previewUrl })
    }

    reader.readAsDataURL(file)
  }

  return (
    <div className="space-y-3">
      <label htmlFor={inputId} className="block text-sm font-semibold text-stone-700">
        Artwork image
      </label>
      <input
        id={inputId}
        type="file"
        accept="image/*"
        onChange={handleChange}
        className="w-full rounded-lg border border-stone-300 bg-white px-3 py-2 text-sm"
      />
      {preview ? (
        <img
          src={preview}
          alt="Artwork preview"
          className="h-44 w-full rounded-xl border border-stone-200 object-cover"
        />
      ) : null}
    </div>
  )
}
