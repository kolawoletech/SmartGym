# SmartGym - Step-by-Step Implementation Guide

This guide walks you through building, configuring, running and demonstrating every required feature of the SmartGym Membership Management System.

---

## 1. Prerequisites

| Tool                              | Version            |
| --------------------------------- | ------------------ |
| Visual Studio                     | 2019 or 2022 (Community works) |
| Workload                          | "ASP.NET and web development"  |
| .NET Framework                    | 4.7.2              |
| SQL Server                        | LocalDB / Express / Developer |
| SSMS or Azure Data Studio         | optional but useful |

---

## 2. Open the Solution

1. Launch Visual Studio.
2. **File -> Open -> Project/Solution** -> select `SmartGym.sln`.
3. Right-click the solution -> **Restore NuGet Packages** (none required, but harmless).
4. Set `SmartGym` as the **Startup Project**.

---

## 3. Create the Database

1. Open `Database/SmartGym_Schema.sql`.
2. In **SSMS** connect to your server (default LocalDB: `(localdb)\MSSQLLocalDB`).
3. Execute the script (F5). It will:
   * Drop existing `SmartGymDB` if present
   * Create five tables (Members, MembershipAccounts, FitnessClasses, Transactions, BookingLogs)
   * Insert seed members, accounts, classes and transactions.

```sql
USE SmartGymDB;
SELECT * FROM Members;
SELECT * FROM FitnessClasses;
```

---

## 4. Configure the Connection String

Edit `SmartGym/Web.config`:

```xml
<add name="SmartGymDB"
     connectionString="Data Source=(localdb)\MSSQLLocalDB;
                       Initial Catalog=SmartGymDB;
                       Integrated Security=True;"
     providerName="System.Data.SqlClient" />
```

If you use a different server change `Data Source=` accordingly (e.g. `.\SQLEXPRESS`).

---

## 5. Run the Application

1. Press **F5** (Debug) or **Ctrl+F5** (Run without debugging).
2. IIS Express starts. The default document is `Login.aspx`.
3. Register a new account via the **Register** link, then log in.

---

## 6. Feature Walkthrough (Marking Checklist)

### 6.1 Registration & Login (Validation + ADO.NET INSERT/SELECT)

* `Register.aspx` uses `RequiredFieldValidator`, `RegularExpressionValidator`, `CompareValidator`.
* `Register.aspx.cs` calls `MemberService.Register` which:
  1. Hashes the password (`PasswordHasher.Hash` - SHA-256).
  2. Calls `MemberDAL.RegisterMember` (parameterised `INSERT ... SELECT SCOPE_IDENTITY()`).
  3. Creates a default `MembershipAccount` via `AccountDAL.CreateAccount`.
* `Login.aspx.cs` uses `MemberService.Authenticate` -> `MemberDAL.GetMemberByEmail` (SqlDataReader).

### 6.2 Password Recovery (UPDATE)

`ForgotPassword.aspx` uses a `MultiView`:
1. Step 1: enter email, fetch security question (SELECT).
2. Step 2: answer + new password -> `MemberDAL.UpdatePassword` (UPDATE).

### 6.3 Dashboard

`Dashboard.aspx` shows tiles + a GridView of accounts (SqlDataReader),
and supports creating an additional account (INSERT).

### 6.4 Book Class (Business Logic + File I/O log)

`BookClass.aspx` -> `BookingService.BookClass`:
1. Validates credit balance.
2. `AccountDAL.AdjustBalance` (UPDATE).
3. `TransactionDAL.AddTransaction` (INSERT into Transactions).
4. `TransactionDAL.AddBookingLog` (INSERT into BookingLogs).
5. `FileLogger.LogBooking` -> appends to `App_Data/Logs/booking_log.txt`
   using **StreamWriter**.

### 6.5 Top Up (UPDATE)

`TopUp.aspx` -> `BookingService.TopUp` -> `AccountDAL.AdjustBalance(+amount)`
and inserts a `TopUp` row in Transactions.

### 6.6 Transactions (SqlDataAdapter + DataSet + XML + StreamReader)

`Transactions.aspx`:
* `TransactionDAL.GetTransactionsForMember` uses **SqlDataAdapter** to fill a **DataTable**.
* "Export to XML" -> `XmlReporter.ExportTransactionsToXml` uses `DataSet.WriteXml`.
* "View File Log" -> `FileLogger.ReadBookingLog` uses **StreamReader**.

### 6.7 Admin (Full CRUD + XmlWriter)

`AdminClasses.aspx` (visible only to users where `Session["Role"] == "Admin"`):
* GridView Add/Edit/Update/Delete -> `ClassDAL` INSERT/UPDATE/DELETE.
* "Export Schedule to XML" -> `XmlReporter.ExportClassScheduleToXml` uses an
  **XmlWriter** to manually build the XML document.

### 6.8 Web Service

Browse to `/Services/SmartGymService.asmx`. You will see all WebMethods:

| Method               | Purpose                                |
| -------------------- | -------------------------------------- |
| Ping                 | Connectivity test                      |
| AddMember            | Adds a new member                      |
| UpdateAccount        | Tops up an account                     |
| ProcessBooking       | Books a class                          |
| GetTransactions      | Returns DataSet of transactions        |
| GetClasses           | Returns list of fitness classes        |
| LogBookingActivity   | Appends to booking log file            |
| GetAccounts          | Returns a member's accounts            |

Each method demonstrates the standard `[WebMethod]` attribute and reuses
the layered architecture (BLL -> DAL).

### 6.9 Logout (Session)

`Logout.aspx.cs` calls `Session.Clear()`, `Session.Abandon()` and expires
the `ASP.NET_SessionId` cookie.

---

## 7. ADO.NET Examples Summary

| Operation     | Where in code                                       |
| ------------- | --------------------------------------------------- |
| SqlConnection | `DatabaseHelper.GetOpenConnection`                  |
| SqlCommand    | every DAL method                                    |
| SqlDataReader | `MemberDAL.GetMemberByEmail`, `AccountDAL.Get*`     |
| SqlDataAdapter| `DatabaseHelper.ExecuteDataTable / ExecuteDataSet`  |
| DataSet       | `TransactionDAL.GetTransactionsDataSet`             |
| INSERT        | `MemberDAL.RegisterMember`                          |
| UPDATE        | `AccountDAL.AdjustBalance`                          |
| DELETE        | `ClassDAL.DeleteClass`                              |
| SELECT        | almost every DAL method                             |

---

## 8. XML Examples Summary

| Concept             | Where in code                                     |
| ------------------- | ------------------------------------------------- |
| Write XML (DataSet) | `XmlReporter.ExportTransactionsToXml`             |
| Write XML (manual)  | `XmlReporter.ExportClassScheduleToXml` (XmlWriter)|
| Read XML            | `XmlReporter.ReadXmlIntoDataSet`                  |

---

## 9. File I/O Examples Summary

| Concept       | Where in code                       |
| ------------- | ----------------------------------- |
| StreamWriter  | `FileLogger.LogBooking / LogError`  |
| StreamReader  | `FileLogger.ReadBookingLog`         |
| Log file path | `App_Data/Logs/booking_log.txt`     |

---

## 10. Troubleshooting

| Problem                                  | Fix                                                    |
| ---------------------------------------- | ------------------------------------------------------ |
| "Cannot open database SmartGymDB"        | Re-run `Database/SmartGym_Schema.sql`                 |
| Login always fails for seeded users      | Use `Register.aspx` to create new accounts             |
| Web service page is blank                | Right-click `SmartGymService.asmx` -> View in Browser  |
| File log not appearing                   | Make sure `App_Data/Logs` exists (Global.asax creates it) |

---

## 11. Suggested Enhancements (Distinction Level)

1. **Bcrypt / PBKDF2 + per-user salt** instead of plain SHA-256.
2. **Stored procedures** instead of inline SQL (with `CommandType.StoredProcedure`).
3. **Repository + Unit-of-Work pattern** with dependency injection (e.g. Unity, Autofac).
4. **Anti-CSRF tokens** on all POST actions.
5. **Logging framework** (NLog or Serilog) instead of custom FileLogger.
6. **Email notifications** (SmtpClient) when a class is booked.
7. **AJAX / UpdatePanel** for live credit refresh on Dashboard.
8. **Charts** of monthly booking volume with `Chart.js` driven by the DataSet/XML feed.
9. **Role-based authorization** with location/path config blocks in `Web.config`.
10. **JSON Web Service** alongside the SOAP one (WCF or WebAPI) for modern clients.
11. **Unit tests** for `BookingService` using MSTest/NUnit with a mocked DAL.
12. **Deployment to Azure App Service + Azure SQL** with Web.config transforms.

---

End of guide. Good luck with the assessment.
