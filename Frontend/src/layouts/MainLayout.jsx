import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'
import { APP_ROUTES } from '../constants/routes'
import { routeForRole } from '../utils/navigation'
import { WinnerNotificationPopup } from '../components/buyer/WinnerNotificationPopup'

const navLinks = [
  { to: APP_ROUTES.HOME, label: 'Home' },
  { to: APP_ROUTES.ARTWORKS, label: 'Artworks' },
]

export function MainLayout() {
  const user = useAuthStore((state) => state.user)
  const logout = useAuthStore((state) => state.logout)
  const navigate = useNavigate()

  const onLogout = () => {
    logout()
    navigate(APP_ROUTES.HOME)
  }

  return (
    <div className="min-h-screen">
      <header className="sticky top-0 z-30 border-b border-[var(--line)] bg-[var(--surface)]/95 backdrop-blur">
        <div className="mx-auto flex w-full max-w-6xl items-center justify-between px-4 py-3 sm:px-6">
          <Link to={APP_ROUTES.HOME} className="text-lg font-extrabold tracking-tight text-stone-900">
            ArtAuction
          </Link>

          <nav className="hidden items-center gap-4 text-sm font-semibold text-stone-700 md:flex">
            {navLinks.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  isActive ? 'text-[var(--primary)] underline underline-offset-8' : ''
                }
              >
                {item.label}
              </NavLink>
            ))}
            {user ? (
              <NavLink
                to={routeForRole(user.role)}
                className={({ isActive }) =>
                  isActive ? 'text-[var(--primary)] underline underline-offset-8' : ''
                }
              >
                {user.role} Area
              </NavLink>
            ) : null}
          </nav>

          <div className="flex items-center gap-2">
            {user ? (
              <>
                <span className="hidden rounded-full bg-stone-100 px-3 py-1 text-xs font-semibold text-stone-700 sm:inline-flex">
                  {user.name}
                </span>
                <button
                  type="button"
                  onClick={onLogout}
                  className="rounded-lg border border-stone-300 px-3 py-2 text-sm font-semibold text-stone-700"
                >
                  Log out
                </button>
              </>
            ) : (
              <Link
                to={APP_ROUTES.AUTH_LOGIN}
                className="rounded-lg bg-[var(--primary)] px-4 py-2 text-sm font-semibold text-white"
              >
                Log in
              </Link>
            )}
          </div>
        </div>
      </header>

      <main className="mx-auto w-full max-w-6xl px-4 py-6 sm:px-6 sm:py-8">
        <Outlet />
      </main>
      <WinnerNotificationPopup />
    </div>
  )
}
