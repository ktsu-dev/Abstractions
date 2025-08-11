---
status: draft
title: ktsu.Abstractions
description: A library providing a consistent set of interfaces for compression, encryption, hashing, obfuscation, serialization, and filesystem access.
tags:
  - abstractions
  - .net
  - csharp
  - provider pattern
  - dependency injection
  - serialization
  - compression
  - encryption
  - hashing
  - obfuscation
---

# ktsu.Abstractions

A small, focused library of interfaces that define a consistent API for common cross-cutting concerns:

- **Compression**: `ICompressionProvider`
- **Encryption**: `IEncryptionProvider`
- **Hashing**: `IHashProvider`
- **Obfuscation**: `IObfuscationProvider`
- **Serialization**: `ISerializationProvider`
- **Filesystem**: `IFileSystemProvider`

Designed for framework-agnostic usage and easy dependency injection.

## Target frameworks

This package multi-targets common frameworks for broad compatibility:

- netstandard2.0, netstandard2.1
- net5.0, net6.0, net7.0, net8.0, net9.0

Supported OS: Windows, Linux, macOS.

## Install

Via dotnet CLI:

```bash
dotnet add package ktsu.Abstractions
```

Via NuGet Package Manager:

```powershell
Install-Package ktsu.Abstractions
```

Via PackageReference in csproj:

```xml
<ItemGroup>
    <PackageReference Include="ktsu.Abstractions" Version="1.0.0" />
</ItemGroup>
```

## Quickstart

Using the implementations from the `ktsu.Common` package via DI:

```csharp
using ktsu.Abstractions;
using ktsu.Common;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.AddTransient<ICompressionProvider, ktsu.Common.GZipCompressionProvider>();
services.AddTransient<IEncryptionProvider, ktsu.Common.AesEncryptionProvider>();
services.AddTransient<IHashProvider, ktsu.Common.Sha256HashProvider>();
services.AddTransient<IObfuscationProvider, ktsu.Common.Base64ObfuscationProvider>();
services.AddTransient<ISerializationProvider, ktsu.Common.JsonSerializationProvider>();
services.AddTransient<IFileSystemProvider, ktsu.Common.FileSystemProvider>();

using IServiceProvider provider = services.BuildServiceProvider();

ICompressionProvider compressionProvider = provider.GetRequiredService<ICompressionProvider>();
IEncryptionProvider encryptionProvider = provider.GetRequiredService<IEncryptionProvider>();
IHashProvider hashProvider = provider.GetRequiredService<IHashProvider>();

```

## Implementing your own providers

Implement the interfaces in your application or infrastructure layer and register them with your DI container.

For example, if you want to implement a custom JSON serialization provider, you can do the following:

```csharp
using System.Text.Json;
using ktsu.Abstractions;

public sealed class MyJsonSerializationProvider : ISerializationProvider
{
    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj);
    public string Serialize(object obj, Type type) => JsonSerializer.Serialize(obj, type);
    public T Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data)!;
    public object Deserialize(string data, Type type) => JsonSerializer.Deserialize(data, type)!;
    // Note: Async methods are inherited via default interface implementations.
}
```

```csharp
using ktsu.Abstractions;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.AddTransient<ISerializationProvider, MyJsonSerializationProvider>();

using IServiceProvider provider = services.BuildServiceProvider();
ISerializationProvider serializer = provider.GetRequiredService<ISerializationProvider>();

string json = serializer.Serialize(new { Message = "Hello" });
string jsonAsync = await serializer.SerializeAsync(new { Message = "Hello" });
```
## Design principles

- Canonical APIs are allocation-free Try methods that write to caller-provided destinations and return `(Success, BytesWritten)`.
- Convenience methods allocate a buffer based on the estimated size of the data, call Try*, retry if needed, and trim to `BytesWritten`.

## Security notes

- **Obfuscation** in `IObfuscationProvider` is not cryptography. Use it only for non-security scenarios (e.g., casual hiding). For confidentiality and integrity, use strong, vetted cryptography via `IEncryptionProvider` implementations.
- Implementations of `IEncryptionProvider` should rely on proven libraries (e.g., .NET BCL crypto, libsodium bindings) and follow best practices (AEAD modes, random IVs/nonces, key management).

## Thread-safety and lifetime

- Provider implementations should be stateless or internally synchronized and safe to register as singletons in DI.
- If an implementation maintains internal mutable state, document the concurrency model and recommended lifetime.

## Why use these abstractions?

- **Consistency**: A single, predictable surface across implementations
- **Testability**: Swap implementations and mock easily, especially for filesystem
- **Separation of concerns**: Keep app code free of vendor-specific details

## Contributing

- Fork the repo and create a feature branch
- Implement or refine providers/analyzers and add tests
- Open a pull request

## License

Licensed under the MIT License. See [LICENSE.md](LICENSE.md) for details.
