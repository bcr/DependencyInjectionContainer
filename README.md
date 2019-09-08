# Dependency Injection Container Code Kata

This is an implementation of the ["Dependency Injection Container Code Kata"](https://gist.github.com/jhubermilliman/3e304cf712bed4e97f5b7d60d28fe230)

This solution is meant to be compatible with [.NET Core](https://dotnet.microsoft.com), originally implemented using .NET Core 2.2.

## Running The Tests

I have implemented unit tests using the [xUnit](https://xunit.net) framework. To run the tests use:

`dotnet test Bcr.CodeKata.DependencyInjectionContainer.Test`

## How I Created Things

You don't have to do any of this, but this is how I created everything

`dotnet new classlib -o Bcr.CodeKata.DependencyInjectionContainer`
`dotnet new xunit -o Bcr.CodeKata.DependencyInjectionContainer.Test`
`dotnet add Bcr.CodeKata.DependencyInjectionContainer.Test reference Bcr.CodeKata.DependencyInjectionContainer`
`dotnet test Bcr.CodeKata.DependencyInjectionContainer.Test`
