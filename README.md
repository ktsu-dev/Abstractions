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

## ktsu.Abstractions

A small, focused library of interfaces that define a consistent API for common cross-cutting concerns:

- **Compression**: `ICompressionProvider`
- **Encryption**: `IEncryptionProvider`
- **Hashing**: `IHashProvider`
- **Obfuscation**: `IObfuscationProvider`
- **Serialization**: `ISerializationProvider`
- **Filesystem**: `IFileSystemProvider` (extends `System.IO.Abstractions.IFileSystem` for testable IO)

Designed for framework-agnostic usage and easy dependency injection. Ships with an analyzer that enforces consistent enum ordering across the codebase.

### Install

```bash
dotnet add package ktsu.Abstractions
```

Or add a project reference if you keep the interfaces in a shared solution.

### Quickstart

Implement the interfaces in your application or infrastructure layer and register them with your DI container.

```csharp
using System.Text.Json;
using ktsu.Abstractions;

public sealed class JsonSerializationProvider : ISerializationProvider
{
    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj);
    public string Serialize(object obj, Type type) => JsonSerializer.Serialize(obj, type);
    public T Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data)!;
    public object Deserialize(string data, Type type) => JsonSerializer.Deserialize(data, type)!;

    public Task<string> SerializeAsync<T>(T obj, CancellationToken cancellationToken = default)
        => Task.FromResult(Serialize(obj));
    public Task<string> SerializeAsync(object obj, Type type, CancellationToken cancellationToken = default)
        => Task.FromResult(Serialize(obj, type));
    public Task<T> DeserializeAsync<T>(string data, CancellationToken cancellationToken = default)
        => Task.FromResult(Deserialize<T>(data));
    public Task<object> DeserializeAsync(string data, Type type, CancellationToken cancellationToken = default)
        => Task.FromResult(Deserialize(data, type));
}
```

Register with DI (example shown with Microsoft.Extensions.DependencyInjection):

```csharp
using ktsu.Abstractions;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();
services.AddSingleton<ISerializationProvider, JsonSerializationProvider>();

IServiceProvider provider = services.BuildServiceProvider();
ISerializationProvider serializer = provider.GetRequiredService<ISerializationProvider>();

string json = serializer.Serialize(new { Message = "Hello" });
```

For file system access, depend on `IFileSystemProvider` to get testable IO via `System.IO.Abstractions`:

```csharp
using ktsu.Abstractions;
using System.IO.Abstractions;

public sealed class MyService
{
    private readonly IFileSystemProvider fileSystem;
    public MyService(IFileSystemProvider fileSystem) => this.fileSystem = fileSystem;

    public void WriteText(string path, string content)
    {
        fileSystem.File.WriteAllText(path, content);
    }
}
```

### Interfaces overview

- **`ICompressionProvider`**: `Compress`, `Decompress`, `CompressAsync`, `DecompressAsync`
- **`IEncryptionProvider`**: `Encrypt`, `Decrypt`, `EncryptAsync`, `DecryptAsync`, `GenerateKey`, `GenerateIV`
- **`IHashProvider`**: hashing abstraction surface (implementations define concrete algorithms)
- **`IObfuscationProvider`**: `Obfuscate`, `Deobfuscate`, async counterparts, with optional parameters
- **`ISerializationProvider`**: `Serialize`/`Deserialize` for generic and non-generic, sync/async
- **`IFileSystemProvider`**: `System.IO.Abstractions.IFileSystem` implementation for DI-friendly IO

Related enums for common options are provided in `ktsu.Abstractions.Models`:

- **Compression**: `CompressionType`
- **Encryption**: `EncryptionType`
- **Hashing**: `HashType`
- **Obfuscation**: `ObfuscationType`
- **Filesystem**: `FileSystemType`

### Analyzer

An included analyzer enforces enum ordering across the codebase:

- **Diagnostic ID**: `KTSU001`
- **Rule**: If present, `None = 0` must be declared first; remaining members must be in alphabetical order, without explicit numeric values.

### Why use these abstractions?

- **Consistency**: A single, predictable surface across implementations
- **Testability**: Swap implementations and mock easily, especially for filesystem
- **Separation of concerns**: Keep app code free of vendor-specific details

### Contributing

- Fork the repo and create a feature branch
- Implement or refine providers/analyzers and add tests
- Open a pull request

### Links

- Project: [`ktsu-dev/Abstractions`](https://github.com/ktsu-dev/Abstractions)
- License: see `LICENSE.md`
- Changelog: see `CHANGELOG.md` and `LATEST_CHANGELOG.md`
- Authors: see `AUTHORS.md`


