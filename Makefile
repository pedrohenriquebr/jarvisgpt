.PHONY: build publish watch

PROJECT_PATH = ./JarvisGPT.Api/JarvisGPT.Api.csproj
PROFILE = JarvisGPT.Api
DOTNET_FLAGS = /property:GenerateFullPaths=true /consoleloggerparameters:NoSummary

all: build publish run

build:
	dotnet build $(PROJECT_PATH) $(DOTNET_FLAGS)

publish:
	dotnet publish $(PROJECT_PATH) $(DOTNET_FLAGS)

watch:
	dotnet watch run --project $(PROJECT_PATH) --profile $(PROFILE)

run:
	dotnet run --project $(PROJECT_PATH) --profile $(PROFILE)