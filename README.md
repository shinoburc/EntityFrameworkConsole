# EntityFrameworkConsole

## Dependencies

- .NET Core 2.2
- PostgreSQL

## Run

```sh

$ git clone https://github.com/shinoburc/EntityFrameworkConsole.git
$ cd EntityFrameworkConsole
$ vi appsettings.json
```

```csharp
  "ConnectionStrings": {
    "AppDbContext": "Host=localhost;Port=5432;User Id=postgres;Password=postgres;Database=consoleapp"
  }
```

```sh
$ dotnet restore
$ dotnet ef database update
$ dotnet run

```
