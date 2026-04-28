export function StatusBadge({ status }) {
  const variants = {
    approved: 'bg-emerald-100 text-emerald-800',
    pending: 'bg-amber-100 text-amber-800',
    rejected: 'bg-rose-100 text-rose-800',
    live: 'bg-blue-100 text-blue-800',
    upcoming: 'bg-slate-100 text-slate-800',
    ended: 'bg-rose-100 text-rose-800',
  }

  return (
    <span
      className={`inline-flex rounded-full px-3 py-1 text-xs font-semibold ${variants[status] || 'bg-zinc-100 text-zinc-700'}`}
    >
      {status}
    </span>
  )
}
