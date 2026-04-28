import { Link } from 'react-router-dom'
import { APP_ROUTES } from '../../constants/routes'

export function PendingApprovalPage() {
  return (
    <section className="mx-auto w-full max-w-xl rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-8 text-center shadow-sm">
      <h1 className="text-3xl font-extrabold text-stone-900">Artist approval pending</h1>
      <p className="mt-2 text-stone-600">
        Your artist account is under admin review. You can log in, but artist tools are unlocked after approval.
      </p>
      <div className="mt-6 flex items-center justify-center gap-3">
        <Link
          to={APP_ROUTES.AUTH_LOGIN}
          className="rounded-lg bg-[var(--primary)] px-4 py-2 text-sm font-semibold text-white"
        >
          Go to login
        </Link>
        <Link
          to={APP_ROUTES.ARTWORKS}
          className="rounded-lg border border-stone-300 px-4 py-2 text-sm font-semibold text-stone-700"
        >
          Browse artworks
        </Link>
      </div>
    </section>
  )
}
