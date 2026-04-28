import { Link } from 'react-router-dom'
import { APP_ROUTES } from '../../constants/routes'

export function UnauthorizedPage() {
  return (
    <div className="rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-8 text-center">
      <h1 className="text-3xl font-extrabold text-stone-900">Unauthorized</h1>
      <p className="mt-2 text-stone-600">Your account does not have permission to access this area.</p>
      <Link
        to={APP_ROUTES.HOME}
        className="mt-5 inline-flex rounded-lg bg-[var(--primary)] px-4 py-2 text-sm font-semibold text-white"
      >
        Return home
      </Link>
    </div>
  )
}
