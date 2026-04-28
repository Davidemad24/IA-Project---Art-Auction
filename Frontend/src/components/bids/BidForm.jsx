import { useState } from 'react'
import { z } from 'zod'
import { getMinimumNextBid } from '../../utils/validators'

export function BidForm({
  currentBid,
  initialPrice,
  onSubmit,
  canBid,
  hint,
  isSubmitting,
  errorMessage,
}) {
  const minimum = getMinimumNextBid(currentBid, initialPrice)

  const [amount, setAmount] = useState(minimum)
  const [localError, setLocalError] = useState('')

  const schema = z.object({
    amount: z.coerce.number().min(minimum, `Bid must be at least $${minimum}.`),
  })

  const handleSubmit = (event) => {
    event.preventDefault()
    setLocalError('')

    const parsed = schema.safeParse({ amount })

    if (!parsed.success) {
      setLocalError(parsed.error.issues[0].message)
      return
    }

    onSubmit(parsed.data.amount)
  }

  return (
    <form
      noValidate
      onSubmit={handleSubmit}
      className="space-y-3 rounded-xl border border-[var(--line)] bg-white p-4"
    >
      <label className="block text-sm font-semibold text-stone-700">
        Place a bid
        <input
          type="number"
          min={minimum}
          step={10}
          value={amount}
          onChange={(event) => setAmount(event.target.value)}
          disabled={!canBid}
          className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2 disabled:bg-stone-100"
        />
      </label>

      {hint ? <p className="text-xs font-semibold text-stone-500">{hint}</p> : null}
      {localError ? <p className="text-xs font-semibold text-rose-700">{localError}</p> : null}
      {errorMessage ? <p className="text-xs font-semibold text-rose-700">{errorMessage}</p> : null}

      <button
        type="submit"
        disabled={!canBid || isSubmitting}
        className="rounded-lg bg-stone-900 px-4 py-2 text-sm font-semibold text-white disabled:cursor-not-allowed disabled:opacity-70"
      >
        {isSubmitting ? 'Submitting bid...' : 'Submit bid'}
      </button>
    </form>
  )
}
