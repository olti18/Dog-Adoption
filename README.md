#Dog adoption
🐶 Dog Adoption Platform
A web-based application built with ASP.NET MVC and SQL Server that connects dog shelters with individuals looking to adopt.

📌 Overview
The Dog Adoption Platform allows animal shelters to list dogs available for adoption and enables users to browse, search, and apply to adopt dogs. It simplifies the adoption process by providing a user-friendly interface and management tools for both admins and users.

🚀 Features
🐕 View available dogs with images, age, breed, and description

🔍 Filter and search dogs based on breed, age, and location

📝 Submit adoption applications

👤 User registration and login (authentication & authorization)

🛠️ Admin dashboard for managing dogs and adoption requests

📅 Track adoption request statuses

🧰 Tech Stack
Backend: ASP.NET MVC (.NET Framework or .NET Core)

Frontend: Razor Views, Bootstrap

Database: Microsoft SQL Server

ORM: Entity Framework

Authentication: ASP.NET Identity



bash
Copy
Edit
git clone https://github.com/olti18/Dog-Adoption.git
Configure the Database

Update your connection string in appsettings.json or web.config:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=DogAdoptionDB;Trusted_Connection=True;"
}
Apply Migrations

bash
Copy
Edit
Update-Database
Run the Application

bash
Copy
Edit
dotnet run


🙌 Contribution
Feel free to fork the repo and submit pull requests. Feedback and feature suggestions are always welcome!
