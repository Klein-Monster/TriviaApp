# TriviaApp

A Blazor Server application that fetches trivia questions from an external API and allows users to answer them.

---

## Running the App

Make sure you have .NET installed.

Run the application:

dotnet run --project TriviaApp.Web

Open the app in your browser (URL will be shown in the console).

---

## Build

dotnet build

---

## Tests

dotnet test

---

## Architecture

- **TriviaApp.Web** → UI (Blazor Server)
- **TriviaApp.Domain** → business logic & validation
- **TriviaApp.Adapter** → external API integration
- **TriviaApp.Adapter.Tests** → Unit tests for Adapter project

---

## External API

Uses the Open Trivia DB:
https://opentdb.com/api.php

---

## Notes

- Correct answers are not exposed to the client
- Answers are shuffled before being sent to the UI
- Correct answers are stored in memory for validation
- In order to show testability of my code I have implemented some unit tests but not all
