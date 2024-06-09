# Currency Delta API

## Overview

A RESTful API to calculate exchange rate deltas between a base currency and other currencies over a specified date range.

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Setup

1. **Clone the Repository**
   ```sh
   git clone https://github.com/yourusername/yourrepository.git
   cd yourrepository
   
2. **Create Configuration File


cp appsettings.sample.json appsettings.json

3. **Update Configuration

Open appsettings.json and add your API key:
{
  "ExchangeRateApi": {
    "BaseUrl": "https://api.exchangerate-api.com/v4/latest/",
    "ApiKey": "YourApiKeyHere"
  }
}

4. **Install Dependencies

dotnet restore

5. **Run the Application

dotnet run# DeltaApi
