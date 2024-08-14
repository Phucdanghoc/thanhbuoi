<!-- Enhanced compatibility of back to top link: See: https://gitlab.duthu.net/S52100852/webud/pull/73 -->
<a name="readme-top"></a>

## About The Project
Thanh Buoi is an inter-provincial bus management system developed with Website and Mobile platforms.
<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With
Built With
- .NET: Official Website
- SQL Server: Official Website
- MongoDB: MongoDB Website
- Bootstrap: Bootstrap Website
- JQuery: JQuery Website

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Software Development Principles, Patterns, and Practices
- **Model-View-Controller (MVC):** The application follows the MVC pattern, separating data, user interface, and control logic.


## Getting Started

### Prerequisites
#### Before you begin, ensure you have the following installed:
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads): Required for the upcoming .NET remake of the project.
- [Microsoft .NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet): Needed for running and developing .NET applications.
### Installation
1. Clone the repository:
   ```sh
   git clone [git clone https://github.com/Phucdanghoc/thanhbuoi.git](https://github.com/Phucdanghoc/thanhbuoi.git)
2. Open Visual Studio Code or your preferred IDE.
3. Open the Integrated Terminal and run : 
    ```sh 
    dotnet restore
    This command will install all dependencies required to run the project.

4. Run migrations to set up the database: 
    ```sh 
    dotnet ef database update
    (Ensure Entity Framework is installed if using EF for migrations.)
   <p align="right">(<a href="#readme-top">back to top</a>)</p>

### Access
 Admin:
 - Username: admin@admin.com    
 - Password: Admin123
### Features
 - Bus Scheduling: Manage bus schedules and routes efficiently.
 - Ticket Management: Handle ticket sales, reservations, and cancellations.
 - User Management: Admin and user accounts with different levels of access.
 - Real-Time Updates: Get real-time updates on bus locations and schedules.
