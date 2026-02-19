---
title: Installation
category: Getting Started
order: 1
description: Install sevDesk.NET and configure your project.
---

## NuGet Package

Install the sevDesk.NET package via the .NET CLI:

```bash
dotnet add package sevDesk.NET
```

Or via the NuGet Package Manager:

```powershell
Install-Package sevDesk.NET
```

## Requirements

- .NET 10.0 or later
- A sevDesk account with an API token

## Project Setup

Add the sevDesk.NET namespace to your project:

```csharp
using sevDesk.NET;
using sevDesk.NET.Clients;
using sevDesk.NET.Models;
```

## Next Steps

- [Quick Start](/docs/getting-started/quick-start) — Make your first API call
- [Configuration](/docs/getting-started/configuration) — Customize the client behavior
- [Authentication](/docs/getting-started/authentication) — Learn about API token management
