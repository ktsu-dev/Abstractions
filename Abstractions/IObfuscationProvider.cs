// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for obfuscation providers that can apply reversible or one-way obfuscation.
/// </summary>
public interface IObfuscationProvider
{
	/// <summary>
	/// Tries to obfuscate data into the destination buffer without allocating.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="destination">The destination buffer to write the obfuscated data to.</param>
	/// <returns>The result of the obfuscation operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryObfuscate(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to deobfuscate data into the destination buffer without allocating.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="destination">The destination buffer to write the deobfuscated data to.</param>
	/// <returns>The result of the deobfuscation operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryDeobfuscate(ReadOnlySpan<byte> obfuscatedData, Span<byte> destination);

	/// <summary>
	/// Asynchronously obfuscates the specified data
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A byte array containing the obfuscated data.</returns>
	public Task<byte[]> ObfuscateAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Obfuscate(data.Span), cancellationToken);

	/// <summary>
	/// Asynchronously deobfuscates the specified data.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A byte array containing the deobfuscated data.</returns>
	public Task<byte[]> DeobfuscateAsync(ReadOnlyMemory<byte> obfuscatedData, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Deobfuscate(obfuscatedData.Span), cancellationToken);

	/// <summary>
	/// Obfuscates the specified data.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <returns>A byte array containing the obfuscated data.</returns>
	public byte[] Obfuscate(ReadOnlySpan<byte> data)
	{
		long estimatedSize = data.Length * 2;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryObfuscate(data, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Obfuscation failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryObfuscate(data, destination);
			if (!success)
			{
				throw new InvalidOperationException("Obfuscation failed to produce output with the allocated buffer.");
			}
		}

		return destination[..bytesWritten].ToArray();
	}

	/// <summary>
	/// Deobfuscates the specified obfuscated data.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <returns>A byte array containing the deobfuscated data.</returns>
	public byte[] Deobfuscate(ReadOnlySpan<byte> obfuscatedData)
	{
		long estimatedSize = obfuscatedData.Length * 2;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryDeobfuscate(obfuscatedData, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Deobfuscation failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryDeobfuscate(obfuscatedData, destination);
			if (!success)
			{
				throw new InvalidOperationException("Deobfuscation failed to produce output with the allocated buffer.");
			}
		}

		return destination[..bytesWritten].ToArray();
	}
}
