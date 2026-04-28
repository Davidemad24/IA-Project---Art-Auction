import { Navigate, Outlet, useLocation } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'
import { APP_ROUTES } from '../constants/routes'
import { ROLES } from '../constants/roles'
import { canAccessRoleRoute } from './roleGuard'

export function RoleProtectedRoute({ allowedRoles = [], requireApprovedArtist = false }) {
  const location = useLocation()
  const user = useAuthStore((state) => state.user)

  if (!user) {
    return <Navigate replace to={`${APP_ROUTES.AUTH_LOGIN}?next=${encodeURIComponent(location.pathname)}`} />
  }

  const isAllowed = canAccessRoleRoute({
    isAuthenticated: true,
    userRole: user?.role,
    allowedRoles,
  })

  if (!isAllowed) {
    return <Navigate replace to={APP_ROUTES.UNAUTHORIZED} />
  }
  

  if (
    requireApprovedArtist &&
    user?.role === "Artist" &&
    user?.adminId === null
  ) {
    return <Navigate replace to={APP_ROUTES.ARTIST_PENDING} />
  }

  return <Outlet />
}
