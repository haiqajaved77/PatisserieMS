# DEPLOYMENT GUIDE
## La Patisserie Management System
## CS-412 Visual Programming

---

## System Requirements
- OS: Windows 10 / 11 (64-bit)
- .NET Runtime: .NET 10.0 Windows Desktop Runtime
- RAM: 256 MB minimum
- Disk Space: 50 MB

---

## Installation Steps

### Step 1 - Install .NET 10 Runtime
Download and install from:
https://dotnet.microsoft.com/download/dotnet/10.0

### Step 2 - Open Project
1. Open **PatisserieMS.csproj** in Visual Studio 2022
2. NuGet will auto-restore **Microsoft.Data.Sqlite**
3. Press **F5** to run

### Step 3 - First Run
On first launch the application will:
- Create **patisserie.db** automatically
- Seed all 105 menu items automatically
- No manual database setup required

---

## Login Credentials
| Role     | Username | Password     |
|----------|----------|--------------|
| Admin    | admin    | admin123     |
| Customer | customer | customer123  |

---

## Features

### Admin Access
- View Menu (105 Pakistani items with PKR prices)
- Place Order + Auto Bill Generation
- Daily Sales Report

### Customer Access
- View Menu
- Place Order + Auto Bill Generation

---

## Database Information
- Database: SQLite
- File: patisserie.db
- Auto-created on first run
- Location: bin/Debug/net10.0-windows/
- Tables: MenuItems, Orders, OrderItems, DailySales

---

## Technology Stack
| Component | Technology |
|-----------|-----------|
| Language | C# (.NET 10) |
| UI Framework | Windows Forms |
| Database | SQLite |
| Data Access | Microsoft.Data.Sqlite |
| IDE | Visual Studio 2022 |

---

## Project Structure
```
PatisserieMS/
├── Database/
│   └── DatabaseHelper.cs
├── Forms/
│   ├── LoginForm.cs
│   ├── MainForm.cs
│   ├── MenuViewForm.cs
│   ├── OrderForm.cs
│   ├── BillForm.cs
│   └── DailySalesForm.cs
├── Models/
│   └── Models.cs
├── Program.cs
├── PatisserieMS.csproj
└── README.md
```

---

## Troubleshooting
| Issue | Solution |
|-------|----------|
| App won't start | Install .NET 10 Runtime |
| Database error | Delete patisserie.db and restart |
| Build errors | Run dotnet restore in project folder |
