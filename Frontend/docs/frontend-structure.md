# ArtAuction Frontend Structure

## 1) Setup and Environment

### Prerequisites
- Node.js 20+ (current machine: Node 24)
- npm (for first-time Corepack setup)
- Git

### Setup Commands (Windows PowerShell)
```powershell
npm install --global corepack@latest
corepack enable pnpm
pnpm --version
pnpm create vite frontend --template react
cd frontend
corepack use pnpm@latest-10
pnpm install
pnpm add react-router-dom @tanstack/react-query zustand axios zod @microsoft/signalr dayjs clsx
pnpm add -D tailwindcss @tailwindcss/vite vitest @testing-library/react @testing-library/jest-dom msw eslint prettier
pnpm add -D jsdom
```

### Run
```powershell
pnpm dev
pnpm test
pnpm build
```

## 2) Source Tree (Feature-First)

```txt
frontend/
  docs/
    frontend-structure.md
  src/
    app/
      guards/
      layouts/
      pages/
      providers/
      router/
    shared/
      api/
      constants/
      contracts/
      lib/
      realtime/
      ui/
    features/
      auth/
      artworks/
      bids/
      watchlist/
      artist/
      admin/
      notifications/
    mocks/
    test/
```

## 3) Ownership Matrix (Parallel Team Work)

| Area | Owner | Notes |
|---|---|---|
| App shell, routing, providers, guards | Codex | Stable integration base for all teams |
| Shared contracts, API client, SignalR service | Codex | All features must consume these interfaces |
| Admin pages + approval flow | Codex | Implemented in first sprint |
| Buyer pages/components | Team | Watchlist, bids, notifications refinements |
| Artist pages/components | Team | CRUD and extension refinements |
| Integration and final polish | Shared | Merge per feature branch |

## 4) Routes and Role Guards

| Route Group | Example Paths | Access Rule |
|---|---|---|
| Public | `/`, `/artworks`, `/artworks/:artworkId` | Visitor access allowed |
| Auth | `/auth/login`, `/auth/register/buyer`, `/auth/register/artist` | Public |
| Buyer | `/buyer/watchlist`, `/buyer/my-bids`, `/buyer/notifications` | `buyer` role required |
| Artist | `/artist/dashboard`, `/artist/artworks`, `/artist/artworks/new`, `/artist/auction-extension` | `artist` role + approved status |
| Admin | `/admin/dashboard`, `/admin/pending-artists`, `/admin/pending-artworks` | `admin` role required |
| Utility | `/artist/pending-approval`, `/unauthorized` | Redirect targets |

## 5) Shared Component Catalog

- `RoleProtectedRoute`: role-based route guard and pending-artist redirect.
- `ArtworkCard`: public listing card with timer, tags, bid value.
- `AuctionTimer`: live/upcoming/ended timer label.
- `FilterBar`: artist/category/tag filter input.
- `BidForm`: bid submission with $10 minimum increment validation.
- `BidHistoryTable`: visible bid history (visitor-safe).
- `ImageUploadWithPreview`: local image upload with preview URL.
- `StatusBadge`: moderation/auction status label.
- `ConfirmationModal`: reusable confirm dialog.

## 6) API Contract Targets (Frontend-facing)

| Domain | Intended Endpoints |
|---|---|
| Auth | `POST /auth/login`, `POST /auth/register/buyer`, `POST /auth/register/artist` |
| Public Artworks | `GET /artworks`, `GET /artworks/{id}` |
| Bids | `GET /artworks/{id}/bids`, `POST /artworks/{id}/bids` |
| Watchlist | `GET /watchlist`, `POST /watchlist/{artworkId}`, `DELETE /watchlist/{artworkId}` |
| Artist | `GET /artist/artworks`, `POST /artist/artworks`, `PUT /artist/artworks/{id}`, `DELETE /artist/artworks/{id}`, `PATCH /artist/artworks/{id}/extend` |
| Admin | `GET /admin/pending-artists`, `PATCH /admin/artists/{id}/review`, `GET /admin/pending-artworks`, `PATCH /admin/artworks/{id}/review` |
| Notifications | `GET /notifications`, `PATCH /notifications/{id}/read` |

## 7) SignalR Contract Table

| Item | Value |
|---|---|
| Hub Path | `/hubs/auctions` |
| Client actions | `JoinAuctionRoom(artworkId)`, `LeaveAuctionRoom(artworkId)` |
| Server events consumed | `BidPlaced`, `AuctionExtended`, `AuctionEnded`, `WinnerNotified` |
| Frontend behavior | Invalidate artwork/bid queries and refresh detail/list UIs |

## 8) Testing Coverage (Current)

- Unit tests:
  - Bid increment and auction active validators.
  - Role guard access logic.
  - Filter parser + filter function.
- Component tests:
  - `FilterBar`
  - `BidForm`
  - `ImageUploadWithPreview`
  - `ApprovalTable`
- Integration test:
  - Admin artwork approval updates public artwork visibility.

## 9) Future: Docker + Security Checklist

### Docker readiness (when backend is added)
1. Keep monorepo layout:
   - `/frontend`
   - `/backend`
2. Add `frontend/Dockerfile` (multi-stage: build with pnpm, serve static assets).
3. Add `backend/Dockerfile` (.NET build + runtime image).
4. Add root `docker-compose.yml` linking frontend + backend networks.
5. Move API/hub URLs to env vars:
   - `VITE_API_BASE_URL`
   - `VITE_AUCTION_HUB_URL`
6. Add health checks and production-ready restart policy.

### JWT hardening checklist (next phase)
1. Add secure login/logout flow with real JWT from backend.
2. Store token in secure strategy (prefer httpOnly cookie with CSRF defense).
3. Attach JWT in API and SignalR connection auth.
4. Enforce role and claim checks in route guards and UI actions.
5. Add token refresh and forced logout on expiry.
6. Add audit logging for admin approve/reject actions.
