import { fireEvent, render, screen } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import { ApprovalTable } from './ApprovalTable'

describe('ApprovalTable', () => {
  it('runs approve action for selected row', async () => {
    const onApprove = vi.fn().mockResolvedValue(undefined)
    const onReject = vi.fn().mockResolvedValue(undefined)

    render(
      <ApprovalTable
        title="Pending"
        rows={[
          { id: 'a1', title: 'Mariam Aziz', meta: 'artist@example.com', status: 'pending' },
        ]}
        onApprove={onApprove}
        onReject={onReject}
        isLoading={false}
      />, 
    )

    fireEvent.click(screen.getByRole('button', { name: 'Approve' }))

    expect(onApprove).toHaveBeenCalledWith('a1')
  })
})
