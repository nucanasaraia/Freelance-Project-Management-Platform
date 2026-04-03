Freelance Project Management Platform

A production-style backend API built with ASP.NET Core that simulates a freelance marketplace. It allows clients to create and manage projects, freelancers to work on them, and administrators to control the platform.

🚀 Features
🔐 JWT Authentication & Role-Based Authorization
👥 Multiple user roles (Admin, Client, Freelancer)
📁 Project creation and management
🧾 Task/workflow handling between clients and freelancers
⚠️ Global error handling and logging
📦 Standardized API responses
🧩 Clean and scalable architecture
🧪 Unit tested with xUnit and Moq

👤 User Roles
Admin
Full system access
Manage users and platform data
Client
Create and manage projects
Assign work to freelancers
Freelancer
View assigned projects
Work on tasks and submit progress

🛠️ Tech Stack
Backend: ASP.NET Core Web API
Database: SQL Server + Entity Framework Core
Authentication: JWT (JSON Web Tokens)
Architecture: Layered (Controllers → Services → Data Access)

📚 Libraries & Tools
AutoMapper
Built-in logging (ILogger)
Custom API response factory
Exception handling patterns
Serilog (structured logging)
FluentValidation
Rate Limiting
HttpOnly Cookie refresh tokens
User Secrets (credential management)

🧠 Architecture Overview

The project follows a clean layered structure:

Controllers → Handle HTTP requests and responses
Services → Contain business logic
Data Access Layer → Manages database operations (EF Core)
DTOs → Transfer data safely between layers

⚙️ Getting Started
1. Clone the repository
git clone https://github.com/your-username/freelance-project-management-platform.git
cd freelance-project-management-platform
2. Configure database

Update your appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "your_connection_string_here"
}
3. Set user secrets
dotnet user-secrets set "SMTP:SenderEmail" "your_email@gmail.com"
dotnet user-secrets set "SMTP:AppPassword" "your_app_password"
4. Apply migrations
dotnet ef database update 
5. Run the project
dotnet run

📬 API Documentation

After running the project, open:

https://localhost:{port}/swagger

Use Swagger UI to explore and test all endpoints.

📸 Example Endpoints
Auth
Register
Login
Projects
Create project
Get all projects
Update project
Users
Role-based access endpoints

🧪 Future Improvements
Implement real-time notifications (SignalR)
Add file upload support
Add integration tests
📌 Project Status

✅ Fully functional backend API
✅ Production-style architecture

💡 Notes

This project focuses on backend development best practices such as clean architecture, secure authentication, and maintainable code structure. It is designed to reflect real-world application development.