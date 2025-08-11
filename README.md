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

## Interfaces overview

- **`ICompressionProvider`**
  - Sync (canonical): `byte[] Compress(ReadOnlySpan<byte> data, int level = 6)`, `byte[] Decompress(ReadOnlySpan<byte> data)`
  - Convenience: `byte[] Compress(byte[] data, int level = 6)`, `byte[] Decompress(byte[] data)`
  - Async (canonical): `Task<byte[]> CompressAsync(ReadOnlyMemory<byte> data, int level = 6, CancellationToken ct = default)`, `Task<byte[]> DecompressAsync(ReadOnlyMemory<byte> data, CancellationToken ct = default)`
  - Async convenience: `Task<byte[]> CompressAsync(byte[] data, int level = 6, CancellationToken ct = default)`, `Task<byte[]> DecompressAsync(byte[] data, CancellationToken ct = default)`
  - Streams: `Task<byte[]> CompressAsync(Stream data, int level = 6, CancellationToken ct = default)`, `Task<byte[]> DecompressAsync(Stream data, CancellationToken ct = default)`

- **`IEncryptionProvider`**
  - Sync (canonical): `byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv = default)`, `byte[] Decrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv = default)`
  - Convenience: `byte[] Encrypt(byte[] data, byte[] key, byte[]? iv = null)`, `byte[] Decrypt(byte[] data, byte[] key, byte[]? iv = null)`
  - Async (canonical): `Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv = default, CancellationToken ct = default)`, `Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv = default, CancellationToken ct = default)`
  - Async convenience: `Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[]? iv = null, CancellationToken ct = default)`, `Task<byte[]> DecryptAsync(byte[] data, byte[] key, byte[]? iv = null, CancellationToken ct = default)`
  - Key material: `byte[] GenerateKey()`, `byte[] GenerateIV()`

- **`IHashProvider`**
  - Sync (canonical): `byte[] Hash(ReadOnlySpan<byte> data)`
  - Async (canonical): `Task<byte[]> HashAsync(ReadOnlyMemory<byte> data, CancellationToken ct = default)`, `Task<byte[]> HashAsync(Stream data, CancellationToken ct = default)`
  - Convenience: `byte[] Hash(byte[] data)`, `Task<byte[]> HashAsync(byte[] data, CancellationToken ct = default)`

- **`IObfuscationProvider`**
  - Sync (canonical): `byte[] Obfuscate(ReadOnlySpan<byte> data, IReadOnlyDictionary<string, object?>? parameters = null)`, `byte[] Deobfuscate(ReadOnlySpan<byte> data, IReadOnlyDictionary<string, object?>? parameters = null)`
  - Convenience: `byte[] Obfuscate(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null)`, `byte[] Deobfuscate(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null)`
  - Async (canonical): `Task<byte[]> ObfuscateAsync(ReadOnlyMemory<byte> data, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken ct = default)`, `Task<byte[]> DeobfuscateAsync(ReadOnlyMemory<byte> data, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken ct = default)`
  - Async convenience: `Task<byte[]> ObfuscateAsync(byte[] data, ...)`, `Task<byte[]> DeobfuscateAsync(byte[] data, ...)`

- **`ISerializationProvider`**
  - Sync: `string Serialize<T>(T obj)`, `string Serialize(object obj, Type type)`, `T Deserialize<T>(string data)`, `object Deserialize(string data, Type type)`
  - Async: `Task<string> SerializeAsync<T>(T obj, CancellationToken ct = default)`, `Task<string> SerializeAsync(object obj, Type type, CancellationToken ct = default)`, `Task<T> DeserializeAsync<T>(string data, CancellationToken ct = default)`, `Task<object> DeserializeAsync(string data, Type type, CancellationToken ct = default)`

- **`IFileSystemProvider`**
  - Implements `System.IO.Abstractions.IFileSystem` for DI-friendly IO

## Design principles

- Sync methods accept ReadOnlySpan<byte> for zero-allocation callers; async methods accept ReadOnlyMemory<byte> and, where appropriate, Stream.
- Array-based overloads are provided as convenience wrappers and forward to span/memory.
- Async wrappers for CPU-bound operations may run on the thread pool; stream-based methods are provided where I/O is involved.

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
