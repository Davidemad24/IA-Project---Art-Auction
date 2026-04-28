import { fireEvent, render, screen } from '@testing-library/react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import { ImageUploadWithPreview } from './ImageUploadWithPreview'

describe('ImageUploadWithPreview', () => {
  const MockFileReader = class {
    readAsDataURL() {
      this.result = 'data:image/png;base64,mock-preview-url'
      if (this.onload) {
        this.onload()
      }
    }
  }

  beforeEach(() => {
    vi.stubGlobal('FileReader', MockFileReader)
  })

  it('emits file + preview on file selection', () => {
    const onChange = vi.fn()

    render(<ImageUploadWithPreview value="" onChange={onChange} />)

    const file = new File(['sample'], 'painting.png', { type: 'image/png' })
    const input = screen.getByLabelText('Artwork image')

    fireEvent.change(input, { target: { files: [file] } })

    expect(onChange).toHaveBeenCalledWith({ file, previewUrl: 'data:image/png;base64,mock-preview-url' })
    expect(screen.getByAltText('Artwork preview')).toHaveAttribute('src', 'data:image/png;base64,mock-preview-url')
  })
})
