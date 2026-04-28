import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { z } from 'zod'
import { AuthFormShell } from '../../components/auth/AuthFormShell'
import { PasswordField } from '../../components/auth/PasswordField'
import { registerBuyer } from '../../services/authService'
import { APP_ROUTES } from '../../constants/routes'

// 1. Updated Schema to match RegisterBuyerDto
const schema = z
  .object({
    fullName: z.string().min(2, 'Full name is required.'),
    email: z.string().email('Please enter a valid email.'),
    password: z.string().min(8, 'Password should be at least 8 characters.'),
    confirmPassword: z.string().min(8, 'Confirm password is required.'),
    city: z.string().min(1, 'City is required.'),
    country: z.string().min(1, 'Country is required.'),
    phoneNumber: z.string().min(5, 'Phone number is required.'),
    address: z.string().min(5, 'Full address is required.'),
  })
  .refine((values) => values.password === values.confirmPassword, {
    message: 'Passwords do not match.',
    path: ['confirmPassword'],
  })

export function BuyerRegisterPage() {
  const navigate = useNavigate()
  
  // 2. Updated state keys
  const [values, setValues] = useState({
    fullName: '',
    email: '',
    password: '',
    confirmPassword: '',
    city: '',
    country: '',
    phoneNumber: '',
    address: '',
  })
  
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
      // This sends the full object to your authService
      await registerBuyer(parsed.data)
      navigate(APP_ROUTES.AUTH_LOGIN)
    } catch (err) {
      setError(typeof err === 'string' ? err : err.message || 'Registration failed')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <AuthFormShell
      title="Buyer registration"
      subtitle="Create an account to bid and track artworks."
      footer={
        <>
          <span>Already registered? </span>
          <Link className="font-semibold text-[var(--primary)]" to={APP_ROUTES.AUTH_LOGIN}>
            Log in
          </Link>
        </>
      }
    >
      <form className="space-y-4" onSubmit={onSubmit}>
        {/* Full Name */}
        <label className="block text-sm font-semibold text-stone-700">
          Full Name
          <input
            name="fullName"
            value={values.fullName}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
          />
        </label>

        {/* Email */}
        <label className="block text-sm font-semibold text-stone-700">
          Email
          <input
            name="email"
            type="email"
            value={values.email}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
          />
        </label>

        {/* City & Country Grid */}
        <div className="grid grid-cols-2 gap-4">
          <label className="block text-sm font-semibold text-stone-700">
            City
            <input
              name="city"
              value={values.city}
              onChange={onChange}
              className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
            />
          </label>
          <label className="block text-sm font-semibold text-stone-700">
            Country
            <input
              name="country"
              value={values.country}
              onChange={onChange}
              className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
            />
          </label>
        </div>

        {/* Phone Number */}
        <label className="block text-sm font-semibold text-stone-700">
          Phone Number
          <input
            name="phoneNumber"
            value={values.phoneNumber}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
          />
        </label>

        {/* Address */}
        <label className="block text-sm font-semibold text-stone-700">
          Full Address
          <input
            name="address"
            value={values.address}
            onChange={onChange}
            className="mt-1 w-full rounded-lg border border-stone-300 bg-white px-3 py-2"
          />
        </label>

        {/* Passwords */}
        <PasswordField
          label="Password"
          name="password"
          value={values.password}
          onChange={onChange}
          autoComplete="new-password"
        />

        <PasswordField
          label="Confirm password"
          name="confirmPassword"
          value={values.confirmPassword}
          onChange={onChange}
          autoComplete="new-password"
        />

        {error ? <p className="text-sm font-semibold text-rose-700">{error}</p> : null}

        <button
          type="submit"
          disabled={submitting}
          className="w-full rounded-lg bg-[var(--primary)] px-4 py-2.5 text-sm font-semibold text-white disabled:cursor-not-allowed disabled:opacity-70"
        >
          {submitting ? 'Registering...' : 'Create Buyer Account'}
        </button>
      </form>
    </AuthFormShell>
  )
}