import { cn } from '../../utils/cn'

export function ConfirmationModal({
  open,
  title,
  description,
  confirmText = 'Confirm',
  cancelText = 'Cancel',
  onConfirm,
  onCancel,
}) {
  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4">
      <div className="w-full max-w-md rounded-2xl bg-white p-6 shadow-2xl">
        <h3 className="text-xl font-bold text-stone-900">{title}</h3>
        <p className="mt-2 text-sm text-stone-600">{description}</p>

        <div className="mt-6 flex items-center justify-end gap-3">
          <button
            type="button"
            onClick={onCancel}
            className="rounded-lg border border-stone-300 px-4 py-2 text-sm font-semibold text-stone-700"
          >
            {cancelText}
          </button>
          <button
            type="button"
            onClick={onConfirm}
            className={cn(
              'rounded-lg px-4 py-2 text-sm font-semibold text-white',
              'bg-[var(--primary)] hover:opacity-90',
            )}
          >
            {confirmText}
          </button>
        </div>
      </div>
    </div>
  )
}
