# Frontend Backend-Ready Copy (No Connection Applied)

This folder is a **separate copy** of `frontend-beginner` prepared for backend integration handoff.

- Copy name: `frontend-backend-ready`
- Backend target (for teammate integration): `D:\College\Programming\Art Aution\IA-Project---Art-Auction\Backend`
- Status: frontend-only preparation complete, **no backend connection applied yet**

## What Was Rearranged

1. Artwork images were organized into:
   - `src/assets/artworks/`
2. Database-owned sample data was extracted into separate reference JSON files:
   - `src/integration-reference/database/users.example.json`
   - `src/integration-reference/database/artworks.example.json`
   - `src/integration-reference/database/bids.example.json`
   - `src/integration-reference/database/watchlists.example.json`
   - `src/integration-reference/database/notifications.example.json`
   - `src/integration-reference/database/purchases.example.json`
3. Backend contract examples were separated into:
   - `src/integration-reference/backend/api-endpoints.example.json`
4. Asset-to-storage mapping example added:
   - `src/integration-reference/assets/artwork-files.example.json`
5. Backend environment template added:
   - `.env.backend.example`

## Data That Should Live In The Database

These are **not frontend source of truth** in production:

1. Users and roles (`admin`, `artist`, `buyer`)
2. Artist approval status (`pending`, `approved`, `rejected`)
3. Artwork records and moderation status
4. Auction windows (`auctionStartTime`, `auctionEndTime`)
5. Bids and bid history
6. Watchlists
7. Notifications (including winner/payment-required notifications)
8. Purchases and payment state
9. Secure payment fields (encrypted/tokenized at backend level)

Use the example files in `src/integration-reference/database/` as sample payload shapes only.

## Data/Assets That Should Not Stay Hardcoded In Frontend

1. Admin emails and user identities
2. Password or password-like values
3. Bid history
4. Winner/purchase notification records
5. Production artwork URLs

Local images in `src/assets/artworks/` are demo placeholders. In production they should come from backend-managed URLs (e.g., object storage or CDN), as shown in:

- `src/integration-reference/assets/artwork-files.example.json`

## Files Your Teammate Should Connect To Backend

1. API client config:
   - `src/api/client.js`
   - `.env.backend.example`
2. Service layer:
   - `src/services/authService.js`
   - `src/services/artworksService.js`
   - `src/services/bidsService.js`
   - `src/services/watchlistService.js`
   - `src/services/artistService.js`
   - `src/services/adminService.js`
   - `src/services/notificationsService.js`
3. Realtime:
   - `src/realtime/auctionHub.js` (SignalR hub `/hubs/auctions`)

## Suggested Backend Mapping Reference

Use this file as the starting endpoint contract map:

- `src/integration-reference/backend/api-endpoints.example.json`

It includes:

1. Auth endpoints
2. Public artwork/bid endpoints
3. Artist/Admin actions
4. Notification + purchase endpoints
5. SignalR events and client actions

## Important Note For Integration

Current application behavior still works with local mock data for development demos.  
When backend is connected, service functions should be switched to API calls and the mock data path should be retired from runtime.
