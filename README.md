# RideSharingApp

This project is a Clean Architecture .NET solution for a ride-sharing business domain (Uber-like) with:
- Subscription, login, JWT authentication/authorization
- SQL Server persistence using Dapper
- Scrutor for dependency injection (no MediatR)
- Domain events with Azure ServiceBus
- RESTful endpoints
- FluentValidation and Result Pattern
- Unit tests with MOQ, XUnit
- Architecture tests with TngTech.ArchUnitNET
- Performance best practices

## Instalação

1. Abra um terminal da pasta `src`.
2. Rode o comando dotnet new install .

**outout**

```bash
Nome do modelo                              Nome Curto         Idioma  Tags
------------------------------------------  -----------------  ------  --------------------------------------------
RideSharingApp Clean Architecture Template  cleanarchtemplate  [C#]    Web/API/Clean Architecture/Flyway/PostgreSQL
 ```

## Usando o Templete

1. Abra um terminal no diretório que será criado a aplicação
2. Execute o comando `dotnet new cleanarchtemplate -n <NOME DO PROJETO> --Framework net8.0`.

**outout**

```bash
O modelo "RideSharingApp Clean Architecture Template" foi criado com êxito.
```
