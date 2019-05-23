# EntityFrameworkConsole

## Dependencies

- .NET Core 2.2
- PostgreSQL

## Run

```sh

$ git clone https://github.com/shinoburc/EntityFrameworkConsole.git
$ cd EntityFrameworkConsole
$ vi Program.cs
```

```csharp
  opt.UseNpgsql("Host=localhost;Database=consoleapp;Username=postgres;Password=postgres");
```

```sh
$ dotnet restore
$ dotnet ef database update
$ dotnet run

```
