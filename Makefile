.PHONY: all
all:
	dotnet build

.PHONY: test
test:
	dotnet test Wake.Tests
