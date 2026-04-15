# 🏥 ALPHA CLINIC | Enterprise Hospital Management System

![Project Status](https://img.shields.io/badge/Status-Development-orange?style=for-the-badge)
![UI Framework](https://img.shields.io/badge/UI-WPF%20%7C%20Material%20Design-blueviolet?style=for-the-badge)
![Core](https://img.shields.io/badge/.NET-SDK%20v8.0-blue?style=for-the-badge)

**Alpha Clinic** is a professional, high-performance Hospital Management System (HMS) built with **Clean Architecture** and modern **MVVM** principles. It provides a seamless, premium interface for Administrators, Doctors, and Patients to manage clinical workflows.

---

## ✨ Key Features

### 🛡️ Admin Portal
*   **System Performance Dashboard**: Real-time monitoring of clinical stats.
*   **Doctor Management**: Full CRUD operations for medical staff.
*   **Global Patient Directory**: Centralized access to all registered patients.
*   **Master Appointment Logs**: oversight of every scheduled visit.
*   **Data Export**: Robust CSV/Excel export module for reports.

### 👨‍⚕️ Doctor Portal
*   **Personal Schedule**: Intuitive view of daily and weekly appointments.
*   **Patient EHR Access**: View comprehensive medical histories.
*   **Clinical Records**: Add diagnoses, prescriptions, and visit notes.

### 👤 Patient Portal
*   **Self-Service Booking**: Easy appointment scheduling.
*   **My Health Records**: Secure access to personal medical logs and doctor feedback.

---

## 🏗️ Architecture & Tech Stack

The solution is architected using **Clean Architecture** to ensure zero-dependency core logic:

- **🎨 WPF & MaterialDesignThemes**: Premium Dark/Light UI experience.
- **🧠 Domain Layer**: Pure POCO entities and core business rules.
- **⚙️ Application Layer**: Use-cases and high-level service logic.
- **🔌 Infrastructure Layer**: Persistence (JSON-based) and external integrations.
- **🗄️ Common Utilities**: Shared helpers (Password Hashing, MVVM Base).

---

## 🚀 Getting Started

### **1. Prerequisites**
*   Visual Studio 2022+ or Rider.
*   .NET 8.0 SDK.

### **2. Running the App**
1.  Clone the repository.
2.  Open `HMS.sln`.
3.  Set `HMS.Presentation` as the **Startup Project**.
4.  Press **F5**.

### **3. Test Credentials**

| Role | Username | Password |
| :--- | :--- | :--- |
| **Administrator** | `admin` | `admin` |
| **Receptionist** | `recep` | `admin` |
| **Doctor** | `smith` | `1234` |
| **Patient** | `tsedi` | `1234` |

---

## 📂 Folder Structure
```text
src/
├── HMS.Domain         # Core Entities & Enums
├── HMS.Application    # Business Logic & Services
├── HMS.Infrastructure # Data Access Implementation
├── HMS.Presentation   # WPF Views & ViewModels
├── HMS.Persistence    # Data Storage Layer
└── HMS.Common         # Shared Utilities
Data/                  # JSON Database Files
docs/                  # Architectural Documentation
```

---

## ✅ Recent Achievements
*   Implemented **Global Crash Protection** for high reliability.
*   Migrated from legacy WinForms to a modern, responsive **WPF UI**.
*   Standardized **Clean Architecture** separation of concerns.
*   Developed a premium, indigo-themed Sidebar Navigation engine.

---
*Created by the Alpha Clinic Development Team.*
