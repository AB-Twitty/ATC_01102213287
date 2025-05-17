
# 🎟️ Evenda – Full Stack Event Booking System

**Evenda** is a full-stack web application that allows users to explore and book event tickets, while offering a powerful admin panel for managing events, bookings, and users.

This repository contains two main projects:

- [`Evenda.API`](./Evenda.API): ASP.NET Core 8.0 Web API (Backend)
- [`Evenda.MVC`](./Evenda.MVC): ASP.NET Core 8.0 MVC Frontend (UI & Views)

---

## 🌟 Key Features

### 👤 Authentication
- Register / Login (JWT + Refresh Token)
- Forgot Password via OTP Email
- Role-based Authorization (Admin, Customer)

### 📅 Event Management
- Browse Events with Pagination & Tags
- Book Tickets (1-click booking)
- Event Details Page with Image Gallery
- Admin Panel

### 🎛️ Admin Dashboard
- Filtering, Sorting, Table Page Size Control
- Add/Edit/Delete Events with Multiple Images

### 🌐 User Interface
- Built with ASP.NET Core MVC 8.0
- Inspinia + Bootstrap UI
- Dark Mode Toggle
- Fully Responsive Design

---

## 🚀 Quick Start

### 1️⃣ Clone the Repo

```bash
git clone https://github.com/your-username/evenda.git
cd evenda
```

### 2️⃣ Set Up the Backend

Follow the [Backend README](./README_Backend_Evenda.md) to:

- Configure connection strings and secrets
- Apply migrations and run the API

### 3️⃣ Set Up the Frontend

Follow the [Frontend README](./README_Frontend_Evenda.md) to:

- Configure API base URL
- Launch the UI with dark mode and event browsing

---

## 🔐 Admin Credentials

```
Email:    admin@evenda.com  
Password: 123@Admin
```

---

## 🛠 Technologies Used

- ASP.NET Core 8.0 (Web API + MVC)
- Entity Framework Core
- SQL Server
- JWT + Refresh Tokens + Cookies
- SMTP with HTML Emails
- Bootstrap + Inspinia Template
- Razor Views + Layouts
- RESTful Integration

---

## 🧪 Bonus Features Implemented

- ✅ Forgot Password with OTP Email
- ✅ Multiple Image Uploads per Event
- ✅ Filtering + Sorting + Pagination
- ✅ Responsive UI + Dark Mode
- ✅ Tags and Categories
- ✅ Global Exception Handling & Custom Error Pages

---

## 📄 License

This project is built for demo and evaluation purposes.

---
