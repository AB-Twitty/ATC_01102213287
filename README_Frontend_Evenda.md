
# 🎨 Evenda Frontend – Event Booking Web UI

This is the frontend web application for **Evenda**, an event booking system built with **ASP.NET Core MVC 8.0**. It communicates with the Evenda Web API using `HttpClient` and renders dynamic, responsive, and user-friendly views using **Inspinia**, **Bootstrap**, and custom UI plugins.

---

## 🧱 Project Structure

```
Evenda.MVC
│
├── Controllers           → MVC Controllers 
├── Views                 → Razor Views and Layouts
├── wwwroot               → Static files (CSS, JS, Images)
├── Dtos                  → Data-Transfer-Objects (Dtos)
├── ApiClients            → HTTP Clients to communicate with the API
├── Middleware            → Exception handling and response filters
├── Extensions            → Extensions Methods for (IServiceCollection, ModelState, ...)
├── Helpers               → Helper services and utils
└── Models                → ViewModels 
```

---

## 🚀 Features

- ✅ User Registration and Login
- ✅ JWT Authentication + Cookie Storage
- ✅ Forgot Password + OTP Email Template
- ✅ Browse Events (Grid view with Pagination)
- ✅ Book Tickets (1-click)
- ✅ "Booked" Label for already reserved events
- ✅ Event Details Page
- ✅ Admin Dashboard (CRUD Events)
- ✅ Manage Multiple Event Images
- ✅ Filtering, Sorting, Pagination for Admin Views
- ✅ Role-based Navigation and Permissions
- ✅ Dark Mode Toggle (Global)
- ✅ Fully Responsive Layout
- ✅ Global Error Handling Pages (404, 500, etc.)
- ✅ Deployed on Somee

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
- Visual Studio 2022+ (recommended) or VS Code

---

### 📂 Step 1: Clone the Project

```bash
git clone https://github.com/your-username/evenda-frontend.git
cd evenda-frontend
```

---

### ⚙️ Step 2: Configure `appsettings.json`

Inside `Evenda.MVC`, create or modify `appsettings.json` like this:

```json
"ApiSettings": {
  "BaseUrl": "https://evendaapi.somee.com/api"
}
```

> Make sure the `BaseUrl` points to your deployed or local API server.

---

### ▶️ Step 3: Run the Application

Run the MVC app using:

```bash
dotnet run --project Evenda.MVC
```

Or from Visual Studio:
- Set `Evenda.MVC` as the startup project
- Press `F5` to launch

The frontend will be accessible at:
```
https://localhost:5002
http://localhost:5001
```

> The ports may differ based on your local settings or launch profile.

---

## 🎨 UI & Plugins

- 🧱 [Inspinia](https://wrapbootstrap.com/theme/inspinia-responsive-admin-theme-WB0R5L90S)
- 💄 Bootstrap 5.0
- 🎯 VirtualSelect (multi-tag selection)
- ⌛ Pace.js (page loading indicator)
- 🌑 Dark Mode
- 📸 Font Awesome Icons

---

## 🧪 Features Summary

| Feature              | User | Admin |
|----------------------|------|-------|
| View Events          | ✅   | ✅    |
| Book Events          | ✅   | ❌    |
| Manage Bookings      | ✅   | ❌    |
| CRUD Events          | ❌   | ✅    |
| Add Event Tags/Images| ❌   | ✅    |
| Forgot Password      | ✅   | ✅    |
| Responsive UI        | ✅   | ✅    |
| Dark Mode            | ✅   | ✅    |

---

## 🛠 Technologies Used

- ASP.NET Core MVC 8.0
- Razor Views
- HttpClient + Cookie Auth
- Bootstrap + Inspinia Template
- FontAwesome, Pace.js, VirtualSelect
- RESTful API Integration

---

## 📄 License

This project is for educational and evaluation purposes.

---
