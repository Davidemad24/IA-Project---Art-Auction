import { useQuery } from '@tanstack/react-query'
import { PageHeader } from '../../components/ui/PageHeader'
import { getAdminDashboardStats } from '../../services/adminService'
import { da } from 'zod/locales'

export function AdminDashboardPage() {
  const { data } = useQuery({
    queryKey: ['admin-dashboard'],
    queryFn: getAdminDashboardStats,
  })

  return (
    <section className="space-y-5">
      <PageHeader title="Admin dashboard" description="Review artist registrations and artwork submissions." />

      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-4">
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Pending artists</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{data?.pendingArtists ?? 0}</p>
        </article>
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Accepted artists</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{data?.acceptedArtists ?? 0}</p>
        </article>
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Pending artworks</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{data?.pendingArtworks ?? 0}</p>
        </article>
        <article className="rounded-xl border border-stone-200 bg-white p-4">
          <p className="text-sm text-stone-600">Approved artworks</p>
          <p className="mt-2 text-3xl font-extrabold text-stone-900">{data?.liveAuctions ?? 0}</p>
        </article>
      </div>
    </section>
  )
}
