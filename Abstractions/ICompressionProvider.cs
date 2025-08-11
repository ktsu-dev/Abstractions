// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for compression providers that can compress and decompress data.
/// </summary>
public interface ICompressionProvider
{
	/// <summary>
	/// Compresses the specified data (synchronous, canonical input type).
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="level">The compression level (1-9, where 9 is maximum compression).</param>
	/// <returns>The compressed data.</returns>
	public byte[] Compress(ReadOnlySpan<byte> data, int level = 6);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="level">The compression level (1-9, where 9 is maximum compression).</param>
	/// <returns>The compressed data.</returns>
	public byte[] Compress(byte[] data, int level = 6) => Compress(data.AsSpan(), level);

	/// <summary>
	/// Decompresses the specified compressed data (synchronous, canonical input type).
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <returns>The decompressed data.</returns>
	public byte[] Decompress(ReadOnlySpan<byte> compressedData);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <returns>The decompressed data.</returns>
	public byte[] Decompress(byte[] compressedData) => Decompress(compressedData.AsSpan());

	/// <summary>
	/// Asynchronously compresses the specified data (canonical async input type).
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="level">The compression level (1-9, where 9 is maximum compression).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous compression operation.</returns>
	public Task<byte[]> CompressAsync(ReadOnlyMemory<byte> data, int level = 6, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Compress(data.Span, level), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="level">The compression level (1-9, where 9 is maximum compression).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous compression operation.</returns>
	public Task<byte[]> CompressAsync(byte[] data, int level = 6, CancellationToken cancellationToken = default)
		=> CompressAsync(data.AsMemory(), level, cancellationToken);

	/// <summary>
	/// Asynchronously decompresses the specified compressed data (canonical async input type).
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous decompression operation.</returns>
	public Task<byte[]> DecompressAsync(ReadOnlyMemory<byte> compressedData, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Decompress(compressedData.Span), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous decompression operation.</returns>
	public Task<byte[]> DecompressAsync(byte[] compressedData, CancellationToken cancellationToken = default)
		=> DecompressAsync(compressedData.AsMemory(), cancellationToken);

	/// <summary>
	/// Asynchronously compresses the specified stream and returns compressed bytes.
	/// Implementers may override to perform true streaming compression.
	/// </summary>
	/// <param name="data">The input stream to read.</param>
	/// <param name="level">The compression level.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The compressed data.</returns>
	public Task<byte[]> CompressAsync(Stream data, int level = 6, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously decompresses the specified stream and returns decompressed bytes.
	/// Implementers may override to perform true streaming decompression.
	/// </summary>
	/// <param name="compressedData">The compressed input stream to read.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The decompressed data.</returns>
	public Task<byte[]> DecompressAsync(Stream compressedData, CancellationToken cancellationToken = default);
}
