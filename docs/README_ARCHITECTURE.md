# 🏥 HMS.Core: Professional Hospital Management System

This project has been redesigned using **Clean Architecture** principles to ensure scalability, maintainability, and a clear separation of concerns.

---

## 🏗️ 1. Project Structure Explained

The system is split into **6 independent projects** in the `src/` directory.

### 🏢 **HMS.Presentation (UI Layer)**
*   **Role**: Handles user interaction (WPF Windows & Pages).
*   **Pattern**: **MVVM** (Model-View-ViewModel).
*   **Contents**: XAML Views, ViewModels, UI Services (Navigation).
*   **Startup**: This is the "Entry Point" project.

### ⚙️ **HMS.Application (Business Layer)**
*   **Role**: Contains "Use Cases" (e.g., Registering a Patient, Booking an Appointment).
*   **Contents**: High-level logic services (`DataManager`, `SessionManager`).
*   **Isolation**: It doesn't know about UI or Databases—it only knows the Domain.

### 🧠 **HMS.Domain (Core entities)**
*   **Role**: The heart of the project. Contains business models (Patient, Doctor, etc.) and Enums.
*   **Contents**: Entities, Core Interfaces (`IRepository`), and Enums (`UserRole`).
*   **Rule**: It has **ZERO** dependencies on other projects.

### 🗄️ **HMS.Persistence (Data Access Implementation)**
*   **Role**: Implementation of data storage.
*   **Contents**: Current JSON persistence logic. In the future, this is where **Entity Framework Core (EF Core)** and SQL Server context will live.

### 🔌 **HMS.Infrastructure (External integrations)**
*   **Role**: Repositories and external services (Email, Auth, Backup).
*   **Contents**: `AuthService`, `BackupService`, `JsonDataService`.

### 🧰 **HMS.Common (Shared Utilities)**
*   **Role**: Reusable helper code for all projects.
*   **Contents**: `PasswordHasher`, `ObservableObject` (Base for MVVM), `RelayCommand`.

---

## 🚀 2. How to Run & Build

### **Using Visual Studio (Recommended)**
1.  Open **`HMS.sln`** in Visual Studio 2022 or newer.
2.  Right-click on **`HMS.Presentation`** in the Solution Explorer and select **"Set as Startup Project"**.
3.  Press **F5** (or click the green Start button).
4.  **Login**: Use the default administrator credentials (seeding):
    *   **Username**: `admin`
    *   **Password**: `Admin@123`

### **Using Command Line**
```powershell
dotnet restore
dotnet build
dotnet run --project src\HMS.Presentation\HMS.Presentation.csproj
```

---

## 🛠️ 3. How to Work (Development Workflow)

### **Adding a New Page (e.g., Pharmacy)**
1.  **Domain**: Create a `Medicine.cs` entity in `HMS.Domain/Entities`.
2.  **Infrastructure**: Add repository methods for Medicine in `HMS.Infrastructure/Repositories/Json/JsonDataService.cs`.
3.  **Application**: Create a `PharmacyService` in `HMS.Application`.
4.  **Presentation (ViewModel)**: Create `PharmacyViewModel.cs`.
5.  **Presentation (View)**: Create `PharmacyView.xaml` and bind it to the ViewModel.

---

## 🔄 4. How to Update Data

The application currently uses **JSON-based** storage for development.
*   **Location**: All data is stored in the **`Data/`** folder at the root.
*   **Updating**: You can manually edit the `.json` files (e.g., `patients.json`) to add initial test data, OR use the **BackupService** to restore data.

---

## 📜 5. Clean Architecture Rule of Thumb
**Always remember the Dependency Rule:**
Inner layers (Domain, Application) should **NEVER** know anything about outer layers (Presentation, Infrastructure, Persistence).

*   ✅ `Presentation` can talk to `Application`.
*   ✅ `Application` can talk to `Domain`.
*   ❌ `Domain` should **NEVER** depend on `Presentation` or `Infrastructure`.

---

## ✅ Summary of Achievement
*   **Modernized**: Converted to .NET SDK format.
*   **Organized**: Grouped all codes into professional Clean Architecture layers.
*   **Cleaned**: Removed redundant WinForms and legacy "ClinicPatient" naming.
*   **Enterprise-Ready**: Scalable structure ready for SQL Server migration.
