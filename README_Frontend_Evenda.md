
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
### 🏃‍♂️ Step 2: Running with IIS Express (Local Development)

1. Open the Solution in Visual Studio
•	Launch Visual Studio 2022.
•	Open your solution (.sln file).

2. Set the Startup Project
•	In Solution Explorer, right-click your main project (e.g., Evenda.UI or Evenda.MVC).
•	Select Set as Startup Project.

3. Select IIS Express
•	At the top of Visual Studio, next to the green play (▶️) button, ensure the dropdown says IIS Express.
•	If not, select it from the dropdown.

4. Run the Project
•	Press F5 (Debug) or Ctrl+F5 (Run without debugging).
•	Visual Studio will build your project and launch it using IIS Express.
•	Your browser will open to a URL like https://localhost:44366/ (the port may vary).

5. Configure appsettings.json (if needed)
•	Make sure your ApiSettings:BaseUrl points to the correct API endpoint accessible from your local machine.

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
