import { fireEvent, render, screen } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import { FilterBar } from './FilterBar'

describe('FilterBar', () => {
  it('sends parsed filters to callback', () => {
    const onChange = vi.fn()

    render(<FilterBar onChange={onChange} />)

    fireEvent.change(screen.getByPlaceholderText('Search by artist'), {
      target: { value: 'Lina' },
    })
    fireEvent.change(screen.getByPlaceholderText('modern, city'), {
      target: { value: 'modern, city' },
    })

    expect(onChange).toHaveBeenCalled()
    expect(onChange).toHaveBeenLastCalledWith(
      expect.objectContaining({ artistName: 'Lina', tags: ['modern', 'city'] }),
    )
  })
})
