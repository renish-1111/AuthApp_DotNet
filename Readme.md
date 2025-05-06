# AuthApp - .NET Authentication Application

A comprehensive authentication application built with .NET, featuring user management, email verification, and secure authentication practices.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Setup and Installation](#setup-and-installation)
- [Configuration](#configuration)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)

## ✨ Features

- User registration and authentication
- Email verification
- Password reset functionality
- Secure token-based authentication


## 💻 Tech Stack

- **Backend**: ASP.NET Core
- **Database**: SQL Server Express
- **Frontend**: (Likely a JavaScript framework running on port 5173)
- **Email Service**: SMTP via Gmail

## 🚀 Setup and Installation

1. Clone the repository
2. Ensure you have .NET SDK installed
3. Ensure SQL Server Express is installed and running
4. Update the connection string in `appsettings.json` if needed
5. Run database migrations: `dotnet ef database update`
6. Start the backend: `dotnet run`
7. Start the frontend (separate instructions)

## ⚙️ Configuration

The application uses the following configuration settings:

```json
{
    "FrontendUrl": "http://localhost:5173",
    "ConnectionStrings": {
        "DefaultConnection": "Server=YourServer;Database=AuthApp;..."
    },
    "EmailSetting": {
        "SmtpHost": "smtp.gmail.com",
        "SmtpPort": "587",
        "SmtpUsername": "your-email@gmail.com",
        "SmtpPassword": "your-app-password"
    }
}
```

> **Note**: For security reasons, never commit sensitive credentials to version control. Use user secrets or environment variables in production.

## 🏃‍♂️ Running the Application

### Backend
```bash
cd AuthApp_DotNet
dotnet run
```

### Frontend
Navigate to the frontend directory and follow its specific setup instructions.

## 📝 API Documentation

API documentation can be accessed via Swagger when running the application:
```
https://localhost:{port}/swagger
```

---

## 🔐 Security Notes

- Update default credentials before deploying to production
- Enable HTTPS in production environments
- Consider implementing additional security measures like rate limiting

