# JustChat
Simple web chat made with SignalR and .NET 6 MVC. Authentication and Authorization backend is based on my [JustAuth](https://github.com/Vansh0t/JustAuth) package. PostgreSQL is used as a database with [Npgsql](https://www.npgsql.org) EF Core provider.
## Preview
![JustChat2](https://user-images.githubusercontent.com/35566242/170767549-ea2d441e-fc11-4d13-b4b5-5d44117a59cd.gif)
## Features
1. Full authentication and authorization from my [JustAuth](https://github.com/Vansh0t/JustAuth) package: Sign Up, Sign In, Sign Out, Email Verification, Email Change, Password Reset
2. Chat restricted only to users with email verified
3. Realtime chat messaging with SignalR
4. Chat history is preserved in PostgreSQL
5. Pagination for chat history (25 messages in a batch) while scrolling
6. User profile
7. User avatars
## Setup
1. Make sure you have [.NET6 Runtime](https://dotnet.microsoft.com/en-us/download) installed
1. Create ``justauth.json`` in the root folder as described in [JustAuth Setup](https://github.com/Vansh0t/JustAuth#justauthjson). It is not included in release package for security reasons.
2. Create user in PostgreSQL named ``v4`` with password ``GWw51AG161XX`` OR edit ``DefaultConnection`` string in ``appsettings.json`` for your PostgreSQL user
