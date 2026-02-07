# CleanArch.Abstractions

[![NuGet](./build/icons/icon.png)](https://www.nuget.org/packages/CleanArch.Specification)  
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A .NET abstractions library built for **Clean Architecture**. It provides foundational interfaces and shared architecture building blocks to facilitate the development of modular, maintainable, and testable applications.

---

## 🚀 Why This Library Exists

In a Clean Architecture design, having clearly separated layers is fundamental. This package provides the core abstractions — domain interfaces, value objects, repository contracts, specification interfaces — that various parts of your system (application layer, infrastructure, domain) depend on *without introducing tight coupling*.  

Using this library helps you:

- Enforce separation of concerns  
- Define reusable repository and specification contracts  
- Keep your domain and application logic technology-agnostic  
- Build your infrastructure layer (e.g., EF, Mongo, Dapper) against stable abstractions  

---

## 📦 What’s Inside

Here are some of the main abstractions and concepts typically exposed by this package (adjust based on your actual code):

- **Domain Interfaces**: Entities, aggregates, value objects, domain events  
- **Repository Interfaces**: Generic repository contracts (`IRepository<T>`, `IReadRepository<T>`, etc.)  
- **Specification Pattern**: Specification interfaces to define reusable business rules (`ISpecification<T>`, `ISpecificationBuilder<T>`, …)  
- **Unit of Work**: If applicable, `IUnitOfWork` abstraction  
- **Domain Exceptions / Results**: Standard domain result or error representation  

---

## 🧰 Getting Started

### Prerequisites

- .NET SDK (e.g., .NET 6 / .NET 7 / .NET 10 — whatever you target)  
- A clean architecture solution (or just use this as a library in your own architecture)

### Installation via NuGet

```bash
dotnet add package CleanArch.Abstractions
```

Or via Package Manager Console:

```
Install-Package CleanArch.Abstractions
```

Including in Your Project

After installing, reference the interfaces in your Domain or Application layer. For example:

    using CleanArch.Abstractions.Repositories;
    using CleanArch.Abstractions.Specifications;

    public class MyService
    {
        private readonly IReadRepository<MyEntity> _readRepo;

        public MyService(IReadRepository<MyEntity> readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<IEnumerable<MyEntity>> GetImportantEntitiesAsync()
        {
            var spec = SpecificationBuilder<MyEntity>.New
                           .Where(e => e.IsImportant)
                           .Build();

            return await _readRepo.ListAsync(spec, cancellationToken: default);
        }
    }


🧭 Architectural Guidance

Here’s how you can integrate CleanArch.Abstractions in a typical Clean Architecture solution:

```
┌───────────────────────┐
│   Presentation Layer  │
└────────────▲──────────┘
             │
┌────────────┴──────────┐
│   Application Layer   │
└────────────▲──────────┘
             │
┌────────────┴─────────────────────┐
│  CleanArch.Abstractions  │  ← Your abstractions package
└────────────▲─────────────────────┘
             │
┌────────────┴───────────┐
│  Infrastructure Layer  │  (e.g. EF, Mongo)  
└────────────────────────┘
```
- **Application** and **Infrastructure** depend on Abstractions
- **Domain** can define its own types (entities, value objects), but relies on interfaces in Abstractions
- This keeps **Infrastructure decoupled** from specific application logic

✅ Common Use Cases

| Use Case                   | Description                                                                                                                  |
| -------------------------- | ---------------------------------------------------------------------------------------------------------------------------- |
| **Repository Abstraction** | Define `IRepository<T>` or `IReadRepository<T>` so that your app code can work independently of your data access technology. |
| **Specification Pattern**  | Use `ISpecification<T>` to describe business rules / queries in a reusable, strongly-typed way.                              |
| **Unit of Work**           | If using UoW, `IUnitOfWork` abstraction helps orchestrate transactional work across multiple repositories.                   |
| **Domain Services**        | Define service contracts (domain-level) that can be implemented in Application or Infrastructure.                            |



📚 Versioning & Releases

This library follows SemVer (e.g. Major.Minor.Patch).
New versions will be published on NuGet.org, and this README is embedded in the NuGet package so consumers can read it directly there.

Contributions are very welcome! Here’s how you can help:

1. **Fork** the repository
2. Create a new **branch**: feature/your-feature or bugfix/your-bug
3. Make your changes and add tests if applicable
4. Commit and push your branch
5. Open a Pull Request
6. Describe your changes clearly

Please follow Clean Architecture principles: keep abstractions clean, don’t introduce infrastructure-specific logic here, keep dependencies minimal.

📄 License

This project is licensed under the MIT License — see the LICENSE file for details.

💬 Issues & Feedback

If you find a bug or want a feature, please open an issue.
I’d appreciate feedback on usage patterns, suggestions, or questions.

📊 References & Resources

    NuGet package-authoring best practices
