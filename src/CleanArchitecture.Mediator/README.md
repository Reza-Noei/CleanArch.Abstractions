# CleanArchitecture.Mediator

`CleanArchitecture.Mediator` is a lightweight, extensible mediator library designed specifically for Clean Architecture, Vertical Slice Architecture, and modular monolith applications.

It provides a strongly structured request/response messaging workflow with support for:

- **Commands**
- **Queries**
- **Notifications**
- **Pipeline Behaviors**
- **Exception Mapping**
- **Automatic Discovery**

The goal of the library is to reduce complexity and boilerplate while staying fully aligned with clean, decoupled application design.

---

## ✨ Features

- 📨 **Simple**, **expressive** mediator pattern

- 📚 **Typed command**, **query**, and **notification handlers**

- 🔄 **Pipeline behaviors** for **cross-cutting** concerns

- 🤝 Strict separation between **Application** layer and **Infrastructure**

- 🧭 **Convention-based** assembly scanning

- 🚀 **Zero** external dependencies (pure .NET)

- 🧪 **Test-friendly** design with handler isolation

Perfect for building scalable API backends, clean services, modular monoliths, and enterprise-grade architectures.

---

## 📦 Installation

```bash
dotnet add package CleanArchitecture.Mediator
```

---

## 🚀 Quick Start

### 1. Define a Request (Command or Query)

#### Command Example

```CSharp
public sealed record CreateUserCommand(string Name, string Email)
    : IRequest<UserDto>;
```

#### Query Example

```CSharp
public sealed record GetUserByIdQuery(Guid Id)
    : IRequest<UserDto>;
```

### 2. Create a Handler

#### Command Handler

```CSharp 
public sealed class CreateUserHandler 
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // Business logic
        return Task.FromResult(new UserDto(request.Name, request.Email));
    }
}
```

#### Query Handler

```CSharp
public sealed class GetUserByIdHandler 
    : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        return Task.FromResult(new UserDto("John", "john@example.com"));
    }
}
```

### 3. Register Mediator

```CSharp
builder.Services.AddMediator(options =>
{
    options.AddApplicationAssembly(typeof(CreateUserCommand).Assembly);
});
```

### 4. Use Mediator Anywhere
```CSharp
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("users")]
    public async Task<IResult> Create(CreateUserCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return Results.Ok(result);
    }
}
```
---

## 🔄 Pipeline Behaviors

**Pipeline behaviors** run before and after every request handler.
Perfect for logging, validation, transactions, performance metrics, etc.

### Example Behavior
```CSharp 
public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        Console.WriteLine($"Handling {typeof(TRequest).Name}");
        
        var response = await next();
        
        Console.WriteLine($"Handled {typeof(TRequest).Name}");
        
        return response;
    }
}
```

### Register Behavior
```CSharp
builder.Services.AddMediator(options =>
{
    options.AddBehavior(typeof(LoggingBehavior<,>));
});
```

---

## 📣 Notifications & Handlers (Publish/Subscribe)
### Notification
```CSharp
public sealed record UserCreatedNotification(Guid UserId): INotification;
```

### Handler
```CSharp
public class SendWelcomeEmailHandler: INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken ct)
    {
        Console.WriteLine($"Sending welcome email to user {notification.UserId}");
        return Task.CompletedTask;
    }
}
```

### Publishing
```CSharp
await _mediator.Publish(new UserCreatedNotification(id));
```

--- 

## 🔧 Exception Mapping (Optional)

You can map domain/application exceptions to more meaningful structured results.

```CSharp 
public class UserNotFoundException : Exception;

public class UserNotFoundExceptionMapper: IExceptionMapper<UserNotFoundException>
{
    public object Map(UserNotFoundException ex)
    {
        return new { error = "User not found" };
    }
}
```

---

## 🧭 Assembly Scanning

Handlers, behaviors, and exception mappers are automatically discovered by adding assemblies:

```CSharp
options.AddApplicationAssembly(typeof(Program).Assembly);
```

---

## 📁 Suggested Folder Structure (Vertical Slice)
```bash
src/
 ├── Users/
 │    ├── CreateUser/
 │    │     ├── CreateUserCommand.cs
 │    │     ├── CreateUserHandler.cs
 │    ├── GetUserById/
 │    │     ├── GetUserByIdQuery.cs
 │    │     ├── GetUserByIdHandler.cs
 ├── Shared/
 │    ├── Behaviors/
 │    ├── Exceptions/
 ```
 ---

## 🧪 Testing Handlers

Handlers are just small, pure classes — very easy to test:

```CSharp
var handler = new CreateUserHandler();
var result = await handler.Handle(new CreateUserCommand("Reza", "reza@mail.com"), default);

Assert.Equal("Reza", result.Name);
```

No mediator container required.

---

## 🤝 Contributing

Pull requests and discussions are welcome!  
Please ensure new features include tests and follow Clean Architecture best practices.

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](../../LICENSE) file for details.
