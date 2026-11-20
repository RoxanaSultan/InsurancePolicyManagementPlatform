---
name: dotnet-good-practices
description: Implement and review ASP.NET Core and .NET applications using layered architecture, clear API/Application/Domain/Infrastructure boundaries, DTOs, validation, mapping, domain behavior, persistence abstractions, centralized errors, dependency injection, logging, cancellation, and proportionate automated tests. Use for new features, refactors, bug fixes, architecture reviews, and code reviews.
---

# .NET Good Practices

Use this skill to preserve strong boundaries and repeatable engineering practices in ASP.NET Core applications. Treat the reference files as guidance, not as a reason to force one pattern onto a project with different constraints.

## Inspect before changing

1. Identify the solution, project references, target framework, existing conventions, and test projects.
2. Trace one representative request through controller, application service, repository, domain model, persistence configuration, and tests.
3. Check the working tree before editing and preserve unrelated changes.
4. Reuse existing naming, folder, DTO, validation, mapping, exception, logging, and test conventions unless the task explicitly changes them.

Read [architecture-and-patterns.md](references/architecture-and-patterns.md) for the boundary and implementation checklist. Read [testing-practices.md](references/testing-practices.md) when adding or reviewing tests.

## Implement changes

- Keep controllers thin: bind input, authorize, call an application interface, and translate the result to the HTTP response.
- Put use-case orchestration in Application services. Inject interfaces, map request DTOs to domain entities, enforce cross-aggregate rules, persist through abstractions, and map entities to response DTOs.
- Keep business state transitions and invariants in Domain entities or value objects. Prefer explicit methods such as `Activate`, `Cancel`, or `Draft` over public state mutation.
- Keep persistence concerns in Infrastructure. Use repositories for query shape and persistence details, `AsNoTracking` for read-only queries, explicit EF configuration for relationships/indexes/value objects, and a unit of work when a use case needs one atomic save.
- Define request, response, filter, and paging DTOs at the application boundary. Do not expose EF entities as API contracts.
- Use FluentValidation for input rules. Compose shared base validators into create/update validators and keep business lookups in the application service where they need repositories.
- Use AutoMapper profiles or the project’s established mapper consistently. Add explicit mappings for nested objects and avoid silently relying on fragile conventions.
- Use typed application exceptions for expected outcomes and one API-level exception handler for consistent status codes and error bodies. Do not expose internal exception details.
- Register interfaces and implementations in focused dependency-injection extension methods. Keep composition-root wiring in API/Infrastructure/Application registration methods.
- Propagate the request `CancellationToken` through public async methods and repository calls. Do not replace it with `CancellationToken.None` unless the work is deliberately independent of the request and that choice is documented.
- Use structured logging with stable event messages and identifiers; avoid secrets and sensitive data.
- Add tests at the narrowest useful level, then add integration coverage when the behavior crosses HTTP, authentication, serialization, DI, or database boundaries.

## Review changes

Check dependency direction, public API contracts, authorization, validation, error/status mapping, transaction boundaries, query tracking, nullability, cancellation propagation, logging, persistence configuration, and test coverage. Flag deviations with concrete evidence and distinguish defects from intentional project conventions.

Do not blindly reproduce known warnings or outdated APIs. During review, call out dependency vulnerabilities, obsolete framework APIs, missing tests, leaked entities, business logic in controllers, infrastructure references from Domain, and exception paths that produce inconsistent responses.

## Completion criteria

Before handing off an implementation:

1. Build the affected projects.
2. Run focused tests and the relevant broader test suite.
3. Add or update tests for success, validation failure, missing data, conflicts/authorization, and important state transitions.
4. Inspect the diff for accidental scope expansion, generated artifacts, secrets, and unrelated formatting.
5. Report warnings or pre-existing failures separately from failures caused by the change.
