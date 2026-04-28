import { describe, expect, it } from 'vitest'
import { canAccessRoleRoute } from './roleGuard'

describe('canAccessRoleRoute', () => {
  it('returns false when user is not authenticated', () => {
    expect(
      canAccessRoleRoute({
        isAuthenticated: false,
        userRole: 'buyer',
        allowedRoles: ['buyer'],
      }),
    ).toBe(false)
  })

  it('returns true when role is allowed', () => {
    expect(
      canAccessRoleRoute({
        isAuthenticated: true,
        userRole: 'admin',
        allowedRoles: ['admin'],
      }),
    ).toBe(true)
  })

  it('returns false when role is not allowed', () => {
    expect(
      canAccessRoleRoute({
        isAuthenticated: true,
        userRole: 'artist',
        allowedRoles: ['admin'],
      }),
    ).toBe(false)
  })
})
