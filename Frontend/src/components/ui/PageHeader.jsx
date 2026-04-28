export function PageHeader({ title, description, action }) {
  return (
    <header className="flex flex-col gap-3 sm:flex-row sm:items-end sm:justify-between">
      <div>
        <h1 className="text-3xl font-extrabold tracking-tight text-stone-900">{title}</h1>
        {description ? <p className="mt-1 text-sm text-stone-600">{description}</p> : null}
      </div>
      {action ? <div>{action}</div> : null}
    </header>
  )
}
