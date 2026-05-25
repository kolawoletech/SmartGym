# SmartGym - Build From Scratch (Student Walk-Through)

This guide shows you **how to construct the project yourself** in Visual Studio, file-by-file, so you understand *why* every folder and file exists. By the end you will have arrived at the same structure as the provided solution.

Steps marked **[Optional]** are not required for the project to work, but they earn extra marks or improve maintainability.

---

## Target Final Structure

```
Project/                                  <-- solution folder
├── SmartGym.sln                          <-- the solution file (auto-created by VS)
├── Database/
│   └── SmartGym_Schema.sql               <-- your CREATE DATABASE script
├── Docs/                       [Optional]
│   ├── Implementation_Guide.md
│   └── Build_From_Scratch.md             <-- this document
├── README.md                   [Optional]
└── SmartGym/                             <-- the Web Application project
    ├── SmartGym.csproj                   <-- project file (auto-created by VS)
    ├── Web.config                        <-- connection string + session
    ├── Global.asax (+ .cs)               <-- app-level events
    ├── Site.Master (+ .cs + .designer.cs)<-- shared layout / navigation
    ├── Content/
    │   └── site.css                      <-- stylesheet
    ├── Models/                           <-- POCO classes (one per table)
    │   ├── Member.cs
    │   ├── MembershipAccount.cs
    │   ├── FitnessClass.cs
    │   └── Transaction.cs
    ├── DAL/                              <-- ADO.NET data access layer
    │   ├── DatabaseHelper.cs             <-- shared SqlConnection / DataAdapter
    │   ├── MemberDAL.cs
    │   ├── AccountDAL.cs
    │   ├── ClassDAL.cs
    │   └── TransactionDAL.cs
    ├── BLL/                    [Optional but recommended]
    │   ├── MemberService.cs              <-- business rules
    │   └── BookingService.cs
    ├── Utilities/
    │   ├── PasswordHasher.cs             <-- SHA-256 hashing
    │   ├── FileLogger.cs                 <-- StreamWriter / StreamReader
    │   └── XmlReporter.cs                <-- DataSet.WriteXml + XmlWriter
    ├── Services/
    │   └── SmartGymService.asmx (+ .cs)  <-- SOAP web service
    ├── App_Data/                         <-- auto-created at runtime
    │   ├── Logs/                         <-- booking_log.txt, error_log.txt
    │   └── Reports/                      <-- generated XML files
    ├── Register.aspx (+ .cs + .designer.cs)
    ├── Login.aspx
    ├── ForgotPassword.aspx
    ├── Dashboard.aspx
    ├── BookClass.aspx
    ├── TopUp.aspx
    ├── Transactions.aspx
    ├── AdminClasses.aspx
    └── Logout.aspx
```

---

## Step 0 - Prerequisites (5 min)

1. Install **Visual Studio 2019 / 2022 / 2026 Community** (any edition).
2. In the VS Installer, tick the workload: **"ASP.NET and web development"**.
3. Ensure **SQL Server LocalDB** is installed (it comes with VS by default), OR SQL Server Express.
4. *(Optional)* Install **SSMS** (SQL Server Management Studio) for easier SQL editing.

---

## Step 1 - Create the Solution and the Web Project (3 min)

1. Open VS -> **Create a new project**.
2. Search for **"ASP.NET Web Application (.NET Framework)"** -> Next.
3. Configure:
   * **Project name:** `SmartGym`
   * **Location:** the folder you want the solution in
   * **Solution name:** `SmartGym` (default)
   * **Framework:** `.NET Framework 4.7.2`
4. Click **Create**.
5. On the next dialog choose **Empty** template, then **tick "Web Forms"**, untick "Configure for HTTPS" if you don't have a cert. Click **Create**.

What you have now:

```
Project/
├── SmartGym.sln
└── SmartGym/
    ├── SmartGym.csproj
    └── Web.config
```

> [Optional] At the solution level, create folders `Database/` and `Docs/` by
> right-clicking the **Solution** -> **Add -> New Solution Folder**. These are
> for organisation; they sit at the solution level, not in the SmartGym project.

---

## Step 2 - Add the Layered Folders Inside SmartGym (2 min)

In Solution Explorer, right-click the **SmartGym project** -> **Add -> New Folder**. Create each of these:

| Folder      | Purpose                                       | Required?    |
| ----------- | --------------------------------------------- | ------------ |
| `Models`    | One C# class per database table               | **Required** |
| `DAL`       | Data Access Layer (ADO.NET)                   | **Required** |
| `BLL`       | Business Logic Layer                          | [Optional]   |
| `Utilities` | Cross-cutting helpers (logging, hashing, xml) | **Required** |
| `Services`  | Web service (.asmx)                           | **Required** |
| `Content`   | CSS / images / static assets                  | **Required** |
| `App_Data`  | Runtime data + logs + xml reports             | Auto-creates |

> **Why layers?** It is much easier to mark, debug and reuse code when SQL,
> business rules and UI are in separate files. You could put all the code in
> the `.aspx.cs` files, but you would lose marks for messy architecture.

---

## Step 3 - Create the Database (10 min)

1. At the **solution** level, add a new **Solution Folder** called `Database`.
2. Right-click `Database` -> **Add -> New Item -> SQL Server -> SQL File**.
3. Name it `SmartGym_Schema.sql`.
4. Paste the schema (5 tables: `Members`, `MembershipAccounts`, `FitnessClasses`, `Transactions`, `BookingLogs`) and seed data.
5. In **SQL Server Object Explorer** (or SSMS), connect to `(localdb)\MSSQLLocalDB`.
6. Right-click the script -> **Execute**. Verify:

   ```sql
   USE SmartGymDB;
   SELECT * FROM Members;
   ```

> [Optional] Add an `.sql` script for stored procedures - distinction-level
> students should call SPs from ADO.NET with `CommandType.StoredProcedure`
> instead of inline SQL.

---

## Step 4 - Add the Connection String to Web.config (1 min)

Open `Web.config` and inside `<configuration>` add:

```xml
<connectionStrings>
  <add name="SmartGymDB"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;
                         Initial Catalog=SmartGymDB;
                         Integrated Security=True;"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Also add the **session timeout** and **forms auth login page**:

```xml
<system.web>
  <compilation debug="true" targetFramework="4.7.2" />
  <httpRuntime targetFramework="4.7.2" />
  <sessionState mode="InProc" timeout="30" />
  <authentication mode="Forms">
    <forms loginUrl="Login.aspx" defaultUrl="Dashboard.aspx" timeout="30" />
  </authentication>
</system.web>
```

---

## Step 5 - Build the Models (5 min)

Right-click the **Models** folder -> **Add -> Class**. Create:

* `Member.cs`
* `MembershipAccount.cs`
* `FitnessClass.cs`
* `Transaction.cs`

Each one is a plain POCO with `{ get; set; }` properties matching the
table columns. **Do not** put SQL or business rules in these files.

---

## Step 6 - Build the DAL (Data Access Layer) (20 min)

Right-click **DAL** -> **Add -> Class** for each:

1. **`DatabaseHelper.cs`** — `static` helper exposing `GetOpenConnection()`,
   `ExecuteNonQuery()`, `ExecuteScalar()`, `ExecuteDataTable()` and
   `ExecuteDataSet()`. This single file demonstrates **SqlConnection**,
   **SqlCommand**, **SqlDataAdapter** and **DataSet** in one place.

2. **`MemberDAL.cs`** — `RegisterMember`, `GetMemberByEmail`,
   `GetMemberById`, `UpdatePassword`, `DeleteMember`. Demonstrates
   **SqlDataReader** (for SELECT) and parameterised **INSERT / UPDATE / DELETE**.

3. **`AccountDAL.cs`** — `CreateAccount`, `GetAccountsForMember`,
   `GetAccountById`, `AdjustBalance`.

4. **`ClassDAL.cs`** — full CRUD + `GetClassesDataSet()` returning a `DataSet`
   to demonstrate XML export.

5. **`TransactionDAL.cs`** — `AddTransaction`, `AddBookingLog`,
   `GetTransactionsForMember` (DataTable for GridView),
   `GetTransactionsDataSet` (for XML).

> **Rule of thumb:** **every SQL string** lives in DAL, **never** in `.aspx.cs`.

---

## Step 7 - Build the Utilities (10 min)

Right-click **Utilities** -> **Add -> Class**:

1. **`PasswordHasher.cs`** — `Hash(string)` using `SHA256`.
   *(Distinction-level: replace with bcrypt or PBKDF2 + salt.)*

2. **`FileLogger.cs`** — `LogBooking()` appends to `App_Data/Logs/booking_log.txt`
   using **`StreamWriter`**, `ReadBookingLog()` uses **`StreamReader`**.

3. **`XmlReporter.cs`** — `ExportTransactionsToXml()` uses **`DataSet.WriteXml`**,
   `ExportClassScheduleToXml()` uses **`XmlWriter`** for manual XML,
   `ReadXmlIntoDataSet()` shows reading XML back in.

---

## Step 8 - Build the BLL [Optional but Recommended] (10 min)

Right-click **BLL** -> **Add -> Class**:

* `MemberService.cs` — Wraps MemberDAL with validation, hashing.
* `BookingService.cs` — Wraps Account + Class + Transaction DALs to enforce the booking workflow (deduct credits, write transaction, write booking log, write file log).

> If you skip this step, put the same logic inside your `.aspx.cs` files —
> but expect **lower marks for architecture**.

---

## Step 9 - Create the Master Page (5 min)

1. Right-click the **SmartGym project** -> **Add -> New Item -> Master Page**.
2. Name it `Site.Master`. Click **Add**.
3. VS auto-creates **three files**:
   * `Site.Master` (markup)
   * `Site.Master.cs` (code-behind)
   * `Site.Master.designer.cs` (auto-generated control fields)
4. Add a `<header>` with the brand, a navigation `<nav>` with `<asp:HyperLink>` for each page, and a `<asp:ContentPlaceHolder ID="MainContent" />`.
5. Link the stylesheet: `<link href="~/Content/site.css" rel="stylesheet" />`.

---

## Step 10 - Create the CSS (5 min)

Right-click **Content** -> **Add -> New Item -> Style Sheet** -> name `site.css`.
Paste a responsive design with `.card`, `.btn`, `.tile`, `.alert-*`, `.grid`
classes.

> [Optional] Use Bootstrap or Tailwind CDN instead of hand-rolled CSS.

---

## Step 11 - Add the Web Pages (30 min)

For each page, right-click the **SmartGym project** -> **Add -> New Item ->
Web Form with Master Page** -> pick `Site.Master` -> name it.

Create in this order so you can test each as you go:

| Order | File                  | Purpose                                  |
| ----- | --------------------- | ---------------------------------------- |
| 1     | `Register.aspx`       | New account form (INSERT)                |
| 2     | `Login.aspx`          | Session-based login                      |
| 3     | `ForgotPassword.aspx` | Security-question reset (UPDATE)         |
| 4     | `Dashboard.aspx`      | Tiles + GridView of accounts             |
| 5     | `BookClass.aspx`      | Book a class (BLL workflow)              |
| 6     | `TopUp.aspx`          | Add credits                              |
| 7     | `Transactions.aspx`   | History + XML export + file-log viewer   |
| 8     | `AdminClasses.aspx`   | GridView CRUD + XML schedule export      |
| 9     | `Logout.aspx`         | Session.Abandon                          |

Each page should:
* Add validators (`RequiredFieldValidator`, `RegularExpressionValidator`, etc.).
* In `Page_Load` redirect to `Login.aspx` if `Session["MemberId"] == null`.
* Call the BLL/DAL — **never** put SQL inline.

> VS auto-creates the matching `.aspx.cs` and `.aspx.designer.cs` files for
> each Web Form. If you ever rename a server control and the code-behind
> can't see it, right-click the `.aspx` -> **Convert to Web Application** to
> regenerate the designer file.

---

## Step 12 - Add the Web Service (10 min)

1. Right-click the **Services** folder -> **Add -> New Item -> Web Service (ASMX)**.
2. Name it `SmartGymService.asmx`.
3. Add `[WebMethod]` methods that reuse your BLL / DAL:
   * `Ping`
   * `AddMember`
   * `UpdateAccount`
   * `ProcessBooking`
   * `GetTransactions`
   * `GetClasses`
   * `LogBookingActivity`
   * `GetAccounts`
4. Right-click `SmartGymService.asmx` -> **View in Browser** to test.

---

## Step 13 - Global.asax (3 min)

1. Right-click the **SmartGym project** -> **Add -> New Item -> Global Application Class**.
2. In `Application_Start`, ensure `App_Data/Logs` and `App_Data/Reports` exist.
3. In `Application_Error`, log the exception via `FileLogger.LogError`.

---

## Step 14 - First Run (2 min)

1. Press **F5**. IIS Express starts.
2. The default page is **Login.aspx** (set via `<defaultDocument>` in Web.config).
3. Click **Register**, create a new account, log back in.

---

## Step 15 - [Optional] Solution Folders & Cleanup

Things you can do to score extra marks:

* Add a **`.gitignore`** to exclude `.vs/`, `bin/`, `obj/`, log files, etc.
* Add a **`README.md`** with screenshots, default users, and run instructions.
* Add a **`Docs/Implementation_Guide.md`** matching this file.
* Convert your inline SQL to **stored procedures**.
* Add **unit tests** (right-click solution -> Add -> New Project -> MSTest)
  targeting `BookingService` with a mocked DAL.
* Add a **Web.config transform** for production (`Web.Release.config`)
  that swaps the connection string for Azure SQL.

---

## Troubleshooting While Building

| Problem during build / run                              | Cause + Fix                                                          |
| -------------------------------------------------------- | -------------------------------------------------------------------- |
| `Could not load type 'SmartGym.Global'`                  | The project has not yet built. Build -> Rebuild Solution first.      |
| `The name 'ddlSomething' does not exist`                 | Designer file is missing. Right-click the `.aspx` -> Convert to Web Application. |
| `Cannot open database SmartGymDB`                        | Run `Database/SmartGym_Schema.sql` against your SQL instance.        |
| `Microsoft.WebApplication.targets not found`             | Install **ASP.NET and web development** workload via VS Installer.   |
| Booking log file not appearing                           | App_Data/Logs is auto-created by Global.asax - run the app once.     |
| GridView columns show old data                           | You forgot `BindGrid()` after Update/Delete event handlers.          |
| `WebForms UnobtrusiveValidationMode requires a ScriptResourceMapping for 'jquery'` | Add `<add key="ValidationSettings:UnobtrusiveValidationMode" value="None" />` inside `<appSettings>` in `Web.config`. |

---

## Marking Map (How Each Section Demonstrates a Requirement)

| Module requirement       | Where in your code                                              |
| ------------------------ | --------------------------------------------------------------- |
| ASP.NET Web Forms        | every `.aspx` file + `Site.Master`                              |
| SQL Server tables / FKs  | `Database/SmartGym_Schema.sql`                                  |
| ADO.NET SqlConnection    | `DAL/DatabaseHelper.cs:GetOpenConnection`                       |
| ADO.NET SqlCommand       | every DAL method                                                |
| ADO.NET SqlDataReader    | `MemberDAL.GetMemberByEmail`                                    |
| ADO.NET SqlDataAdapter   | `DatabaseHelper.ExecuteDataTable / ExecuteDataSet`              |
| ADO.NET DataSet          | `TransactionDAL.GetTransactionsDataSet`                         |
| SELECT example           | every `Get*` DAL method                                         |
| INSERT example           | `MemberDAL.RegisterMember`                                      |
| UPDATE example           | `AccountDAL.AdjustBalance`                                      |
| DELETE example           | `ClassDAL.DeleteClass`                                          |
| Web Service              | `Services/SmartGymService.asmx.cs` (8 WebMethods)               |
| StreamWriter (File I/O)  | `Utilities/FileLogger.LogBooking`                               |
| StreamReader (File I/O)  | `Utilities/FileLogger.ReadBookingLog`                           |
| Write XML                | `XmlReporter.ExportTransactionsToXml` and `ExportClassScheduleToXml` |
| Read XML into DataSet    | `XmlReporter.ReadXmlIntoDataSet`                                |
| Validation controls      | every `.aspx` (RequiredField / Range / Compare / RegularExpression) |
| Session authentication   | `Login.aspx.cs` + `Web.config <sessionState>`                   |
| Layered architecture     | `Models`, `DAL`, `BLL`, `Utilities`, `Services`, page layer     |
| Try / catch error handling | every `.aspx.cs` and BLL service                              |

---

End of walk-through. Building it in this order means at each step you can
press F5 and see something working - which is the best way to learn.
