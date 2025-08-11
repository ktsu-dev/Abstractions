// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for encryption providers that can encrypt and decrypt data.
/// </summary>
public interface IEncryptionProvider
{
	/// <summary>
	/// Encrypts the specified data (synchronous, canonical input type).
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <returns>The encrypted data.</returns>
	public byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv = default);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <returns>The encrypted data.</returns>
	public byte[] Encrypt(byte[] data, byte[] key, byte[]? iv = null)
		=> Encrypt(data.AsSpan(), key.AsSpan(), iv is null ? ReadOnlySpan<byte>.Empty : iv.AsSpan());

	/// <summary>
	/// Decrypts the specified encrypted data (synchronous, canonical input type).
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The decryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <returns>The decrypted data.</returns>
	public byte[] Decrypt(ReadOnlySpan<byte> encryptedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv = default);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The decryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <returns>The decrypted data.</returns>
	public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[]? iv = null)
		=> Decrypt(encryptedData.AsSpan(), key.AsSpan(), iv is null ? ReadOnlySpan<byte>.Empty : iv.AsSpan());

	/// <summary>
	/// Asynchronously encrypts the specified data (canonical async input type).
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous encryption operation.</returns>
	public Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv = default, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Encrypt(data.Span, key.Span, iv.Span), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous encryption operation.</returns>
	public Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[]? iv = null, CancellationToken cancellationToken = default)
		=> EncryptAsync(data.AsMemory(), key.AsMemory(), iv is null ? ReadOnlyMemory<byte>.Empty : iv.AsMemory(), cancellationToken);

	/// <summary>
	/// Asynchronously decrypts the specified encrypted data (canonical async input type).
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The decryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous decryption operation.</returns>
	public Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> encryptedData, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv = default, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Decrypt(encryptedData.Span, key.Span, iv.Span), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The decryption key.</param>
	/// <param name="iv">The initialization vector (optional).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous decryption operation.</returns>
	public Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[]? iv = null, CancellationToken cancellationToken = default)
		=> DecryptAsync(encryptedData.AsMemory(), key.AsMemory(), iv is null ? ReadOnlyMemory<byte>.Empty : iv.AsMemory(), cancellationToken);

	/// <summary>
	/// Generates a new encryption key.
	/// </summary>
	/// <returns>A new encryption key.</returns>
	public byte[] GenerateKey();

	/// <summary>
	/// Generates a new initialization vector.
	/// </summary>
	/// <returns>A new initialization vector.</returns>
	public byte[] GenerateIV();
}
