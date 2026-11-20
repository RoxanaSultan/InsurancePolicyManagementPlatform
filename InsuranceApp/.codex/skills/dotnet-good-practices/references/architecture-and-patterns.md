# Architecture and patterns

## Layer responsibilities

Use the following dependency direction unless the application has a documented alternative:

`API -> Application -> Domain`

`Infrastructure -> Application and Domain`

- API owns controllers, middleware, authentication setup, serialization, Swagger, and the HTTP pipeline.
- Application owns use cases, service interfaces, DTOs, validators, mapping profiles, application exceptions, and cross-entity orchestration.
- Domain owns entities, value objects, enums, constants, and business state transitions. It should not depend on EF Core, ASP.NET Core, DTOs, or infrastructure services.
- Infrastructure owns EF Core `DbContext`, repository implementations, unit of work, entity configurations, authentication services, current-user adapters, and hosted/background jobs.

Keep the composition root explicit. Prefer `AddApplication`, `AddInfrastructure`, and API-specific registration methods over scattered service registration.

## API and application boundaries

Keep controllers thin. A controller should receive validated DTOs and route parameters, call an application interface, and return an HTTP result. Avoid database queries, mapping logic, transaction handling, or business calculations in controllers.

Use DTOs for input, output, filtering, and paging. Create/update DTOs may share a base DTO for common validation, while response DTOs should expose only the contract required by the caller.

Application services should:

- log the use-case boundary with structured identifiers;
- map input DTOs into entities;
- load required related data through repository abstractions;
- enforce uniqueness, authorization, existence, and cross-entity rules;
- call domain methods for state changes;
- save once at the use-case transaction boundary;
- map entities or projections into response DTOs.

Use typed exceptions such as not-found, bad-request, conflict, and unauthorized exceptions for expected application outcomes. Let API middleware map them to stable status codes and a consistent error response. Keep unexpected responses generic and log the exception server-side.

## Domain modeling

Prefer private setters or controlled mutation and expose meaningful behavior methods for important state transitions. For example, a policy can expose `Draft`, `Activate`, and `Cancel`, allowing the entity to own status rules instead of letting every caller assign status directly.

Use value objects for cohesive concepts such as addresses and contact information. Records can provide value equality; configure them as owned types in EF Core when they are persisted with their owning entity. Keep validation and invariants close to the value object when they are domain invariants.

Use enums and named constants for finite business concepts and role names. Avoid scattering magic strings or integer meanings across services and controllers.

## Validation and mapping

Use FluentValidation for request shape and input rules. Put reusable common rules in a base validator and compose it with `Include` in create/update validators. Use application services for rules requiring repository lookups or current-user context.

Keep mapping profiles grouped by feature or aggregate. Define request-to-entity and entity-to-response mappings explicitly for nested or differently named fields. Validate mapper configuration in application startup/tests where the project supports it.

## Persistence

Expose repository interfaces from Application and implementations from Infrastructure. A generic repository can cover simple CRUD; feature repositories should own specialized filters, eager-loading, paging, reports, and existence checks.

Use `AsNoTracking` for read-only queries. Apply filters before materialization, make paging deterministic, and pass cancellation tokens to EF Core async methods. Use `Include`/`ThenInclude` only for data required by the response or calculation.

Configure relationships, indexes, owned objects, conversions, delete behavior, and database constraints in dedicated `IEntityTypeConfiguration<T>` classes. Choose cascade/restrict behavior intentionally and test important repository behavior with an isolated database provider.

Use a unit of work when a use case changes multiple repositories and must save atomically. Do not call `SaveChangesAsync` repeatedly inside one logical operation without a reason.

## Conventions versus universal rules

The following are strong conventions observed in the reference project, not mandatory for every .NET codebase: AutoMapper profiles, a generic repository plus feature repositories, a unit-of-work facade, custom application exceptions, primary-constructor services, and separate administrator/broker controller areas. Preserve the host project’s established equivalent when it provides the same boundary and testability.

During reviews, identify tradeoffs. For example, a private helper using `CancellationToken.None`, an `InvalidOperationException` that bypasses the standard error contract, a vulnerable package version, or an obsolete EF API may be worth flagging even if surrounding code follows the architecture.
