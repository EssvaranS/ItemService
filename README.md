# ItemService

## Overview
This project is a production-ready ASP.NET Core Web API microservice for managing items, built as part of the Riverstone Infotech technical assessment. It demonstrates clean architecture, MongoDB integration, robust validation, global error handling, and modern API practices.

---

## Features
- **CRUD API for Items**: Create, read, update, and delete items.
- **Health Check Endpoint**: `/health` returns service and database status.
- **Consistent API Responses**: All endpoints return a standard response format.
- **Validation**: FluentValidation for input validation.
- **Global Error Handling**: Middleware for consistent error responses with error codes.
- **Timestamps**: Automatic `createdAt` and `updatedAt` fields for items.
- **Unit Tests**: xUnit and Moq for service layer tests.

---

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [MongoDB](https://www.mongodb.com/try/download/community) (local or use Docker)
- (Optional) [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

---

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ItemService.git
cd ItemService
```
---

### 2. Run Locally
1. Ensure MongoDB is running locally or accessible via your connection string.
2. Set the required environment variables (see above).
3. Build and run the API:

```bash
dotnet build
cd ItemService.Api
dotnet run
```
- The API will be available at: [https://localhost:3000](https://localhost:3000) 
- You can override the port in the launch settings.

---

## API Endpoints

### Item CRUD
- `POST   /api/items`        – Create item
- `GET    /api/items`        – Retrieve all items
- `GET    /api/items/{id}`   – Retrieve item by ID
- `PUT    /api/items/{id}`   – Update item
- `DELETE /api/items/{id}`   – Delete item

### Health Check
- `GET /health` – Returns:
```json
{
	"status": "UP",
	"dbStatus": "Connected",
	"uptime": "120s"
}
```

---

## Response Format
All API responses follow this structure:
```json
{
	"success": true,
	"message": "",
	"data": { }
}
```
On error:
```json
{
	"success": false,
	"errorCode": "ITEMSVC-00001",
	"message": "Error message",
	"details": "Stack trace (if available)"
}
```

---

## Running Tests
Navigate to the test project and run:
```bash
cd ItemService.Tests
dotnet test
```

---

## Project Structure
- `ItemService.Api/`         – ASP.NET Core Web API
- `ItemService.Application/` – Application logic, DTOs, validators
- `ItemService.Domain/`      – Domain entities
- `ItemService.Infrastructure/` – MongoDB context, repositories, unit of work
- `ItemService.Tests/`       – Unit tests



---

## Notes
- The database and collections are created automatically on first write.
- All major components are commented for clarity.
- For any issues, please raise an issue in the repository or contact the author.

---