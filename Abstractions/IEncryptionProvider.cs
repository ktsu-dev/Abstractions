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
	/// Generates a new encryption key.
	/// </summary>
	/// <returns>A new encryption key.</returns>
	public byte[] GenerateKey();

	/// <summary>
	/// Generates a new initialization vector.
	/// </summary>
	/// <returns>A new initialization vector.</returns>
	public byte[] GenerateIV();

	/// <summary>
	/// Tries to encrypt data into the destination buffer without allocating.
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <param name="destination">The destination buffer to write the encrypted data to.</param>
	/// <returns>The result of the encryption operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryEncrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, Span<byte> destination);

	/// <summary>
	/// Tries to decrypt data into the destination buffer without allocating.
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <param name="destination">The destination buffer to write the decrypted data to.</param>
	/// <returns>The result of the decryption operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryDecrypt(ReadOnlySpan<byte> encryptedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv, Span<byte> destination);

	/// <summary>
	/// Tries to encrypt data asynchronously into the destination buffer.
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <param name="destination">The destination buffer to write the encrypted data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the encryption operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TryEncryptAsync(ReadOnlyMemory<byte> data, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TryEncrypt(data.Span, key.Span, iv.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Tries to decrypt data asynchronously into the destination buffer.
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <param name="destination">The destination buffer to write the decrypted data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the decryption operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TryDecryptAsync(ReadOnlyMemory<byte> encryptedData, ReadOnlyMemory<byte> key, ReadOnlyMemory<byte> iv, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TryDecrypt(encryptedData.Span, key.Span, iv.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Encrypts the specified data.
	/// </summary>
	/// <param name="data">The data to encrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <returns>A byte array containing the encrypted data.</returns>
	public byte[] Encrypt(ReadOnlySpan<byte> data, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv)
	{
		long estimatedSize = data.Length * 2;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryEncrypt(data, key, iv, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Encryption failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryEncrypt(data, key, iv, destination);
			if (!success)
			{
				throw new InvalidOperationException("Encryption failed to produce output with the allocated buffer.");
			}
		}

		return destination[..bytesWritten].ToArray();
	}

	/// <summary>
	/// Decrypts the specified encrypted data.
	/// </summary>
	/// <param name="encryptedData">The encrypted data to decrypt.</param>
	/// <param name="key">The encryption key.</param>
	/// <param name="iv">The initialization vector.</param>
	/// <returns>A byte array containing the decrypted data.</returns>
	public byte[] Decrypt(ReadOnlySpan<byte> encryptedData, ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv)
	{
		long estimatedSize = encryptedData.Length * 2;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryDecrypt(encryptedData, key, iv, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Decryption failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryDecrypt(encryptedData, key, iv, destination);
			if (!success)
			{
				throw new InvalidOperationException("Decryption failed to produce output with the allocated buffer.");
			}
		}

		return destination[..bytesWritten].ToArray();
	}
}
