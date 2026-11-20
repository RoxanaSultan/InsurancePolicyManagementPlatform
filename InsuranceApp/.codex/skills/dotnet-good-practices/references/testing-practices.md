# Testing practices

Choose the narrowest test level that proves the behavior, then add broader coverage when integration points are part of the risk.

## Validator tests

Instantiate validators directly and test invalid boundaries plus a valid model. Cover required values, ranges, date relationships, decimal precision, IDs, conditional rules, and composed base-validator rules. Assert both invalidity and the relevant property/error message when the message is part of the contract.

Keep reusable valid models and edge-case variations in focused test-data helpers. Use descriptive names such as `PolicyBaseValidator_WhenEndDateBeforeStartDate_ShouldFail`.

## Application-service tests

Mock repository and unit-of-work interfaces; do not use a real database for orchestration tests. Use fixtures to centralize strict mocks and valid entities. Follow Arrange/Act/Assert.

Cover:

- successful create, update, read, activation, cancellation, and search flows;
- missing entities and expected application exceptions;
- duplicate or conflicting values;
- unauthorized/current-user behavior;
- invalid state transitions;
- mapping of returned entities to DTOs;
- correct repository calls, save count, and cancellation-token propagation;
- negative interaction assertions proving persistence does not occur after a failed precondition.

Use strict mocks where they make missing interactions visible. Verify behavior that matters to the use case, not every incidental implementation call.

## Infrastructure tests

Use an isolated EF Core test context per test or fixture. Test generic repository CRUD/paging and feature repository query behavior, including filters, eager-loaded relationships, ordering, no-tracking reads, uniqueness checks, and cancellation where relevant.

Seed only the entities needed for the scenario. Keep database setup in a factory such as `RepositoryTestDbFactory`, and avoid depending on production data or a developer’s local SQL Server.

## Integration tests

Use `WebApplicationFactory` to exercise the real API composition root, routing, model binding, validation, middleware, serialization, authentication, DI, and database wiring. Replace external persistence with an isolated in-memory/test database when appropriate, configure deterministic JWT/test authentication, and seed through the test host.

Assert HTTP status codes and important response shape/data. Cover successful endpoints and contract failures such as validation errors, not-found, conflict, unauthorized/forbidden, and invalid state transitions. Keep test data helpers explicit and deterministic.

## Test execution and review

Run focused tests first, then the affected project suite, then integration tests when API or infrastructure behavior changed. Treat build warnings and pre-existing failures as separate findings. A change is incomplete when it adds production behavior without a test at the appropriate level or when tests only verify the happy path for failure-prone business rules.
