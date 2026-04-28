import { fireEvent, render, screen } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import { BidForm } from './BidForm'

describe('BidForm', () => {
  it('blocks bids below minimum and shows validation message', () => {
    const onSubmit = vi.fn()

    render(
      <BidForm
        currentBid={100}
        initialPrice={100}
        onSubmit={onSubmit}
        canBid
        isSubmitting={false}
      />,
    )

    fireEvent.change(screen.getByLabelText('Place a bid'), {
      target: { value: 105 },
    })
    fireEvent.click(screen.getByRole('button', { name: 'Submit bid' }))

    expect(onSubmit).not.toHaveBeenCalled()
    expect(screen.getByText('Bid must be at least $110.')).toBeInTheDocument()
  })

  it('submits valid bid', () => {
    const onSubmit = vi.fn()

    render(
      <BidForm
        currentBid={100}
        initialPrice={100}
        onSubmit={onSubmit}
        canBid
        isSubmitting={false}
      />,
    )

    fireEvent.change(screen.getByLabelText('Place a bid'), {
      target: { value: 120 },
    })
    fireEvent.click(screen.getByRole('button', { name: 'Submit bid' }))

    expect(onSubmit).toHaveBeenCalledWith(120)
  })
})
