import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { z } from 'zod'
import { useAuthStore } from '../../store/authStore'
import { APP_ROUTES } from '../../constants/routes'
import { formatCurrency } from '../../utils/currency'
import {
  completePayment,
  getPaymentNotification,
} from '../../services/notificationsService'
import { createEncryptedFields, encryptText } from '../../utils/secureData'

const paymentSchema = z.object({
  cardHolderName: z.string().min(3, 'Card holder name is required.'),
  cardNumber: z
    .string()
    .regex(/^\d{16}$/, 'Card number must be exactly 16 digits.'),
  expiry: z
    .string()
    .regex(/^(0[1-9]|1[0-2])\/\d{2}$/, 'Expiry must be in MM/YY format.'),
  cvv: z
    .string()
    .regex(/^\d{3}$/, 'CVV must be exactly 3 digits.'),
  billingAddress: z.string().min(8, 'Billing address is required.'),
})

function formatCardNumberInput(rawValue) {
  const digitsOnly = rawValue.replace(/\D/g, '').slice(0, 16)
  return digitsOnly.replace(/(\d{4})(?=\d)/g, '$1 ').trim()
}

export function PaymentPage() {
  const { notificationId } = useParams()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const user = useAuthStore((state) => state.user)

  const [values, setValues] = useState({
    cardHolderName: '',
    cardNumber: '',
    expiry: '',
    cvv: '',
    billingAddress: '',
  })
  const [encryptedDraft, setEncryptedDraft] = useState({})
  const [error, setError] = useState('')

  const { data: notification, isLoading } = useQuery({
    queryKey: ['payment-notification', notificationId, user?.id],
    queryFn: () => getPaymentNotification(notificationId, user.id),
    enabled: Boolean(notificationId && user?.id),
  })

  const mutation = useMutation({
    mutationFn: ({ plainPayload, encryptedPayload }) =>
      completePayment({
        notificationId,
        userId: user.id,
        paymentDetails: plainPayload,
        encryptedPaymentDetails: encryptedPayload,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications', user.id] })
      queryClient.invalidateQueries({ queryKey: ['purchases', user.id] })
      queryClient.invalidateQueries({ queryKey: ['watchlist', user.id] })
      queryClient.invalidateQueries({ queryKey: ['artworks', 'public'] })
      queryClient.invalidateQueries({ queryKey: ['bids', 'ongoing-feed'] })
      navigate(APP_ROUTES.BUYER_PURCHASES)
    },
    onError: (err) => {
      setError(err.message)
    },
  })

  const onChange = (event) => {
    const { name, value } = event.target

    if (name === 'cardNumber') {
      const formattedCardNumber = formatCardNumberInput(value)
      setValues((current) => ({ ...current, cardNumber: formattedCardNumber }))
      encryptText(formattedCardNumber.replace(/\s/g, '')).then((encryptedValue) => {
        setEncryptedDraft((current) => ({ ...current, cardNumberEncrypted: encryptedValue }))
      })
      return
    }

    setValues((current) => ({ ...current, [name]: value }))
    encryptText(value).then((encryptedValue) => {
      setEncryptedDraft((current) => ({ ...current, [`${name}Encrypted`]: encryptedValue }))
    })
  }

  const onSubmit = async (event) => {
    event.preventDefault()
    setError('')

    const payload = {
      ...values,
      cardNumber: values.cardNumber.replace(/\s/g, ''),
    }

    const parsed = paymentSchema.safeParse(payload)

    if (!parsed.success) {
      setError(parsed.error.issues[0].message)
      return
    }

    const encryptedPayload =
      Object.keys(encryptedDraft).length >= 5
        ? encryptedDraft
        : await createEncryptedFields(parsed.data, [
            'cardHolderName',
            'cardNumber',
            'expiry',
            'cvv',
            'billingAddress',
          ])

    mutation.mutate({
      plainPayload: parsed.data,
      encryptedPayload,
    })
  }

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading payment details...</p>
  }

  if (!notification) {
    return (
      <div className="rounded-xl border border-dashed border-stone-300 p-6 text-sm text-stone-600">
        Payment request not found.
      </div>
    )
  }

  if (notification.paymentStatus === 'paid') {
    return (
      <section className="space-y-4 rounded-xl border border-stone-200 bg-white p-6">
        <h1 className="text-2xl font-extrabold text-stone-900">Payment already completed</h1>
        <p className="text-sm text-stone-600">This purchase has already been paid.</p>
        <Link
          to={APP_ROUTES.BUYER_NOTIFICATIONS}
          className="inline-flex rounded-lg bg-stone-900 px-4 py-2 text-sm font-semibold text-white"
        >
          Back to notifications
        </Link>
      </section>
    )
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Complete payment</h1>

      <div className="rounded-xl border border-stone-200 bg-white p-4 text-sm text-stone-700">
        <p className="font-semibold">Order summary</p>
        <p className="mt-1">{notification.message}</p>
        <p className="mt-1 font-bold text-stone-900">
          Total: {formatCurrency(notification.amount || 0)}
        </p>
      </div>

      <form onSubmit={onSubmit} className="grid gap-4 rounded-xl border border-stone-200 bg-white p-4 sm:grid-cols-2">
        <label className="text-sm font-semibold text-stone-700 sm:col-span-2">
          Card holder name
          <input
            name="cardHolderName"
            value={values.cardHolderName}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700 sm:col-span-2">
          Card number
          <input
            name="cardNumber"
            value={values.cardNumber}
            onChange={onChange}
            inputMode="numeric"
            maxLength={19}
            autoComplete="cc-number"
            placeholder="1234 1234 1234 1234"
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          Expiry (MM/YY)
          <input
            name="expiry"
            value={values.expiry}
            onChange={onChange}
            placeholder="08/27"
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700">
          CVV
          <input
            name="cvv"
            value={values.cvv}
            onChange={onChange}
            placeholder="123"
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        <label className="text-sm font-semibold text-stone-700 sm:col-span-2">
          Billing address
          <textarea
            name="billingAddress"
            value={values.billingAddress}
            onChange={onChange}
            rows={3}
            className="mt-1 w-full rounded-lg border border-stone-300 px-3 py-2"
          />
        </label>

        {error ? <p className="sm:col-span-2 text-sm font-semibold text-rose-700">{error}</p> : null}

        <button
          type="submit"
          disabled={mutation.isPending}
          className="sm:col-span-2 rounded-lg bg-stone-900 px-4 py-2.5 text-sm font-semibold text-white disabled:opacity-70"
        >
          {mutation.isPending ? 'Processing payment...' : 'Pay now'}
        </button>
      </form>
    </section>
  )
}
