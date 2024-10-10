**Overview** 
The Auction App is a cross-platform mobile application developed using .NET MAUI and C#. It allows users to view, participate in, and manage vehicle auctions. 
This project includes an overhaul of the UI, the addition of new services, and the implementation of asynchronous data loading for auction items.

**Features**
  - Asynchronous auction data loading using System.Text.Json.
  - New models: Auction, Details, OwnerShip, Specification.
  - UI components: CardsComponent, TableViewComponent for dynamic displays.
  - Complete overhaul of login, vehicle initialization, and vehicle details pages.
  - Updated splash screen and color scheme.

**Prerequisites**
 - .NET SDK: Download .NET
 - Visual Studio 2022 (or later) with .NET MAUI workload installed.
 - MauiCheck tool (optional but recommended): Install using the command dotnet tool install -g Redth.Net.Maui.Check.

**Setup and Installation**
 - Clone the repository: git clone https://github.com/PedroPereira-10389/VehicleAuctionApp.git
 - cd VehicleAuctionApp
 - Install dependencies: dotnet restore
 - Build the project: dotnet build
 - Run the app: dotnet run
