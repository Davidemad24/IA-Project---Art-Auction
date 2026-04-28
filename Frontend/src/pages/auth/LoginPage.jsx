import { useState } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { z } from 'zod'
import { AuthFormShell } from '../../components/auth/AuthFormShell'
import { PasswordField } from '../../components/auth/PasswordField'
import { login } from '../../services/authService'
import { useAuthStore } from '../../store/authStore'
import { APP_ROUTES } from '../../constants/routes'
import { ROLES } from '../../constants/roles'
import { routeForRole } from '../../utils/navigation'

const schema = z.object({
  email: z.string().email('Please enter a valid email.'),
  password: z.string().min(6, 'Password is required.'),
})

export function LoginPage() {
  const [searchParams] = useSearchParams()
  const setSession = useAuthStore((state) => state.setSession)
  const navigate = useNavigate()

  const [values, setValues] = useState({ email: 'admin@art.com', password: 'P@ssword123' })
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)

  const onChange = (event) => {
    const { name, value } = event.target
    setValues((current) => ({ ...current, [name]: value }))
  }

  const onSubmit = async (event) => {
    event.preventDefault()
    setError('')

    const parsed = schema.safeParse(values)
    if (!parsed.success) {
      setError(parsed.error.issues[0].message)
      return
    }

    setSubmitting(true)

    try {
      const response = await login(parsed.data)
      setSession(response)

      const nextPath = searchParams.get('next')
      if (nextPath) {
        navigate(nextPath)
        return
      }

      if (response.user.role === "Artist" && response.user.adminId === null) {
        navigate(APP_ROUTES.ARTIST_PENDING)
        return
      }

      navigate(routeForRole(response.user.role))
    } catch (err) {
      setError(err.message)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <AuthFormShell
      title="Welcome back"
      subtitle="Sign in to place bids, manage artworks, or review approvals."
      footer={
        <>
          <span>Need an account? </span>
          <Link className="font-semibold text-[var(--primary)]" to={APP_ROUTES.AUTH_BUYER_REGISTER}>
            Register as Buyer
          </Link>
          <span> or </span>
          <Link className="font-semibold text-[var(--primary)]" to={APP_ROUTES.AUTH_ARTIST_REGISTER}>
            Register as Artist
          </Link>
        </>
      }
    >
      <form onSubmit={onSubmit} className="space-y-4">
        <label className="block text-sm font-semibold text-stone-700">
          Email
          <input
            className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
            type="email"
            name="email"
            value={values.email}
            onChange={onChange}
          />
        </label>

        <PasswordField
          label="Password"
          name="password"
          value={values.password}
          onChange={onChange}
          autoComplete="current-password"
        />

        {error ? <p className="text-sm font-semibold text-rose-700">{error}</p> : null}

        <button
          type="submit"
          disabled={submitting}
          className="w-full rounded-lg bg-[var(--primary)] px-4 py-2.5 text-sm font-semibold text-white disabled:cursor-not-allowed disabled:opacity-70"
        >
          {submitting ? 'Logging in...' : 'Log in'}
        </button>
      </form>
    </AuthFormShell>
  )
}
