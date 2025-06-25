# Slot Machine UI

A modern Blazor-based user interface for the Slot Machine game, built with MudBlazor components and supporting both Blazor Server and WebAssembly rendering modes.

## 🎰 Features

### Core Gameplay
- **Interactive Slot Machine**: Spin the reels with realistic slot machine mechanics
- **User Management**: Create accounts, manage balances, and track game history
- **Free Spins System**: Earn free spins through bonus symbols
- **Real-time Updates**: Live balance and game state updates
- **Game History**: View recent spins with detailed results

### User Experience
- **Modern UI**: Clean, responsive design using MudBlazor components
- **Responsive Layout**: Works seamlessly on desktop and mobile devices
- **Real-time Notifications**: Snackbar notifications for wins, losses, and bonuses
- **User Panel**: Quick access to add cash, cashout, and account management
- **Recent Users**: Easy selection from recently created accounts

### Technical Features
- **Hybrid Rendering**: Supports both Blazor Server and WebAssembly modes
- **API Integration**: Communicates with the Slot Machine API backend
- **State Management**: Efficient component state management
- **Error Handling**: Graceful error handling with user-friendly messages

## 🏗️ Architecture

### Project Structure
```
SlotMachineUI/
├── SlotMachineUI/                 # Main Blazor Server project
│   ├── Components/                # Blazor components
│   │   ├── Pages/                 # Main application pages
│   │   │   ├── Home.razor         # Main game interface
│   │   │   ├── Statistics.razor   # Game statistics and analysis
│   │   │   ├── Symbols.razor      # Symbol management
│   │   │   └── Error.razor        # Error page
│   │   ├── UserLogin.razor        # User login/creation component
│   │   ├── SlotMachineGame.razor  # Main slot machine component
│   │   ├── UserPanel.razor        # User account management
│   │   ├── GameHistory.razor      # Game history display
│   │   └── Layout/                # Layout components
│   ├── Services/                  # Business logic and API communication
│   │   └── SlotMachineService.cs  # API service layer
│   ├── Models/                    # Data models
│   ├── Helpers/                   # Utility classes
│   └── wwwroot/                   # Static assets
└── SlotMachineUI.Client/          # Blazor WebAssembly project
    ├── Pages/                     # WebAssembly pages
    │   └── Counter.razor          # Template counter page
    └── wwwroot/                   # WebAssembly static assets
```

### Technology Stack
- **.NET 9.0**: Latest .NET framework
- **Blazor Server**: Main rendering mode for real-time interactions
- **Blazor WebAssembly**: Client-side rendering for enhanced performance
- **MudBlazor**: Modern UI component library
- **HTTP Client**: API communication with the backend
- **Bootstrap**: Additional styling framework

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code
- Slot Machine API running on `https://localhost:7076`

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SlotMachineUI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run --project SlotMachineUI/SlotMachineUI.csproj
   ```

5. **Access the application**
   - Open your browser and navigate to `https://localhost:5001`
   - The application will automatically connect to the API

### Development Setup

1. **API Configuration**
   - Ensure the Slot Machine API is running on `https://localhost:7076`
   - The UI is configured to communicate with this endpoint

2. **Development Mode**
   - The application runs in development mode by default
   - WebAssembly debugging is enabled for enhanced debugging
   - Hot reload is available for both Server and WebAssembly components

## 🎮 How to Play

### Getting Started
1. **Create an Account**: Enter a username to create or reuse an existing account
2. **Add Cash**: Use the user panel to add money to your balance
3. **Start Playing**: Click the spin button to play the slot machine
4. **Track Progress**: Monitor your balance and game history in real-time

### Game Mechanics
- **Betting**: Each spin costs $1.00 (or uses a free spin)
- **Winning**: Match 3 symbols to win (payout based on symbol value)
- **Free Spins**: Earn free spins by landing bonus symbols
  - 1 Bonus symbol = 5 free spins
  - 2 Bonus symbols = 10 free spins (5 each)
  - 3 Bonus symbols = 10 free spins (special bonus)
- **Cashout**: Withdraw your winnings at any time

### Symbols and Values
- **Nine**: $0.25 payout
- **Ten**: $0.50 payout
- **Jack**: $1.00 payout
- **Queen**: $2.00 payout
- **King**: $4.00 payout
- **Ace**: $8.00 payout
- **Bonus**: Awards free spins (1-2 bonus = 5 free spins each, 3 bonus = 10 free spins total)
- **Jackpot**: $100.00 payout

## 🔧 Configuration

### API Endpoint
The UI is configured to communicate with the API at `https://localhost:7076`. To change this:

1. **Update Program.cs**
   ```csharp
   builder.Services.AddHttpClient<ISlotMachineService, SlotMachineService>(client =>
   {
       client.BaseAddress = new Uri("YOUR_API_URL");
   });
   ```

2. **Environment-specific settings**
   - Create `appsettings.Development.json` for development
   - Create `appsettings.Production.json` for production

### Rendering Modes
The application supports both rendering modes:

- **Blazor Server**: Default mode with real-time server communication
- **Blazor WebAssembly**: Client-side rendering for enhanced performance

To switch modes, modify the component render modes in the application.

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test SlotMachineAPI.Tests/
```

### UI Testing
- Manual testing through the browser interface
- Component testing using browser developer tools
- API integration testing through the UI

## 📁 Key Components

### Core Components
- **Home.razor**: Main application page with game interface
- **UserLogin.razor**: User authentication and account creation
- **SlotMachineGame.razor**: Interactive slot machine component
- **UserPanel.razor**: Account management and quick actions
- **GameHistory.razor**: Recent game history display

### Services
- **SlotMachineService**: Handles all API communication
- **ISlotMachineService**: Service interface for dependency injection

### Models
- **User**: User account information
- **Game**: Game session data
- **Symbol**: Slot machine symbols and values

## 🎨 Styling

### MudBlazor Components
The UI uses MudBlazor for a modern, Material Design-inspired interface:
- **MudPaper**: Card-like containers
- **MudButton**: Interactive buttons
- **MudSnackbar**: Toast notifications
- **MudGrid**: Responsive layout system
- **MudText**: Typography components

### Custom Styling
- **app.css**: Custom styles and overrides
- **Bootstrap**: Additional styling framework
- **Responsive Design**: Mobile-first approach

## 🔄 State Management

### Component State
- **Local State**: Component-specific data
- **Service State**: Shared data through dependency injection
- **Real-time Updates**: Live updates from API responses

### User Session
- **Session Management**: User state persistence
- **Auto-refresh**: Automatic data updates
- **Error Recovery**: Graceful error handling

## 🚀 Deployment

### Production Build
```bash
dotnet publish -c Release -o ./publish
```

### Deployment Options
- **Azure App Service**: Host both UI and API
- **Docker**: Containerized deployment
- **Static Hosting**: WebAssembly static files
- **IIS**: Traditional ASP.NET Core hosting

## 🤝 Contributing

### Development Guidelines
1. **Code Style**: Follow C# coding conventions
2. **Component Structure**: Use proper Blazor component patterns
3. **Error Handling**: Implement proper error handling
4. **Testing**: Add tests for new features
5. **Documentation**: Update documentation for changes

### Pull Request Process
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Check the API documentation
- Review the test files for examples
- Open an issue on the repository

---

**Happy Spinning! 🎰**
