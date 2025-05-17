
# 📦 Evenda Backend – Event Booking API

This is the backend API for **Evenda**, a full-stack event booking system built with **ASP.NET Core Web API 8.0**. It supports secure user authentication, event management, and ticket booking, with a layered architecture for scalability and maintainability.

---

## 🧱 Project Structure

```
Evenda.API                 → API endpoints (Controllers)
Evenda.Application         → Core application logic (Services, DTOs, Interfaces)
Evenda.Domain              → Entity definitions and domain models
Evenda.Persistence         → Repositories, EF Core DbContext, configurations
Evenda.Infrastructure      → JWT Provider, OTP Service, Email Sender, Hasher
```

---

## 🚀 Features

- ✅ User & Admin Authentication (JWT + Refresh Tokens)
- ✅ Role-based Access Control (Admin, Customer)
- ✅ Secure Ticket Booking System
- ✅ Email-based Forgot/Reset Password with OTP and HTML Templates
- ✅ Full CRUD for Events (Admin only)
- ✅ Upload and manage multiple event images
- ✅ Tags and Categories for Events
- ✅ Pagination, Filtering, and Sorting
- ✅ Global Exception Handling with custom error responses
- ✅ Deployed on Somee (API + Database)

---

## 🔐 Admin Credentials (for testing)

```
Email:    admin@evenda.com  
Password: 123@Admin
```

---

## 🔧 Setup Instructions (Detailed)

### 📌 Prerequisites

Ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0/runtime)
- [SQL Server Express or Developer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022+ (recommended) or VS Code
- Entity Framework CLI  
  Install if needed:  
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

### 📂 Step 1: Clone the Project

```bash
git clone https://github.com/your-username/evenda-backend.git
cd evenda-backend
```

---

### ⚙️ Step 2: Configure `appsettings.json` (Option if you want to have your local database)

Navigate to `Evenda.API/appsettings.json` and update the following section:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EvendaDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
}
```

---

### 🧱 Step 3: Apply Migrations & Create Database (only if you changed the ConnectionString in step 2)

Open a terminal and run:

```bash
cd Evenda.API
dotnet ef database update
```

This will apply all migrations and create the database.

---

### ▶️ Step 4: Run the Application

Run the API using:

```bash
dotnet run --project Evenda.API
```

Or from Visual Studio:
- Set `Evenda.API` as the startup project
- Press `F5` to run

Default URLs:
```
https://localhost:5001
http://localhost:5000
```

---

## 🔍 API Highlights

- `POST /api/Auth/register` – User registration
- `POST /api/Auth/login` – Login (returns JWT + Refresh Token)
- `POST /api/Auth/forgot-password` – Sends OTP to user email
- `POST /api/Auth/reset-password` – Resets password using OTP
- `GET /api/Events/filtered/paginated` – List events (with pagination/filtering)
- `POST /api/Tickets/book` – Book an event ticket
- `GET /api/Tickets/my-bookings` – List Bookings

> Full Swagger UI available at `/swagger`

---

## 🛠 Technologies Used

- ASP.NET Core Web API 8.0
- Entity Framework Core
- SQL Server
- JWT & Refresh Tokens
- SMTP (HTML email templates)
- Clean Architecture (Layered)
- Global Exception Middleware

---

## 📄 License

This project is for educational and evaluation purposes.

---
