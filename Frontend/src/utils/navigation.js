import { APP_ROUTES } from '../constants/routes'
import { ROLES } from '../constants/roles'

export function routeForRole(role) {
  if (role === "Admin") return APP_ROUTES.ADMIN_DASHBOARD;
  if (role === "Artist") return APP_ROUTES.ARTIST_DASHBOARD
  if (role === "Buyer") return APP_ROUTES.BUYER_WATCHLIST
  return APP_ROUTES.HOME
}
