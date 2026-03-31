# VK18 Coupon Admin

Angular 19 frontend converted from the provided `positivity_coupon_mvp (1).html`, plus a .NET 5 Web API configured for SQL Server.

## Structure

- `src/`: Angular 19 standalone application
- `backend/PositivityCoupon.Api/`: ASP.NET Core 5 Web API
- `database/`: SQL Server bootstrap script

## Frontend

1. Install packages:
   `npm install`
2. Start the Angular app:
   `npm start`

The Angular app uses the HTML prototype as the base layout and splits it into routed pages:

- `/overview`
- `/create`
- `/list`
- `/b2b`
- `/b2b/:companyId`
- `/dashboard`
- `/sessions`
- `/users`

By default the frontend points to `http://localhost:5000/api`. If the API is unavailable, the UI falls back to seeded demo data so the converted pages still render.

## Backend

The API project targets `.NET 5.0` and uses SQL Server through Entity Framework Core 5.

1. Install a .NET 5 SDK/runtime if it is not already available.
2. Update the connection string in `backend/PositivityCoupon.Api/appsettings.json`.
3. Run the API:
   `dotnet run --project backend/PositivityCoupon.Api/PositivityCoupon.Api.csproj`

The API seeds sample coupon, company, and redemption data on startup with `Database.EnsureCreated()`.

## SQL Server

Use `database/positivity-coupon-admin.sql` if you want to create the schema manually before running the API.
# vk18
