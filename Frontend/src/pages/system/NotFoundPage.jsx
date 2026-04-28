import { Link } from 'react-router-dom'
import { APP_ROUTES } from '../../constants/routes'

export function NotFoundPage() {
  return (
    <div className="rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-8 text-center">
      <h1 className="text-3xl font-extrabold text-stone-900">Page not found</h1>
      <p className="mt-2 text-stone-600">The page you requested does not exist.</p>
      <Link
        to={APP_ROUTES.HOME}
        className="mt-5 inline-flex rounded-lg border border-stone-300 px-4 py-2 text-sm font-semibold text-stone-700"
      >
        Go back home
      </Link>
    </div>
  )
}
