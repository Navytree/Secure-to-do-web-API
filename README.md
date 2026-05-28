## A secure To-Do web application featuring password hashing (salting), architectural DTO isolation, and JWT Bearer token authentication.

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
* [Setup](#setup)

## General info
This project is an advanced To-Do list application designed with a strong focus on web security, user authentication, and fully automated data lifecycle/history tracking.

## Technologies
* **Architecture:** WebAPI (REST)
* **Backend:** .NET 8, C#, Entity Framework Core
* **Security:** JWT Bearer Authentication, Custom Password Salting & Hashing
* **Frontend:** Dynamic JavaScript (ES6+), HTML5
* **Database:** SQL Server

## Features
* **Advanced Password Hashing:** Implemented secure cryptographic password storage utilizing salting techniques. This ensures unique database hashes even for identical plain-text passwords, preventing rainbow table attacks.
* **Architectural Data Isolation (DTOs):** Handled API request/response flows through specialized Data Transfer Objects (DTOs), preventing internal database entity exposure and over-posting vulnerabilities.
* **JWT Stateless Authentication:** Secure user authentication managed via JWT (JSON Web Tokens). User identifiers are cryptographically signed, preventing raw user IDs from being exposed to the client-side JavaScript environment.
* **Dynamic Lifecycle Management & History:** Completed or overdue tasks automatically transition into a historical state. The UI dynamically grants edit/delete permissions based on task state, enforcing strict domain validation rules.
* **Due Date Extensions:** Provides flexible task management by allowing users to extend deadlines directly on existing domain models instead of forcing redundant task creation.


## Setup
To run this project locally, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Navytree/Secure-to-do-web-API.git

2. **Update Database Connection:**
Open appsettings.json and update the DefaultConnection string to point to your local SQL Server instance.

3. **Apply Migrations:**
Run the following command in the Package Manager Console (Visual Studio) or Terminal to create the database:
   ```bash
    dotnet ef database update
   
4. **Run the application:**
  Press F5 in Visual Studio 2022 or use the command:
     ```bash
     dotnet run
