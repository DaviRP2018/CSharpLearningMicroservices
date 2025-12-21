# 1. Application Architecture Style

* Vertical Slice Architecture

## characteristics of Vertical Slice Architecture

...

## benefits & challenges of vertical slice architecture

...

## vertical slice architecture vs clean architecture

...

## when to use vertical slice architecture

* rapid development & deployment
* agile & scrum teams
* microservices

# 2. Patterns & principles

* CQRS pattern
* Mediator pattern
* DI in ASP.NET Core
* Minimal API and routing in asp.net core
* orm pattern

# 3. libraries nuget packages

* MediatR for CQRS
* carter for api endpoints
* marten for postgresql interaction
* mapster for object mapping
* fluentvalidation for validation

# 4. project folder structure

* model
* features
* data
* abstractions

# 5. Running docker over HTTPS

* https://learn.microsoft.com/pt-br/aspnet/core/security/docker-https?view=aspnetcore-10.0#running-pre-built-container-images-with-https
* dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p <CERT_PASSWORD>
* dotnet dev-certs https --trust
* Update .env with CERT_PASSWORD
