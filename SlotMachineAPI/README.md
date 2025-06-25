# Slot Machine API

A comprehensive slot machine API built with ASP.NET Core and Entity Framework Core.

## Features

- **Symbol System**: Database-driven symbols with configurable values and weights
- **Weighted Random Selection**: Each symbol has a specific frequency on the reel
- **User Management**: Create users, manage balance, free spins, and multipliers
- **Game Logic**: Spin functionality with win detection and payout calculation
- **Game History**: Track all spins and results
- **Free Spins**: Bonus feature with free spin mechanics

## API Endpoints

### Users

#### Create User
```
POST /api/users
{
  "userName": "string"
}
```

#### Get User
```
GET /api/users/{userId}
```

#### Add Cash
```
POST /api/users/{userId}/add-cash
{
  "amount": decimal
}
```

#### Add Free Spins
```
POST /api/users/{userId}/add-free-spins
{
  "count": int (default: 10)
}
```

#### Cashout
```
POST /api/users/{userId}/cashout
```

### Games

#### Spin
```
POST /api/games/{userId}/spin
{
  "betAmount": decimal (default: 1.0)
}
```

#### Get Game History
```
GET /api/games/{userId}/history?limit=10
```

#### Check if User Can Play
```
GET /api/games/{userId}/can-play
```

### Symbols

#### Get All Symbols
```
GET /api/symbols
```

#### Get Symbol by ID
```
GET /api/symbols/{id}
```

#### Update Symbol
```
PUT /api/symbols/{id}
{
  "value": decimal,
  "weight": int
}
```

#### Get Symbol Statistics
```
GET /api/symbols/statistics
```

#### Reset to Default Values
```
POST /api/symbols/reset-to-defaults
```

## Default Symbol Configuration

| Symbol | Value ($) | Weight | Probability |
|--------|-----------|--------|-------------|
| Nine | 0.25 | 256 | 50.2% |
| Ten | 0.50 | 128 | 25.1% |
| Jack | 1.00 | 64 | 12.5% |
| Queen | 2.00 | 32 | 6.3% |
| King | 4.00 | 16 | 3.1% |
| Ace | 8.00 | 8 | 1.6% |
| Bonus | 0.00 | 4 | 0.8% |
| Jackpot | 100.00 | 2 | 0.4% |

## Win Logic

A win occurs when all three symbols in the spin result are identical. The win amount is calculated as:
`Symbol Value × Bet Amount × User Multiplier`

## Database

The API uses Entity Framework Code First with SQL Server LocalDB. The database will be automatically created on first run and seeded with default symbols.

## Running the API

1. Ensure you have .NET 9.0 installed
2. Navigate to the SlotMachineAPI directory
3. Run `dotnet run`
4. Access Swagger UI at `https://localhost:7001/swagger`

## Example Usage

1. Create a user: `POST /api/users` with `{"userName": "Player1"}`
2. Add cash: `POST /api/users/1/add-cash` with `{"amount": 50}`
3. Spin: `POST /api/games/1/spin` with `{"betAmount": 2.0}`
4. Check history: `GET /api/games/1/history`
5. View symbol statistics: `GET /api/symbols/statistics` 