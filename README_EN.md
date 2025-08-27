# 🧾 Pojišťák.NET – Insurance Records in ASP.NET

This project was created as the final assignment for a retraining course at ITnetwork.

The original version was functional but conceptually incomplete – so I decided to revisit and improve it  
to better reflect my skills and serve as a portfolio piece for applying to junior IT positions.

---

## 🧩 About the Project

**Pojišťák.NET** is a simple web application for managing insurance clients and their policies.  
It is built with ASP.NET Core MVC, using ASP.NET Identity for user authentication and Entity Framework Core as the ORM.

The application demonstrates core concepts such as authentication, role-based authorization, entity relationships,  
and the separation of public and authenticated areas of the website.

---

## ✅ Project Status

The project is **considered complete** and is **not in active development** at the moment.

All key features have been implemented to meet the expectations for a junior-level .NET developer.  
There is still room for further enhancements, such as:

- improved XSS protection,  
- user profile management,  
- CI/CD pipeline and audit logging.

These features were left out due to prioritization of stability, time constraints, and technical limitations.  
However, the project remains open for future improvements if needed or desired.

---

## ✨ Implemented Features

- ASP.NET Identity – registration, login, role management (`User`, `Admin`, `SuperAdmin`)
- Creating articles and managing accounts, including admins.
- Basic dashboard with role-based access control
- CRUD operations for managing insured individuals and their insurance records
- Logging and basic audit tracking (login events, data changes)
- Form validation
- Clear separation of public vs. protected areas
- Dependency Injection, `DbContext` registration, and basic testing

---

## 🟡 Potential Improvements (Non-blocking)

- Paging support for large datasets  
- Additional XSS protection beyond built-in mechanisms  
- Profile editing for standard users  
- CI/CD integration, two-factor authentication, responsive design  
- Improved form helpers and UX refinements

---

## 🧠 Summary

This project demonstrates practical skills in ASP.NET Core web development.  
It meets the expectations for a junior developer portfolio and is ready for presentation to recruiters or hiring managers.

> 🎯 The goal was not perfection, but a clean, functional, and extensible project that shows real-world abilities – not just a template assignment.


## How to run the project

For running the project, I recommend using advanced editors such as Visual Studio Community or JetBrains Rider.  
Alternatively, you can use Visual Studio Code with the C# Dev Kit extension installed, which sets up the .NET SDK including the dotnet tool.

1. Clone the repository  
   `git clone https://github.com/danixek/PojistakNET.git`  
   `cd PojistakNET`
2. Verify the database connection string in `appsettings.json`  
   (skip this step if you use the default settings)
3. Build the project:  
   `dotnet build`  
   This will check the project structure and automatically download NuGet dependencies.
4. Run database migrations:
   ```bash příkazy
   dotnet ef database update --context ApplicationDbContext  
   dotnet ef database update --context InsuranceContext
5. Run the project:  
   `dotnet run`
   
> 💡 **Note:** If the `dotnet ef` command fails, you may need to install the EF CLI tool:  
`dotnet tool install --global dotnet-ef`

After successful startup, the console will display an address (e.g., https://localhost:5001).  
Open it in your browser — the project should be accessible.  
In Visual Studio Community or Rider, the app usually launches automatically with the browser opening.
