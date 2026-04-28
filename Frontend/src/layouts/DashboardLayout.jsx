import { NavLink, Outlet } from 'react-router-dom'

export function DashboardLayout({ title, subtitle, links }) {
  return (
    <div className="grid gap-6 lg:grid-cols-[240px_1fr]">
      <aside className="h-fit rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-4 shadow-sm">
        <h2 className="text-xl font-extrabold text-stone-900">{title}</h2>
        <p className="mt-1 text-sm text-stone-600">{subtitle}</p>
        <nav className="mt-4 flex flex-col gap-2">
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              end={link.end}
              className={({ isActive }) =>
                [
                  'rounded-lg px-3 py-2 text-sm font-semibold transition',
                  isActive ? 'bg-stone-900 text-white' : 'text-stone-700 hover:bg-stone-100',
                ].join(' ')
              }
            >
              {link.label}
            </NavLink>
          ))}
        </nav>
      </aside>

      <section className="min-w-0 rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-5 shadow-sm sm:p-6">
        <Outlet />
      </section>
    </div>
  )
}
