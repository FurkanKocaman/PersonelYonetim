# PersonelYonetim

PersonelYonetim is a personnel management system built using .NET 9 and C# 13.0. This project includes various functionalities for managing personnel, departments, positions, and leave requests.

## Project Structure

The project is organized into several layers:

- **Domain**: Contains the core business logic and domain entities.
- **Application**: Contains the application logic, including commands, queries, and handlers.
- **Infrastructure**: Contains the implementation of repositories and other infrastructure-related code.
- **WebAPI**: Contains the API controllers and endpoint definitions.

## Domain Layer

### Entities

- **Personel**: Represents a personnel entity with properties like `Ad`, `Soyad`, `Iletisim`, `Adres`, `UserId`, etc.
- **IzinTalep**: Represents a leave request entity with properties like `PersonelId`, `BaslangicTarihi`, `BitisTarihi`, `IzinTurId`, etc.
- **PersonelAtama**: Represents personnel assignment details.
- **IzinTur**: Represents the type of leave.
- **DegerlendirmeDurumEnum**: Represents the evaluation status of a leave request.

### Repositories

- **IPersonelRepository**: Interface for personnel repository.
- **IIzinTalepRepository**: Interface for leave request repository.
- **ISirketRepository**: Interface for company repository.
- **ISubeRepository**: Interface for branch repository.
- **IDepartmanRepository**: Interface for department repository.
- **IPozisyonRepository**: Interface for position repository.

## Application Layer

### Commands

- **PersonelCreateCommand**: Command for creating a new personnel.
- **PersonelUpdateCommand**: Command for updating personnel details.
- **PersonelDeleteCommand**: Command for deleting a personnel.
- **IzinTalepCreateCommand**: Command for creating a new leave request.
- **IzinTalepUpdateCommand**: Command for updating a leave request.
- **IzinTalepDeleteCommand**: Command for deleting a leave request.

### Queries

- **IzinTalepGetAllQuery**: Query for retrieving all leave requests.

### Handlers

- **PersonelCreateCommandHandler**: Handles the creation of a new personnel.
- **PersonelUpdateCommandHandler**: Handles the updating of personnel details.
- **PersonelDeleteCommandHandler**: Handles the deletion of a personnel.
- **IzinTalepCreateCommandHandler**: Handles the creation of a new leave request.
- **IzinTalepUpdateCommandHandler**: Handles the updating of a leave request.
- **IzinTalepDeleteCommandHandler**: Handles the deletion of a leave request.
- **IzinTalepGetAllQueryHandler**: Handles the retrieval of all leave requests.

## WebAPI Layer

### Controllers

- **PersonelController**: API controller for managing personnel.
- **IzinTalepController**: API controller for managing leave requests.

### Endpoints

- **/personeller/create**: Endpoint for creating a new personnel.
- **/personeller/update**: Endpoint for updating personnel details.
- **/personeller/delete**: Endpoint for deleting a personnel.
- **/izintalepler/create**: Endpoint for creating a new leave request.
- **/izintalepler/update**: Endpoint for updating a leave request.
- **/izintalepler/delete**: Endpoint for deleting a leave request.
- **/izintalepler/getall**: Endpoint for retrieving all leave requests.

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio 2022

### Running the Application

1. Clone the repository.
2. Open the solution in Visual Studio 2022.
3. Restore the NuGet packages.
4. Update the database connection string in `appsettings.json`.
5. Run the application.

### Contributing

Contributions are welcome! Please open an issue or submit a pull request.

### License

This project is licensed under the MIT License.
