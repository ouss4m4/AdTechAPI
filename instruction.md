camp sample

{
"name": "Adobe Coupon Code",
"AdvertiserId": 1,
"Platforms": [1, 3],
"Verticals": [4, 5, 6],
"Countries": [226, 227]
}

Let’s break this down into clear backend tasks.

1. Authentication & Security
   • Implement JWT-based authentication for API access.
   • Define roles & permissions (Advertiser, Publisher, Admin).
   • Secure endpoints based on roles.

2. Database Schema (SQL)
   • Clients table (Advertisers & Publishers).
   • Campaigns table (Belongs to an Advertiser).
   • Traffic Sources table (Belongs to a Publisher, has a Traffic Type).
   • Placements table (Links a Publisher, Traffic Source, and Vertical).
   • Clicks table (Stores placement, campaign, user agent, and metadata).
   • Postbacks table (Tracks conversion events).

3. API Endpoints
   • Campaign Management
   • Create, update, delete campaigns.
   • Set targeting (geo, device, vertical).
   • Traffic Source Management
   • Create traffic sources for publishers.
   • Placement Management
   • Select a publisher, traffic source, vertical.
   • Generate placement URL (click.domain.com/in/{placement_id}).
   • Ad Selection API
   • Resolve placement, traffic type, vertical.
   • Extract User-Agent data.
   • Select an active campaign based on targeting.
   • Postback Endpoint
   • Accept conversion tracking (/postback?cid={click_id}&revenue=1).

4. Caching & Background Processing
   • Use Redis to cache active campaigns & placements.
   • Implement a queue (e.g., Hangfire, BackgroundService) for:
   • Processing postbacks.
   • Rebuilding campaign/placement caches.

5. Reporting & Rollups
   • Implement aggregated reports (impressions, clicks, conversions).
   • Optimize SQL queries for performance.
