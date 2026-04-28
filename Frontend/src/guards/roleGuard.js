export function canAccessRoleRoute({ isAuthenticated, userRole, allowedRoles }) {
  if (!isAuthenticated) {
    return false
  }

  if (!allowedRoles?.length) {
    return true
  }

  return allowedRoles.includes(userRole)
}
