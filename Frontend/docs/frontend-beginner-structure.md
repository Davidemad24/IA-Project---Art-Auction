# Frontend Beginner Structure

This folder is a beginner-friendly copy of `frontend-full`, reorganized by layer (`pages`, `components`, `services`, etc.) while keeping all current functionality.

## Run Commands

From VS Code terminal:

```powershell
cd "D:\College\Programming\Art Aution\frontend-beginner"
pnpm install --ignore-workspace
pnpm dev
```

Validation:

```powershell
pnpm lint
pnpm test
pnpm build
```

## Main Folder Layout

```txt
src/
  api/                # axios client
  components/         # reusable UI + feature components
  constants/          # routes, roles, categories
  contracts/          # shared JSDoc types/contracts
  data/mocks/         # in-memory mock DB + msw handlers
  guards/             # role guard helpers + protected route
  hooks/              # useArtworks, useAuctionRealtime
  layouts/            # MainLayout, DashboardLayout
  pages/              # grouped screens by role/domain
    public/
    auth/
    buyer/
    artist/
    admin/
    system/
  providers/          # React Query provider
  realtime/           # SignalR auction hub service
  routes/             # AppRouter
  services/           # API-like service functions
  store/              # Zustand auth store
  utils/              # time, currency, validators, filters, helpers
  index.css
  main.jsx
```

## Notes

- `frontend-full` was not changed.
- `frontend-beginner` now uses the same behavior with simpler folder mental model for team onboarding.
