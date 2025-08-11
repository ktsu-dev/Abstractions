// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for compression providers that can compress and decompress data.
/// </summary>
public interface ICompressionProvider
{
	/// <summary>
	/// Tries to compress the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public bool TryCompress(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to compress the data from the stream and write the result to the destination.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public bool TryCompress(Stream data, Stream destination);

	/// <summary>
	/// Tries to compress the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public bool TryCompress(ReadOnlySpan<byte> data, Stream destination)
	{
		using MemoryStream inputStream = new(data.ToArray());
		return TryCompress(inputStream, destination);
	}

	/// <summary>
	/// Compresses the data from the span and returns the result.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <returns>The compressed data.</returns>
	public byte[] Compress(ReadOnlySpan<byte> data)
	{
		using MemoryStream outputStream = new();
		if (!TryCompress(data, outputStream))
		{
			throw new InvalidOperationException("Compression failed to produce output with the allocated buffer.");
		}

		return outputStream.ToArray();
	}

	/// <summary>
	/// Compresses the data from the stream and returns the result.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <returns>The compressed data.</returns>
	public byte[] Compress(Stream data)
	{
		using MemoryStream outputStream = new();
		if (!TryCompress(data, outputStream))
		{
			throw new InvalidOperationException("Compression failed to produce output with the allocated buffer.");
		}

		return outputStream.ToArray();
	}

	/// <summary>
	/// Compresses the data from the string and returns the result.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <returns>The compressed data.</returns>
	public string Compress(string data)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		return Encoding.UTF8.GetString(Compress(bytes));
	}

	/// <summary>
	/// Tries to compress the data from the span and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public Task<bool> TryCompressAsync(ReadOnlyMemory<byte> data, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryCompress(data.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Tries to compress the data from the stream and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public Task<bool> TryCompressAsync(ReadOnlyMemory<byte> data, Stream destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryCompress(data.Span, destination), cancellationToken);

	/// <summary>
	/// Tries to compress the data from the stream and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="destination">The destination to write the compressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the compression was successful, false otherwise.</returns>
	public Task<bool> TryCompressAsync(Stream data, Stream destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryCompress(data, destination), cancellationToken);

	/// <summary>
	/// Compresses the data from the span and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The compressed data.</returns>
	public Task<byte[]> CompressAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Compress(data.Span), cancellationToken);

	/// <summary>
	/// Compresses the data from the stream and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The compressed data.</returns>
	public Task<byte[]> CompressAsync(Stream data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Compress(data), cancellationToken);

	/// <summary>
	/// Compresses the data from the string and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to compress.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The compressed data.</returns>
	public Task<string> CompressAsync(string data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<string>(cancellationToken)
			: Task.Run(() => Compress(data), cancellationToken);

	/// <summary>
	/// Tries to decompress the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <returns>True if the decompression was successful, false otherwise.</returns>
	public bool TryDecompress(ReadOnlySpan<byte> compressedData, Span<byte> destination);

	/// <summary>
	/// Tries to decompress the data from the stream and write the result to the destination.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <returns>True if the decompression was successful, false otherwise.</returns>
	public bool TryDecompress(Stream compressedData, Stream destination);

	/// <summary>
	/// Tries to decompress the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <returns>True if the decompression was successful, false otherwise.</returns>
	public bool TryDecompress(ReadOnlySpan<byte> compressedData, Stream destination)
	{
		using MemoryStream inputStream = new(compressedData.ToArray());
		return TryDecompress(inputStream, destination);
	}

	/// <summary>
	/// Decompresses the data from the span and returns the result.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <returns>The decompressed data.</returns>
	public byte[] Decompress(ReadOnlySpan<byte> compressedData)
	{
		using MemoryStream outputStream = new();
		if (!TryDecompress(compressedData, outputStream))
		{
			throw new InvalidOperationException("Decompression failed to produce output with the allocated buffer.");
		}

		return outputStream.ToArray();
	}

	/// <summary>
	/// Decompresses the data from the stream and returns the result.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <returns>The decompressed data.</returns>
	public byte[] Decompress(Stream compressedData)
	{
		using MemoryStream outputStream = new();
		if (!TryDecompress(compressedData, outputStream))
		{
			throw new InvalidOperationException("Decompression failed to produce output with the allocated buffer.");
		}

		return outputStream.ToArray();
	}

	/// <summary>
	/// Tries to decompress the data from the span and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<bool> TryDecompressAsync(ReadOnlyMemory<byte> compressedData, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDecompress(compressedData.Span, destination.Span), cancellationToken);

	/// <summary>
	/// Tries to decompress the data from the stream and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<bool> TryDecompressAsync(ReadOnlyMemory<byte> compressedData, Stream destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDecompress(compressedData.Span, destination), cancellationToken);

	/// <summary>
	/// Tries to decompress the data from the stream and write the result to the destination asynchronously.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="destination">The destination to write the decompressed data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<bool> TryDecompressAsync(Stream compressedData, Stream destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDecompress(compressedData, destination), cancellationToken);

	/// <summary>
	/// Decompresses the data from the span and returns the result asynchronously.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The decompressed data.</returns>
	public Task<byte[]> DecompressAsync(ReadOnlyMemory<byte> compressedData, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Decompress(compressedData.Span), cancellationToken);

	/// <summary>
	/// Decompresses the data from the stream and returns the result asynchronously.
	/// </summary>
	/// <param name="compressedData">The compressed data to decompress.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The decompressed data.</returns>
	public Task<byte[]> DecompressAsync(Stream compressedData, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Decompress(compressedData), cancellationToken);
}
