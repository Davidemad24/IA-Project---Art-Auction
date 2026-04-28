import { useEffect, useMemo, useRef, useState } from 'react'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { APP_ROUTES } from '../../constants/routes'
import { ROLES } from '../../constants/roles'
import { auctionHubService } from '../../realtime/auctionHub'
import { listNotifications } from '../../services/notificationsService'
import { useAuthStore } from '../../store/authStore'
import { formatCurrency } from '../../utils/currency'

export function WinnerNotificationPopup() {
  const user = useAuthStore((state) => state.user)
  const isBuyer = user?.role === ROLES.BUYER
  const queryClient = useQueryClient()
  const navigate = useNavigate()
  const knownNotificationIdsRef = useRef(new Set())
  const dismissedNotificationIdsRef = useRef(new Set())
  const [activeNotificationId, setActiveNotificationId] = useState(null)

  const { data: notifications = [] } = useQuery({
    queryKey: ['notifications', user?.id],
    queryFn: () => listNotifications(user.id),
    enabled: Boolean(isBuyer && user?.id),
    refetchInterval: 5000,
    refetchIntervalInBackground: true,
  })

  const paymentRequiredNotifications = useMemo(
    () =>
      notifications
        .filter((notification) => notification.type === 'payment_required')
        .filter((notification) => notification.paymentStatus !== 'paid')
        .sort((a, b) => new Date(b.timestamp).getTime() - new Date(a.timestamp).getTime()),
    [notifications],
  )

  useEffect(() => {
    knownNotificationIdsRef.current = new Set()
    dismissedNotificationIdsRef.current = new Set()
    setActiveNotificationId(null)
  }, [user?.id])

  useEffect(() => {
    if (!isBuyer || !paymentRequiredNotifications.length) {
      setActiveNotificationId(null)
      return
    }

    if (knownNotificationIdsRef.current.size === 0) {
      paymentRequiredNotifications.forEach((notification) => {
        knownNotificationIdsRef.current.add(notification.id)
      })

      const latest = paymentRequiredNotifications.find(
        (notification) => !dismissedNotificationIdsRef.current.has(notification.id),
      )

      if (latest) {
        setActiveNotificationId(latest.id)
      }
      return
    }

    const incomingNotification = paymentRequiredNotifications.find(
      (notification) => !knownNotificationIdsRef.current.has(notification.id),
    )

    paymentRequiredNotifications.forEach((notification) => {
      knownNotificationIdsRef.current.add(notification.id)
    })

    if (
      incomingNotification &&
      !dismissedNotificationIdsRef.current.has(incomingNotification.id)
    ) {
      setActiveNotificationId(incomingNotification.id)
    }
  }, [isBuyer, paymentRequiredNotifications])

  useEffect(() => {
    if (!isBuyer || !user?.id) {
      return undefined
    }

    const handleWinnerNotified = (payload) => {
      if (payload.userId !== user.id) return
      if (dismissedNotificationIdsRef.current.has(payload.id)) return

      queryClient.invalidateQueries({ queryKey: ['notifications', user.id] })
      setActiveNotificationId(payload.id)
    }

    auctionHubService.subscribe('WinnerNotified', handleWinnerNotified)

    return () => {
      auctionHubService.unsubscribe('WinnerNotified', handleWinnerNotified)
    }
  }, [isBuyer, queryClient, user?.id])

  const activeNotification =
    paymentRequiredNotifications.find((notification) => notification.id === activeNotificationId) ||
    null

  if (!isBuyer || !activeNotification) {
    return null
  }

  const onDismiss = () => {
    dismissedNotificationIdsRef.current.add(activeNotification.id)
    setActiveNotificationId(null)
  }

  const onProceedToPayment = () => {
    navigate(APP_ROUTES.BUYER_PAYMENT.replace(':notificationId', activeNotification.id))
    setActiveNotificationId(null)
  }

  return (
    <div className="fixed bottom-4 right-4 z-50 w-[92vw] max-w-sm rounded-xl border border-rose-200 bg-white p-4 shadow-lg">
      <p className="text-xs font-bold uppercase tracking-wide text-rose-700">Auction ended</p>
      <h3 className="mt-1 text-lg font-extrabold text-stone-900">{activeNotification.title}</h3>
      <p className="mt-1 text-sm text-stone-700">{activeNotification.message}</p>
      <p className="mt-2 text-sm font-semibold text-stone-900">
        Amount due: {formatCurrency(activeNotification.amount || 0)}
      </p>

      <div className="mt-4 flex items-center gap-2">
        <button
          type="button"
          onClick={onProceedToPayment}
          className="rounded-lg bg-stone-900 px-3 py-2 text-xs font-semibold text-white"
        >
          Proceed to payment
        </button>
        <button
          type="button"
          onClick={onDismiss}
          className="rounded-lg border border-stone-300 px-3 py-2 text-xs font-semibold text-stone-700"
        >
          Dismiss
        </button>
      </div>
    </div>
  )
}
