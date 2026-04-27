# 🏥 Hospital Management System (HMS) - Project Documentation

This document outlines the architecture, actors, and current implementation status of the Hospital Management System built with **C# WPF**, **Entity Framework Core**, and **SQL Server**.

---

## 🏗️ System Architecture Highlights

*   **Presentation Layer**: Built with **WPF** using **MaterialDesignInXaml** for a modern, responsive, and professional UI. Follows the MVVM (Model-View-ViewModel) pattern.
*   **Application Layer**: Uses a centralized `DataManager` for in-memory caching and data synchronization to ensure high performance and data freshness across different departmental views.
*   **Persistence Layer**: Entity Framework Core (`HMSDbContext`) with a generic Unit of Work / Repository pattern for database operations and automatic migrations.

---

## 👥 Actors & Implementation Status

### 1. 👨‍⚕️ Doctor (🟢 Largely Implemented)
**Role:** Provides medical care and drives clinical workflows.
**Key Functionalities:**
*   [x] View patient records & medical history.
*   [x] Request lab tests.
*   [x] View detailed test results (numerical values, reference ranges, clinical notes).
*   [x] Create detailed prescriptions (medicines, dosages, durations).
*   [ ] Manage appointments (Pending polish).
*   [ ] Write discharge summaries.

### 2. 💊 Pharmacist (🟢 Highly Developed)
**Role:** Manages medications, inventory safety, and dispensing workflows.
**Key Functionalities:**
*   [x] View prescriptions issued by doctors (with medicine summaries).
*   [x] Professional Dispensing Workflow: Real-time inventory validation against prescribed quantities.
*   [x] Clinical Counseling: Add pharmacist-specific instructions to dispensed items.
*   [x] Cross-Department Integration: Automatically generate billing records (`Bill` & `BillItem`) upon dispensing.
*   [x] Inventory Safety: Dashboard alerts for **Expired** and **Expiring Soon** (30 days) medications.

### 3. 🧪 Lab Technician (🟢 Implemented)
**Role:** Handles medical tests and diagnostic reporting.
**Key Functionalities:**
*   [x] Receive and view pending test requests from doctors.
*   [x] Conduct tests and upload detailed results (Actual values, Reference Ranges, Comments).
*   [x] Maintain lab records.

### 4. 👩‍⚕️ Nurse (🟡 Partially Implemented)
**Role:** Assists doctors and monitors patient status.
**Key Functionalities:**
*   [x] Monitor and record patient vitals (BP, temperature, heart rate).
*   [x] Persistent vitals tracking.
*   [ ] Administer medications (track administration vs. dispensing).
*   [ ] Manage patient admissions/discharges.

### 5. 💰 Billing/Accounts Staff (🟡 Partially Implemented)
**Role:** Handles financial operations and patient invoicing.
**Key Functionalities:**
*   [x] Receive automated bill generation from Pharmacy actions.
*   [x] Track unit prices and total costs.
*   [ ] Process payments and generate receipts.
*   [ ] Manage insurance claims.

### 6. 🧑‍💼 Receptionist / Front Desk (🟡 Partially Implemented)
**Role:** First point of contact and patient management.
**Key Functionalities:**
*   [x] Register new patients.
*   [ ] Handle patient check-in/check-out.
*   [ ] Manage queues and schedule appointments.

### 7. 🧾 Administrator (Admin) (⚪ Pending)
**Role:** System controller and manager.
**Key Functionalities:**
*   [x] Basic identity/auth flow.
*   [ ] Manage users (doctors, nurses, staff) and permissions.
*   [ ] Monitor system usage and audit logs.

### 8. 👤 Patient (🟢 Fully Integrated)
**Role:** The center of the healthcare system, empowered with self-service tools.
**Key Functionalities:**
*   [x] **Intelligent Dashboard**: Real-time statistics on pending medications, unpaid bills, and upcoming clinical events.
*   [x] **Combined Activity Feed**: Unified timeline of diagnoses, test results, and prescriptions.
*   [x] **Smart Booking**: Real-world appointment scheduling with doctor specialty filtering and slot validation.
*   [x] **Medication Tracking**: Live status updates for prescribed vs. dispensed medicines.
*   [x] **Lab Results Portal**: Instant access to diagnostic findings, reference ranges, and clinical notes.
*   [x] **Financial Management**: Integrated billing view with simulated payment processing to settle clinic invoices.

---

## 🧩 Core Modules Integration Flow

The system is designed to have interconnected modules rather than isolated silos. 
**Current Active Flow Example:**
1.  **Doctor** prescribes medication to a Patient.
2.  **Pharmacist** views the pending prescription.
3.  Pharmacist reviews real-time stock levels.
4.  Pharmacist dispenses the medicine, which:
    *   Deducts stock from the **Inventory Module**.
    *   Updates the **Prescription Module** status.
    *   Automatically creates a pending charge in the **Billing Module**.

---

## 📋 To-Do / Pending Tasks (Next Steps)

*   **Robust Registration**: Improve patient registration to handle edge cases (e.g., prompting for manual entry if email is missing).
*   **Audit Logging**: Verify that the `AuditLogs` table correctly captures critical actions like "DispensedBy" to complete the accountability loop.
*   **Billing UI Sync**: Ensure the Billing portal live-updates when new bills are generated by the pharmacy.
*   **UI Polish**: Further enhance data grids (e.g., color-coding expired items in red within the inventory view).
*   **Reporting**: Build out the analytics dashboard for hospital management.
