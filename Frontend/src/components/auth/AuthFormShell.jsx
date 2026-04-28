export function AuthFormShell({ title, subtitle, children, footer }) {
  return (
    <section className="mx-auto w-full max-w-lg rounded-2xl border border-[var(--line)] bg-[var(--surface)] p-6 shadow-sm sm:p-8">
      <h1 className="text-3xl font-extrabold text-stone-900">{title}</h1>
      {subtitle ? <p className="mt-2 text-sm text-stone-600">{subtitle}</p> : null}
      <div className="mt-6">{children}</div>
      {footer ? <div className="mt-6 text-sm text-stone-600">{footer}</div> : null}
    </section>
  )
}
