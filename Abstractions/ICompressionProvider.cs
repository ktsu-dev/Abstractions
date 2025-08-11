// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for compression providers that can compress and decompress data.
/// </summary>
public interface ICompressionProvider
{
	/// <summary>
	/// Tries to compress the specified data into the provided destination buffer.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination buffer to write the compressed data to.</param>
	/// <returns>The result of the compression operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryCompress(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to decompress the specified compressed data into the provided destination buffer.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination buffer to write the decompressed data to.</param>
	/// <returns>The result of the decompression operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TryDecompress(ReadOnlySpan<byte> compressedData, Span<byte> destination);

	/// <summary>
	/// Asynchronously tries to compress the specified data into the provided destination buffer.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination buffer to write the compressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the compression operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TryCompressAsync(ReadOnlyMemory<byte> data, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TryCompress(data.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Asynchronously tries to decompress the specified compressed data into the provided destination buffer.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination buffer to write the decompressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the decompression operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TryDecompressAsync(ReadOnlyMemory<byte> compressedData, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TryDecompress(compressedData.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Compresses the specified data.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <returns>A byte array containing the compressed data.</returns>
	public byte[] Compress(ReadOnlySpan<byte> data)
	{
		long estimatedSize = data.Length * 1;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryCompress(data, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Compression failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryCompress(data, destination);
			if (!success)
			{
				throw new InvalidOperationException("Compression failed to produce output with the allocated buffer.");
			}
		}

		return destination[..bytesWritten].ToArray();
	}

	/// <summary>
	/// Decompresses the specified compressed data.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <returns>A byte array containing the decompressed data.</returns>
	public byte[] Decompress(ReadOnlySpan<byte> compressedData)
	{
		long estimatedSize = compressedData.Length * 2;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TryDecompress(compressedData, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Decompression failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TryDecompress(compressedData, destination);
			if (!success)
			{
				throw new InvalidOperationException("Decompression failed to produce output with the allocated buffer.");
			}
		}
		return destination[..bytesWritten].ToArray();
	}
}
