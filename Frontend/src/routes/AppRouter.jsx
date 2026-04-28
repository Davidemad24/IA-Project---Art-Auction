import { Navigate, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../layouts/MainLayout'
import { DashboardLayout } from '../layouts/DashboardLayout'
import { RoleProtectedRoute } from '../guards/RoleProtectedRoute'
import { UnauthorizedPage } from '../pages/system/UnauthorizedPage'
import { NotFoundPage } from '../pages/system/NotFoundPage'
import { HomePage } from '../pages/public/HomePage'
import { ArtworksListPage } from '../pages/public/ArtworksListPage'
import { ArtworkDetailPage } from '../pages/public/ArtworkDetailPage'
import { LoginPage } from '../pages/auth/LoginPage'
import { BuyerRegisterPage } from '../pages/auth/BuyerRegisterPage'
import { ArtistRegisterPage } from '../pages/auth/ArtistRegisterPage'
import { PendingApprovalPage } from '../pages/auth/PendingApprovalPage'
import { WatchlistPage } from '../pages/buyer/WatchlistPage'
import { MyBidsPage } from '../pages/buyer/MyBidsPage'
import { NotificationsPage } from '../pages/buyer/NotificationsPage'
import { PaymentPage } from '../pages/buyer/PaymentPage'
import { BoughtArtworksPage } from '../pages/buyer/BoughtArtworksPage'
import { ArtistDashboardPage } from '../pages/artist/ArtistDashboardPage'
import { MyArtworksPage } from '../pages/artist/MyArtworksPage'
import { ArtworkFormPage } from '../pages/artist/ArtworkFormPage'
import { AuctionExtensionPage } from '../pages/artist/AuctionExtensionPage'
import { AdminDashboardPage } from '../pages/admin/AdminDashboardPage'
import { PendingArtistsPage } from '../pages/admin/PendingArtistsPage'
import { PendingArtworksPage } from '../pages/admin/PendingArtworksPage'
import { AcceptedArtistsPage } from '../pages/admin/AcceptedArtistsPage'
import { APP_ROUTES } from '../constants/routes'
import { ROLES } from '../constants/roles'

const buyerLinks = [
  { to: APP_ROUTES.BUYER_WATCHLIST, label: 'Watchlist', end: true },
  { to: APP_ROUTES.BUYER_BIDS, label: 'My Bids' },
  { to: APP_ROUTES.BUYER_NOTIFICATIONS, label: 'Notifications' },
  { to: APP_ROUTES.BUYER_PURCHASES, label: 'My Purchases' },
]

const artistLinks = [
  { to: APP_ROUTES.ARTIST_DASHBOARD, label: 'Overview', end: true },
  { to: APP_ROUTES.ARTIST_ARTWORKS, label: 'My Artworks', end: true },
  { to: APP_ROUTES.ARTIST_ARTWORK_NEW, label: 'Create Artwork', end: true },
  { to: APP_ROUTES.ARTIST_AUCTION_EXTENSION, label: 'Extend Auction' },
]

const adminLinks = [
  { to: APP_ROUTES.ADMIN_DASHBOARD, label: 'Overview', end: true },
  { to: APP_ROUTES.ADMIN_PENDING_ARTISTS, label: 'Pending Artists' },
  { to: APP_ROUTES.ADMIN_ACCEPTED_ARTISTS, label: 'Accepted Artists' },
  { to: APP_ROUTES.ADMIN_PENDING_ARTWORKS, label: 'Pending Artworks' },
]

export function AppRouter() {
  return (
    <Routes>
      <Route path={APP_ROUTES.HOME} element={<MainLayout />}>
        <Route index element={<HomePage />} />
        <Route path="artworks" element={<ArtworksListPage />} />
        <Route path="artworks/:artworkId" element={<ArtworkDetailPage />} />

        <Route path="auth/login" element={<LoginPage />} />
        <Route path="auth/register/buyer" element={<BuyerRegisterPage />} />
        <Route path="auth/register/artist" element={<ArtistRegisterPage />} />

        <Route path="artist/pending-approval" element={<PendingApprovalPage />} />
        <Route path="unauthorized" element={<UnauthorizedPage />} />

        <Route element={<RoleProtectedRoute allowedRoles={"Buyer"} />}>
          <Route
            path="buyer"
            element={
              <DashboardLayout
                title="Buyer Area"
                subtitle="Track watched auctions and your bidding activity."
                links={buyerLinks}
              />
            }
          >
            <Route index element={<Navigate to="watchlist" replace />} />
            <Route path="watchlist" element={<WatchlistPage />} />
            <Route path="my-bids" element={<MyBidsPage />} />
            <Route path="notifications" element={<NotificationsPage />} />
            <Route path="purchases" element={<BoughtArtworksPage />} />
            <Route path="payment/:notificationId" element={<PaymentPage />} />
          </Route>
        </Route>

        <Route
          element={
            <RoleProtectedRoute
              allowedRoles={"Artist"}
              requireApprovedArtist
            />
          }
        >
          <Route
            path="artist"
            element={
              <DashboardLayout
                title="Artist Area"
                subtitle="Manage artwork posts and auction timelines."
                links={artistLinks}
              />
            }
          >
            <Route index element={<Navigate to="dashboard" replace />} />
            <Route path="dashboard" element={<ArtistDashboardPage />} />
            <Route path="artworks" element={<MyArtworksPage />} />
            <Route path="artworks/new" element={<ArtworkFormPage />} />
            <Route path="artworks/:artworkId/edit" element={<ArtworkFormPage />} />
            <Route path="auction-extension" element={<AuctionExtensionPage />} />
          </Route>
        </Route>

        <Route element={<RoleProtectedRoute allowedRoles={"Admin"} />}>
          <Route
            path="admin"
            element={
              <DashboardLayout
                title="Admin Area"
                subtitle="Approve artist accounts and artwork submissions."
                links={adminLinks}
              />
            }
          >
            <Route index element={<Navigate to="dashboard" replace />} />
            <Route path="dashboard" element={<AdminDashboardPage />} />
            <Route path="pending-artists" element={<PendingArtistsPage />} />
            <Route path="accepted-artists" element={<AcceptedArtistsPage />} />
            <Route path="pending-artworks" element={<PendingArtworksPage />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  )
}
