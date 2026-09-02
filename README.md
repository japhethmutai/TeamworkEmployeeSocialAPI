# Teamwork

Teamwork is an internal social network for employees of an organization. The goal
of the application is to facilitate more interaction between colleagues and
promote team bonding.

Built with **.NET 8**, **PostgreSQL**, and **Clean Architecture**.

## Features

### Required

- Admin can create an employee user account.
- Admin/Employees can sign in.
- Employees can post gifs.
- Employees can write and post articles.
- Employees can edit their articles.
- Employees can delete their articles.
- Employees can delete their gif posts.
- Employees can comment on other colleagues' article posts.
- Employees can comment on other colleagues' gif posts.
- Employees can view all articles and gifs, showing the most recently posted first.
- Employees can view a specific article.
- Employees can view a specific gif post.

### Optional

- Employees can view all articles that belong to a category (tag).
- Employees can flag a comment, article, and/or gif as inappropriate.
- Admin can delete a comment, article, and/or gif flagged as inappropriate.

## Architecture

This project is intentionally built beyond what the feature list above strictly
requires, as a showcase of production-grade .NET architecture and patterns:

- **Clean Architecture** — `Domain`, `Application`, `Infrastructure`, and `API`
  projects, with dependencies enforced by project references so business logic
  stays decoupled from persistence and delivery concerns.
- **CQRS via MediatR** — each use case (command or query) is a single,
  independently testable handler; controllers stay thin.
- **EF Core + PostgreSQL** — Fluent API configuration, global query filters for
  soft deletes, and DTO projection to avoid over-fetching.
- **RBAC via Identity Core + JWT** — policy-based authorization for rules that
  go beyond a simple role check (e.g. only the author may edit their own
  article).
- **Cursor-based pagination** on the main feed, for consistent performance as
  post volume grows.
- **Audit logging** for moderation actions (e.g. an admin deleting a flagged
  post records who acted and why).
- **FluentValidation** for centralized, testable input validation.
- **Integration tests** via `WebApplicationFactory` against a real database.

## Status

Early scaffolding — solution structure is being set up. This section will be
kept up to date as the project progresses.
