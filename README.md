# Slot Machine Project

A complete slot machine game implementation with a .NET 9.0 API backend and Blazor UI frontend, featuring realistic slot machine mechanics, user management, and comprehensive testing.

## 🎰 Project Overview

This project consists of three main components:

- **Slot Machine API**: .NET 9.0 Web API with Entity Framework Core
- **Slot Machine UI**: Blazor Server/WebAssembly hybrid application
- **Slot Machine Tests**: Comprehensive xUnit test suite

## 📁 Project Structure

```
SlotMachine/
├── SlotMachineAPI/              # Backend API and business logic
│   ├── Controllers/             # API endpoints
│   ├── Services/                # Business logic services
│   ├── Models/                  # Data models
│   ├── Data/                    # Entity Framework context
│   ├── DTOs/                    # Data transfer objects
│   └── README.md               # API documentation
├── SlotMachineUI/               # Frontend Blazor application
│   ├── SlotMachineUI/          # Main Blazor Server project
│   ├── SlotMachineUI.Client/   # Blazor WebAssembly project
│   └── README.md               # UI documentation
├── SlotMachineAPI.Tests/        # Test suite
│   ├── GameLogicServiceTests.cs
│   ├── UserModelTests.cs
│   ├── GameModelTests.cs
│   └── README.md               # Testing documentation
└── README.md                   # This file
```

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- SQLite (included, no additional setup required)

### Getting Started

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SlotMachine
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the API**
   ```bash
   cd SlotMachineAPI
   dotnet run
   ```

4. **Run the UI** (in a new terminal)
   ```bash
   cd SlotMachineUI/SlotMachineUI
   dotnet run
   ```

5. **Access the application**
   - UI: `https://localhost:5001`
   - API: `https://localhost:7076`
   - API Documentation: `https://localhost:7076/swagger`

## 🎮 Game Features

### Core Gameplay
- **Interactive Slot Machine**: Realistic 3-reel slot machine
- **User Management**: Create accounts, manage balances, track history
- **Free Spins System**: Earn free spins through bonus symbols
- **Real-time Updates**: Live balance and game state updates
- **Game History**: View recent spins with detailed results

### Bonus System
- **1 Bonus Symbol**: 5 free spins
- **2 Bonus Symbols**: 10 free spins (5 each)
- **3 Bonus Symbols**: 10 free spins (special bonus)

### Symbols and Payouts
- **Nine**: $0.25 payout
- **Ten**: $0.50 payout
- **Jack**: $1.00 payout
- **Queen**: $2.00 payout
- **King**: $4.00 payout
- **Ace**: $8.00 payout
- **Bonus**: Awards free spins
- **Jackpot**: $100.00 payout

## 🏗️ Architecture

### Technology Stack
- **Backend**: .NET 9.0, ASP.NET Core, Entity Framework Core, SQLite
- **Frontend**: Blazor Server, Blazor WebAssembly, MudBlazor
- **Testing**: xUnit, Moq, In-Memory Database
- **Documentation**: Swagger/OpenAPI

### Design Patterns
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic encapsulation
- **Dependency Injection**: Loose coupling and testability
- **DTO Pattern**: Clean API contracts

## 📚 Documentation

### Project-Specific Documentation
- **[API Documentation](SlotMachineAPI/README.md)**: Backend API details, endpoints, and setup
- **[UI Documentation](SlotMachineUI/README.md)**: Frontend application guide and features
- **[Testing Documentation](SlotMachineAPI.Tests/README.md)**: Test suite overview and running tests

### API Endpoints
- **Users**: Create, retrieve, and manage user accounts
- **Games**: Spin the slot machine and retrieve game history
- **Symbols**: Manage slot machine symbols and view statistics

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test SlotMachineAPI.Tests/

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Coverage
- **Unit Tests**: Service layer, models, and business logic
- **Integration Tests**: API endpoints and database operations
- **Component Tests**: UI component functionality

## 🔧 Development

### Development Setup
1. **API Development**: Use the API project for backend development
2. **UI Development**: Use the UI project for frontend development
3. **Testing**: Use the test project for adding new tests

### Code Style
- Follow C# coding conventions
- Use async/await for asynchronous operations
- Implement proper error handling
- Write comprehensive tests for new features

### Database
- **SQLite**: In-memory database for development and testing
- **Migrations**: Entity Framework migrations for schema changes
- **Seeding**: Default symbols and test data

## 🚀 Deployment

### Production Build
```bash
# Build API
dotnet publish SlotMachineAPI -c Release -o ./publish/api

# Build UI
dotnet publish SlotMachineUI/SlotMachineUI -c Release -o ./publish/ui
```

### Deployment Options
- **Azure App Service**: Host both API and UI
- **Docker**: Containerized deployment
- **IIS**: Traditional ASP.NET Core hosting
- **Static Hosting**: WebAssembly static files

## 🤝 Contributing

### Development Guidelines
1. **Fork the repository**
2. **Create a feature branch**
3. **Follow coding conventions**
4. **Add tests for new features**
5. **Update documentation**
6. **Submit a pull request**

### Pull Request Process
1. Ensure all tests pass
2. Update relevant documentation
3. Follow the existing code style
4. Provide clear commit messages

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

### Getting Help
- **API Issues**: Check the [API documentation](SlotMachineAPI/README.md)
- **UI Issues**: Check the [UI documentation](SlotMachineUI/README.md)
- **Testing Issues**: Check the [Testing documentation](SlotMachineAPI.Tests/README.md)
- **General Issues**: Open an issue on the repository

### Common Issues
- **Port Conflicts**: Ensure ports 5001 and 7076 are available
- **Database Issues**: Check SQLite file permissions
- **Build Issues**: Ensure .NET 9.0 SDK is installed

## 🎯 Roadmap

### Planned Features
- [ ] User authentication and authorization
- [ ] Leaderboards and additional statistics
- [ ] Progressive jackpots

### Technical Improvements
- [ ] Performance optimizations
- [ ] Enhanced error handling
- [ ] Comprehensive logging
- [ ] Monitoring and analytics
- [ ] CI/CD pipeline

---

**Happy Spinning! 🎰**

For detailed information about each component, please refer to the specific README files in each project folder. 