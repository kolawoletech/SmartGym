# SmartGym Membership Management System

A complete ASP.NET Web Forms application for an Advanced C# Programming (Level 5) module.

## Technologies

| Concept            | Where it lives                                                      |
| ------------------ | ------------------------------------------------------------------- |
| ASP.NET Web Forms  | `*.aspx` + `Site.Master`                                            |
| SQL Server         | `Database/SmartGym_Schema.sql`                                      |
| ADO.NET            | `SmartGym/DAL/*` (SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter, DataSet) |
| Web Services       | `SmartGym/Services/SmartGymService.asmx`                            |
| XML                | `SmartGym/Utilities/XmlReporter.cs`                                 |
| File I/O           | `SmartGym/Utilities/FileLogger.cs` (StreamWriter / StreamReader)    |
| Session Auth       | `Web.config` + `Site.Master.cs` + each `*.aspx.cs` Page_Load        |

## Project Structure

```
Project/
├── SmartGym.sln
├── Database/
│   └── SmartGym_Schema.sql           # CREATE DATABASE + tables + seed data
├── SmartGym/
│   ├── SmartGym.csproj
│   ├── Web.config                    # Connection string + session config
│   ├── Global.asax + .cs             # App start + global error log
│   ├── Site.Master + .cs             # Shared layout / nav
│   ├── Content/site.css              # Responsive professional UI
│   ├── Models/                       # POCO domain entities
│   ├── DAL/                          # ADO.NET data access layer
│   │   ├── DatabaseHelper.cs         # Central SqlConnection / DataAdapter / DataSet
│   │   ├── MemberDAL.cs
│   │   ├── AccountDAL.cs
│   │   ├── ClassDAL.cs
│   │   └── TransactionDAL.cs
│   ├── BLL/                          # Business logic layer
│   │   ├── MemberService.cs
│   │   └── BookingService.cs
│   ├── Utilities/
│   │   ├── PasswordHasher.cs         # SHA-256 hashing
│   │   ├── FileLogger.cs             # StreamWriter / StreamReader logging
│   │   └── XmlReporter.cs            # DataSet.WriteXml + XmlWriter + ReadXml
│   ├── Services/
│   │   └── SmartGymService.asmx + .cs  # SOAP Web Service
│   ├── App_Data/
│   │   ├── Logs/                     # booking_log.txt + error_log.txt
│   │   └── Reports/                  # generated XML reports
│   ├── Register.aspx        Login.aspx        ForgotPassword.aspx
│   ├── Dashboard.aspx       BookClass.aspx    TopUp.aspx
│   ├── Transactions.aspx    AdminClasses.aspx Logout.aspx
└── Docs/
    └── Implementation_Guide.md       # Step-by-step build/run guide
```

## Quick Start

1. Open `SmartGym.sln` in Visual Studio 2019 / 2022.
2. In SQL Server Management Studio (or Azure Data Studio) execute
   `Database/SmartGym_Schema.sql` against your SQL Server / LocalDB instance.
3. Verify the connection string in `SmartGym/Web.config` matches your server.
4. Press **F5** (IIS Express) - the browser opens at `Login.aspx`.

## Default Accounts

| Role  | Email                  | Password   | Note                                 |
| ----- | ---------------------- | ---------- | ------------------------------------ |
| Admin | admin@smartgym.local   | admin      | Has access to `AdminClasses.aspx`    |
| User  | jane@example.com       | Password1  | Has two seeded accounts              |
| User  | john@example.com       | Password1  | Premium account, 500 credits         |

> The seeded hashes in the SQL script are illustrative placeholders.
> The recommended approach for a fresh database is to **register** new accounts
> through `Register.aspx` so they are hashed correctly by the running
> application (`SHA256` via `PasswordHasher`).

## Pages and What They Demonstrate

| Page                  | Concepts                                                         |
| --------------------- | ---------------------------------------------------------------- |
| Register.aspx         | Validation controls, INSERT via ADO.NET, password hashing        |
| Login.aspx            | SELECT via SqlDataReader, Session-based authentication           |
| ForgotPassword.aspx   | MultiView, UPDATE via ADO.NET                                    |
| Dashboard.aspx        | SELECT to GridView, dynamic tiles, INSERT account                |
| BookClass.aspx        | BLL workflow, credit deduction, file logging                     |
| TopUp.aspx            | UPDATE statement, transaction insert                             |
| Transactions.aspx     | SqlDataAdapter -> DataTable, DataSet -> XML export, StreamReader |
| AdminClasses.aspx     | Full CRUD on GridView (INSERT/UPDATE/DELETE), XmlWriter export   |
| Logout.aspx           | Session.Abandon + cookie expiry                                  |
| SmartGymService.asmx  | 7 WebMethods: Ping, AddMember, UpdateAccount, ProcessBooking,    |
|                       | GetTransactions, GetClasses, LogBookingActivity, GetAccounts     |

See **Docs/Implementation_Guide.md** for the full step-by-step build.
