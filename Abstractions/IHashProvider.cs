// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for hash providers that can hash data.
/// </summary>
public interface IHashProvider
{
	/// <summary>
	/// The length of the hash in bytes.
	/// </summary>
	public int HashLengthBytes { get; }

	/// <summary>
	/// Tries to hash the specified data into the provided hash buffer.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <param name="destination">The hash buffer to write the result to.</param>
	/// <returns>The result of the hash operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryHash(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to hash the specified data into the provided hash buffer asynchronously.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <param name="destination">The hash buffer to write the result to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the hash operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TryHashAsync(ReadOnlyMemory<byte> data, Memory<byte> destination, CancellationToken cancellationToken = default)
	{
		return cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TryHash(data.Span, destination.Span), cancellationToken);
	}

	/// <summary>
	/// Asynchronously hashes the specified data.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A byte array containing the hash of the data.</returns>
	public Task<byte[]> HashAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Hash(data.Span), cancellationToken);

	/// <summary>
	/// Hashes the specified data.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <returns>A byte array containing the hash of the data.</returns>
	public byte[] Hash(ReadOnlySpan<byte> data)
	{
		Span<byte> hash = new byte[HashLengthBytes];
		(bool success, int bytesWritten) = TryHash(data, hash);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Hashing failed to produce output with the allocated buffer.");
			}
			hash = new byte[bytesWritten];
			(success, bytesWritten) = TryHash(data, hash);
			if (!success)
			{
				throw new InvalidOperationException("Hashing failed to produce output with the allocated buffer.");
			}
		}

		return hash[..bytesWritten].ToArray();
	}
}
