import { Link } from 'react-router-dom'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useAuthStore } from '../../store/authStore'
import {
  listNotifications,
  markNotificationAsRead,
} from '../../services/notificationsService'
import { APP_ROUTES } from '../../constants/routes'
import { formatCurrency } from '../../utils/currency'
import { formatDateTime } from '../../utils/time'

export function NotificationsPage() {
  const user = useAuthStore((state) => state.user)
  const queryClient = useQueryClient()

  const { data: notifications = [], isLoading } = useQuery({
    queryKey: ['notifications', user?.id],
    queryFn: () => listNotifications(user.id),
    enabled: Boolean(user?.id),
    refetchInterval: 5000,
    refetchIntervalInBackground: true,
  })

  const markReadMutation = useMutation({
    mutationFn: (notificationId) => markNotificationAsRead(notificationId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications', user.id] })
    },
  })

  if (isLoading) {
    return <p className="text-sm text-stone-600">Loading notifications...</p>
  }

  return (
    <section className="space-y-4">
      <h1 className="text-2xl font-extrabold text-stone-900">Notifications</h1>

      {notifications.length === 0 ? (
        <p className="rounded-lg border border-dashed border-stone-300 p-4 text-sm text-stone-600">
          No notifications yet.
        </p>
      ) : (
        <div className="space-y-3">
          {notifications.map((note) => (
            <article
              key={note.id}
              className="rounded-xl border border-stone-200 bg-white p-4"
            >
              <div className="flex items-start justify-between gap-3">
                <div>
                  <h2 className="font-bold text-stone-900">{note.title}</h2>
                  <p className="text-sm text-stone-700">{note.message}</p>
                  {note.type === 'payment_required' && note.amount ? (
                    <p className="mt-1 text-xs font-semibold text-stone-700">
                      Amount due: {formatCurrency(note.amount)}
                    </p>
                  ) : null}
                  <p className="mt-1 text-xs text-stone-500">{formatDateTime(note.timestamp)}</p>
                </div>
                {note.type === 'payment_required' && note.paymentStatus !== 'paid' ? (
                  <Link
                    to={APP_ROUTES.BUYER_PAYMENT.replace(':notificationId', note.id)}
                    className="rounded-lg bg-stone-900 px-3 py-1.5 text-xs font-semibold text-white"
                  >
                    Proceed to payment
                  </Link>
                ) : !note.isRead ? (
                  <button
                    type="button"
                    onClick={() => markReadMutation.mutate(note.id)}
                    className="rounded-lg border border-stone-300 px-3 py-1.5 text-xs font-semibold text-stone-700"
                  >
                    Mark read
                  </button>
                ) : (
                  <span className="text-xs font-semibold text-emerald-700">
                    {note.paymentStatus === 'paid' ? 'Paid' : 'Read'}
                  </span>
                )}
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}
