all:
	dotnet build ./Bot/OWLeagueBot.csproj
	dotnet build ./Tests/OWLeagueBot.Tests.csproj

clean:
	dotnet clean ./Bot/OWLeagueBot.csproj
	dotnet clean ./Tests/OWLeagueBot.Tests.csproj
run:
	dotnet run --project ./Bot/OWLeagueBot.csproj