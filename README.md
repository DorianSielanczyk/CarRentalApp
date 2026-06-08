<a id="readme-top"></a>

<div align="center">
<h1 align="center">Car Rental App</h1>

  <p align="center">
    A modular full-stack web application built with ASP.NET Core and Blazor, implementing Clean Architecture principles to streamline car rental operations.
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## About The Project

<div align="center">
  <p>
    Designed to connect customer convenience with efficient fleet management, this platform automates the daily workflows of a rental business.
    It provides a smooth booking experience for clients, while giving employees a set of powerful tools to manage cars, track reservations, and handle damage reports.
  </p>
</div>

  ### Homepage
  <img src="https://github.com/user-attachments/assets/ad33a83d-8e56-412d-9091-f9e5cd9b5d25" alt="Homepage Screenshot" width="100%" style="border: 1px solid #ddd; border-radius: 8px;" />

  <br />

</div>

### Built With

[![.NET 8][Dotnet-badge]][Dotnet-url]
[![C#][Csharp-badge]][Csharp-url] 
[![ASP.NET][Aspnet-badge]][Aspnet-url] 
[![Blazor][Blazor-badge]][Blazor-url]
[![Entity Framework Core][EFCore-badge]][EFCore-url]
[![ASP.NET Identity][Identity-badge]][Identity-url]
[![Azure SQL][AzureSql-badge]][AzureSql-url] 
[![MudBlazor][MudBlazor-badge]][MudBlazor-url] 
[![Bootstrap][Bootstrap-badge]][Bootstrap-url] 
[![Clean Architecture][CleanArch-badge]][CleanArch-url] 
[![Design Patterns][DesignPatterns-badge]][DesignPatterns-url]
[![xUnit][xUnit-badge]][xUnit-url]

[Dotnet-badge]: https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white
[Dotnet-url]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[Csharp-badge]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white
[Csharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[Aspnet-badge]: https://img.shields.io/badge/asp.net-%235C2D91.svg?style=for-the-badge&logo=dotnet&logoColor=white
[Aspnet-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
[Blazor-badge]: https://img.shields.io/badge/blazor-%235C2D91.svg?style=for-the-badge&logo=blazor&logoColor=white
[Blazor-url]: https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor
[EFCore-badge]: https://img.shields.io/badge/EF%20Core-388E3C?style=for-the-badge&logo=dotnet&logoColor=white
[EFCore-url]: https://learn.microsoft.com/en-us/ef/core/
[Identity-badge]: https://img.shields.io/badge/ASP.NET%20Identity-512BD4?style=for-the-badge
[Identity-url]: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity
[AzureSql-badge]: https://img.shields.io/badge/azure%20sql-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white
[AzureSql-url]: https://azure.microsoft.com/en-us/products/azure-sql/
[MudBlazor-badge]: https://img.shields.io/badge/MudBlazor-%23594AE2.svg?style=for-the-badge&logoColor=white
[MudBlazor-url]: https://mudblazor.com/
[Bootstrap-badge]: https://img.shields.io/badge/bootstrap-%238511FA.svg?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com/
[CleanArch-badge]: https://img.shields.io/badge/Clean%20Architecture-20232A?style=for-the-badge
[CleanArch-url]: https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture
[DesignPatterns-badge]: https://img.shields.io/badge/Design%20Patterns-20232A?style=for-the-badge
[DesignPatterns-url]: https://refactoring.guru/design-patterns
[xUnit-badge]: https://img.shields.io/badge/xUnit-000000?style=for-the-badge&logo=xunit&logoColor=white
[xUnit-url]: https://xunit.net/


<!-- GETTING STARTED -->
### Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

To run this application, make sure you have the following tools installed:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended)
* SQL Server (the free [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or LocalDB included with Visual Studio)

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/DorianSielanczyk/CarRentalApp.git
   
2. Open the solution file in your preferred IDE (Visual Studio or JetBrains Rider).

3. Ensure that the `DefaultConnection` in appsettings.Development.json points to your local SQL Server instance.

4. Set `CarRentalApp.WebUI.Server` as your startup project in Visual Studio.

5. Run the project (press `F5` or type `dotnet run` in the terminal inside the web project folder).
   
6. The database will be created automatically, and the tables will be populated with initial vehicles, categories, and users.

###  Default Test Accounts

These accounts allow you to test all system features, from booking a car (Customer), through reporting damage and managing returns (Worker), up to full fleet and employee management (Admin).

| System Role | Login (Email) | Password |
| :--- | :--- | :--- |
| **Customer** | `customer@carrental.com` | `Customer123!` |
| **Worker** | `worker@carrental.com` | `Worker123!` |
| **Admin** | `admin@carrental.com` | `Admin123!` |

###  Project Structure

The application follows the **Clean Architecture** pattern to ensure separation of concerns and high maintainability:

* **`CarRentalApp.Domain`** (Core Layer): Contains business entities and repository interfaces. It has no external dependencies.
* **`CarRentalApp.Application`** (Use Cases): Houses the business logic, services, and DTOs.
* **`CarRentalApp.Infrastructure`** (Data Layer): Manages Entity Framework Core, database migrations, repository implementations, and ASP.NET Core Identity.
* **`CarRentalApp.WebUI.Server`** (Presentation Layer): The user interface built using Blazor (Interactive Server) and MudBlazor components.
* **`CarRentalApp.Tests`** (Testing Layer): Automated unit tests using xUnit, Moq, and FluentAssertions to verify core business rules.

<!-- USAGE EXAMPLES -->
## Usage

  ### Booking
  <p>Book a vehicle using an interactive calendar, featuring real-time availability updates (thanks to Interactive Server mode) to prevent double-booking.</p>
  <img src="https://github.com/user-attachments/assets/691b2365-82c5-402e-af90-6e8276648f7e" alt="Booking Screenshot" width="100%" style="border: 1px solid #ddd; border-radius: 8px;" />

  <br />

  ### MudBlazor components
  <p>Enjoy a modern and responsive user interface with dialogs and form elements powered by the MudBlazor library.</p>
  <img src="https://github.com/user-attachments/assets/59d63d7e-7b4a-4610-a190-6b8a4e5e8300" alt="MudBlazor Components" width="955" style="border: 1px solid #ddd; border-radius: 8px;" />

  <br />

  ### Fleet management
  <p>Workers can easily add, update, or safely archive vehicle records within a management dashboard.</p>
  <img src="https://github.com/user-attachments/assets/e02798ec-7b2e-4b7c-9de6-e059ac9d964f" alt="Fleet Management Screenshot" width="100%" style="border: 1px solid #ddd; border-radius: 8px;" />

  <br />

  ### Reports
  <p>Report, document, and track vehicle damages or breakdowns to maintain the highest fleet quality standards.</p>
  <img src="https://github.com/user-attachments/assets/e421818f-2ae2-4c29-a928-1dbb7dc5712b" alt="Reports Screenshot" width="100%" style="border: 1px solid #ddd; border-radius: 8px;" />

  <br />

  And more...

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
