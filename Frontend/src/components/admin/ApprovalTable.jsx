import { useState } from 'react'
import { StatusBadge } from '../ui/StatusBadge'

export function ApprovalTable({ title, rows, onApprove, onReject, isLoading }) {
  const [processingId, setProcessingId] = useState(null)

  const handleAction = async (id, action) => {
    setProcessingId(id)
    try {
      await action(id)
    } finally {
      setProcessingId(null)
    }
  }

  return (
    <section className="space-y-3">
      <h2 className="text-xl font-extrabold text-stone-900">{title}</h2>

      {isLoading ? <p className="text-sm text-stone-600">Loading...</p> : null}

      {!isLoading && rows.length === 0 ? (
        <p className="rounded-lg border border-dashed border-stone-300 p-4 text-sm text-stone-600">
          Nothing pending.
        </p>
      ) : null}

      {rows.length > 0 ? (
        <div className="overflow-x-auto rounded-lg border border-[var(--line)]">
          <table className="min-w-full bg-white text-left text-sm">
            <thead className="bg-stone-100 text-xs uppercase tracking-wide text-stone-700">
              <tr>
                <th className="px-4 py-3">Name</th>
                <th className="px-4 py-3">Meta</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((row) => (
                <tr key={row.id} className="border-t border-stone-200">
                  <td className="px-4 py-3 font-semibold text-stone-800">{row.title}</td>
                  <td className="px-4 py-3 text-stone-600">{row.meta}</td>
                  <td className="px-4 py-3">
                    <StatusBadge status={"pending"} />
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex items-center gap-2">
                      <button
                        type="button"
                        onClick={() => handleAction(row.id, onApprove)}
                        disabled={processingId === row.id}
                        className="rounded-md bg-emerald-600 px-3 py-1.5 text-xs font-semibold text-white disabled:opacity-70"
                      >
                        Approve
                      </button>
                      <button
                        type="button"
                        onClick={() => handleAction(row.id, onReject)}
                        disabled={processingId === row.id}
                        className="rounded-md bg-rose-600 px-3 py-1.5 text-xs font-semibold text-white disabled:opacity-70"
                      >
                        Reject
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : null}
    </section>
  )
}
