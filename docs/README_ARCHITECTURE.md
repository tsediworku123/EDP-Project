# 🏥 HMS.Core: Technical Architecture & Developer Guide

This document provides a comprehensive deep dive into the engineering principles, design patterns, and technologies that power the **Hospital Management System (HMS)**.

---

## 🏗️ 1. The Technology Stack

| Technology | Purpose | Key Benefits |
| :--- | :--- | :--- |
| **WPF (.NET 8)** | **The UI Engine** | Uses **XAML** for the visual layout and **C#** for the logic. Supports advanced styling, animations, and data-binding. |
| **Material Design XAML** | Styling Engine | Google's Material Design language for a premium, modern feel. |
| **Clean Architecture** | Core Design | Ensures scalability by decoupling business logic from the UI. |
| **MVVM** | UI Pattern | Clean separation between logic (ViewModel) and layout (View). |
| **JSON Persistence** | Data Storage | Lightweight, file-based storage for rapid development and portability. |

---

## 🎨 2. WPF Fundamentals: XAML vs C#

Windows Presentation Foundation (WPF) works on a "Dual-File" principle:

1.  **XAML (`.xaml`) - The "Painter"**:
    *   **Role**: Declarative UI design.
    *   **Focus**: Controls (Buttons, TextBoxes), Layout (Grids, StackPanels), and Styling (Colors, Shadows).
    *   **Like**: HTML/CSS for desktop.

2.  **C# Code-Behind (`.xaml.cs`) - The "Mechanic"**:
    *   **Role**: Handles initialization of the UI and links it to the logic.
    *   **Note**: In a good MVVM project, this file is kept as small as possible.

---

## 🔄 3. Understanding MVVM (Model-View-ViewModel)

MVVM is the backbone of WPF development. It ensures that your **Logic** and **UI** are separate, making the code testable and organized.

### **The Three Pillars:**

1.  **MODEL (`HMS.Domain`)**:  
    *   **What it is**: Pure data objects (Entities).  
    *   **Example**: `Patient.cs`, `Appointment.cs`.  
    *   **Note**: It has no logic and doesn't know about the UI.

2.  **VIEW (`HMS.Presentation/Views`)**:  
    *   **What it is**: The XAML file. It defines the "Look."  
    *   **Example**: `AdminReportsView.xaml`.  
    *   **Logic**: Contains **zero** C# code-behind (except for `InitializeComponent`).

3.  **VIEWMODEL (`HMS.Presentation/ViewModels`)**:  
    *   **What it is**: The "Brain" of the View.  
    *   **Role**: It fetches data from services and prepares it for the View.  
    *   **Communication**: It uses **Data Binding** and **Commands**.

---

## 📡 4. How it Works Together (Key Mechanisms)

### **A. Data Binding (Displaying Data)**
The View "binds" to properties in the ViewModel. When a property in the ViewModel changes, it calls `OnPropertyChanged()`, and the UI automatically updates.
*   **ViewModel**: `public string TotalRevenue { get; set; }`
*   **View**: `<TextBlock Text="{Binding TotalRevenue}" />`

### **B. Commands (Handling Actions)**
Instead of "Event Handlers" (like `btn_Click`), we use **Commands**.
*   **ViewModel**: `public ICommand ExportCommand => new RelayCommand(ExecuteExport);`
*   **View**: `<Button Command="{Binding ExportCommand}" />`

### **C. Dependency Injection & Services**
The ViewModel doesn't talk directly to the JSON files. It uses **Services** (like `DataManager`) to handle data flow, keeping the VM clean.

---

## 📂 5. Folder Structure Deep-Dive

```
📁 src/
├── 🏢 HMS.Presentation         # XAML Windows, Pages, ViewModels, Resources.
├── ⚙️ HMS.Application          # Use Cases and Logic Managers (DataManager).
├── 🧠 HMS.Domain               # Core Entities (Patient, Doctor, Appointment).
├── 🗄️ HMS.Persistence          # Repository Implementations (JSON saving).
├── 🔌 HMS.Infrastructure       # External services (Auth, Backup).
└── 🧰 HMS.Common               # Base classes (ObservableObject, RelayCommand).

📁 Data/                        # The local database (JSON files).
```

---

## 🛠️ 6. Developer Workflow: Adding a Feature

To add a new module (e.g., "Pharmacy"):

1.  **Define Domain**: Add `Medicine.cs` to `HMS.Domain`.
2.  **Update DataService**: Add `Medicines` list to `JsonDataService` and handle saving/loading.
3.  **Create Service**: (Optional) Create `PharmacyService` in `HMS.Application` for complex logic.
4.  **Create ViewModel**: Create `PharmacyViewModel` in `HMS.Presentation/ViewModels`. Define your properties and `RelayCommands`.
5.  **Design View**: Create `PharmacyView.xaml` in `HMS.Presentation/Views`. Use Material Design components and bind attributes to the ViewModel.
6.  **Wire Up**: Register the new View in your `AdminShell` navigation logic.

---

## ⚡ 7. Anatomy of an Action: "Creating & Displaying a User"

Let’s trace the full lifecycle of adding a new staff member to the system:

### **Step 1: The Input (The View)**
An Administrator opens the "Add User" dialog and enters a username and role. 
-   **Binding**: The input fields (TextBoxes) are bound to properties in the `AdminUsersViewModel`.
-   **Action**: When "Save" is clicked, it triggers the `AddUserCommand`.

### **Step 2: The Logic (The ViewModel)**
The `AdminUsersViewModel` receives the data.
-   **Blueprint (The Domain)**: It uses the `User` entity from **`HMS.Domain`** as the model for the new account.
-   **Validation**: It checks if the username exists or if fields are empty.
-   **Creation**: It creates a new `User` object: `new User { Username = "Dr.Smith", Role = "Doctor" }`.
-   **Addition**: It adds this object to the global data: `DataManager.Users.Add(newUser)`.

### **Step 3: The Persistence (The Infrastructure)**
The system ensures the data is saved permanently.
-   **Call**: The ViewModel calls `DataManager.SaveUsers()`.
-   **File Write**: The `JsonDataService.cs` serializes the user list into JSON and overwrites **`Data/users.json`**.

### **Step 4: The Ripple Effect (Notification)**
Now that the data is saved, the UI needs to know.
-   **Re-filtering**: The ViewModel calls `ApplyFilters()`.
-   **Signal**: The `FilteredUsers` collection is refreshed. Because it is an `ObservableCollection`, it automatically raises a notification to WPF.

### **Step 5: The UI Refresh (The Bound Grid)**
The `AdminUsersView.xaml` responds to the data change.
-   **Update**: The `DataGrid` detects the new item in its `ItemsSource`.
-   **Display**: A new row appears instantly in the "User Accounts" table with the correct status badge and action buttons. 
-   **Total Time**: The user sees the new account appear in milliseconds.

---

## 🌟 Quality Standards
*   **Aesthetics First**: Every new screen must use harmonious color palettes and clean spacing.
*   **Code Cleanliness**: No "Spaghetti code" in code-behind. Keep logic in ViewModels.
*   **Separation**: Ensure `HMS.Domain` never references `HMS.Presentation`.
