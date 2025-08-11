// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for obfuscation providers that can apply reversible or one-way obfuscation.
/// </summary>
[SuppressMessage("Maintainability", "CA1510:Use ArgumentNullException throw helper", Justification = "Not available in netstandard")]
public interface IObfuscationProvider
{
	/// <summary>
	/// Tries to obfuscate the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="destination">The destination to write the obfuscated data to.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public bool TryObfuscate(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to obfuscate the data from the reader and write the result to the writer.
	/// </summary>
	/// <param name="reader">The reader to read the data from.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public bool TryObfuscate(Stream reader, Stream writer);

	/// <summary>
	/// Tries to obfuscate the data from the span and write the result to the writer.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public bool TryObfuscate(ReadOnlySpan<byte> data, Stream writer)
	{
		if (writer is null)
		{
			throw new ArgumentNullException(nameof(writer));
		}

		using MemoryStream inputStream = new(data.ToArray());  // Copy here unavoidable
		return TryObfuscate(inputStream, writer);
	}

	/// <summary>
	/// Tries to obfuscate the data from the string and write the result to the writer.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public bool TryObfuscate(string data, Stream writer)
	{
		if (writer is null)
		{
			throw new ArgumentNullException(nameof(writer));
		}

		using MemoryStream inputStream = new(Encoding.UTF8.GetBytes(data));
		return TryObfuscate(inputStream, writer);
	}

	/// <summary>
	/// Obfuscates the data from the string and returns the result.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <returns>The obfuscated data.</returns>
	public string Obfuscate(string data)
	{
		if (data is null)
		{
			throw new ArgumentNullException(nameof(data));
		}

		using MemoryStream outputStream = new();
		TryObfuscate(data, outputStream);
		return Encoding.UTF8.GetString(outputStream.ToArray());
	}

	/// <summary>
	/// Obfuscates the data from the span and returns the result.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <returns>The obfuscated data.</returns>
	public byte[] Obfuscate(ReadOnlySpan<byte> data)
	{
		using MemoryStream inputStream = new(data.ToArray());  // Copy here unavoidable
		using MemoryStream outputStream = new();
		TryObfuscate(inputStream, outputStream);
		return outputStream.ToArray();
	}

	/// <summary>
	/// Tries to obfuscate the data from the reader and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="reader">The reader to read the data from.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public Task<bool> TryObfuscateAsync(Stream reader, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryObfuscate(reader, writer), cancellationToken);

	/// <summary>
	/// Tries to obfuscate the data from the span and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public Task<bool> TryObfuscateAsync(ReadOnlyMemory<byte> data, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryObfuscate(data.Span, writer), cancellationToken);

	/// <summary>
	/// Tries to obfuscate the data from the string and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="writer">The writer to write the obfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the obfuscation was successful, false otherwise.</returns>
	public Task<bool> TryObfuscateAsync(string data, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryObfuscate(data, writer), cancellationToken);

	/// <summary>
	/// Obfuscates the data from the string and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The obfuscated data.</returns>
	public Task<string> ObfuscateAsync(string data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<string>(cancellationToken)
			: Task.Run(() => Obfuscate(data), cancellationToken);

	/// <summary>
	/// Obfuscates the data from the span and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The obfuscated data.</returns>
	public Task<byte[]> ObfuscateAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Obfuscate(data.Span), cancellationToken);

	/// <summary>
	/// Tries to deobfuscate the data from the span and write the result to the destination.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="destination">The destination to write the deobfuscated data to.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public bool TryDeobfuscate(ReadOnlySpan<byte> data, Span<byte> destination);

	/// <summary>
	/// Tries to deobfuscate the data from the reader and write the result to the writer.
	/// </summary>
	/// <param name="reader">The reader to read the data from.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public bool TryDeobfuscate(Stream reader, Stream writer);

	/// <summary>
	/// Tries to deobfuscate the data from the span and write the result to the writer.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public bool TryDeobfuscate(ReadOnlySpan<byte> data, Stream writer)
	{
		if (writer is null)
		{
			throw new ArgumentNullException(nameof(writer));
		}

		using MemoryStream inputStream = new(data.ToArray());  // Copy here unavoidable
		return TryDeobfuscate(inputStream, writer);
	}

	/// <summary>
	/// Tries to deobfuscate the data from the string and write the result to the writer.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public bool TryDeobfuscate(string data, Stream writer)
	{
		if (writer is null)
		{
			throw new ArgumentNullException(nameof(writer));
		}

		using MemoryStream inputStream = new(Encoding.UTF8.GetBytes(data));
		return TryDeobfuscate(inputStream, writer);
	}

	/// <summary>
	/// Deobfuscates the data from the string and returns the result.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <returns>The deobfuscated data.</returns>
	public string Deobfuscate(string data)
	{
		if (data is null)
		{
			throw new ArgumentNullException(nameof(data));
		}

		using MemoryStream inputStream = new(Encoding.UTF8.GetBytes(data));
		using MemoryStream outputStream = new();
		TryDeobfuscate(inputStream, outputStream);
		return Encoding.UTF8.GetString(outputStream.ToArray());
	}

	/// <summary>
	/// Deobfuscates the data from the span and returns the result.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <returns>The deobfuscated data.</returns>
	public byte[] Deobfuscate(ReadOnlySpan<byte> data)
	{
		using MemoryStream inputStream = new(data.ToArray());  // Copy here unavoidable
		using MemoryStream outputStream = new();
		TryDeobfuscate(inputStream, outputStream);
		return outputStream.ToArray();
	}

	/// <summary>
	/// Tries to deobfuscate the data from the reader and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="reader">The reader to read the data from.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public Task<bool> TryDeobfuscateAsync(Stream reader, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDeobfuscate(reader, writer), cancellationToken);

	/// <summary>
	/// Tries to deobfuscate the data from the span and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public Task<bool> TryDeobfuscateAsync(ReadOnlyMemory<byte> data, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDeobfuscate(data.Span, writer), cancellationToken);

	/// <summary>
	/// Tries to deobfuscate the data from the string and write the result to the writer asynchronously.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="writer">The writer to write the deobfuscated data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the deobfuscation was successful, false otherwise.</returns>
	public Task<bool> TryDeobfuscateAsync(string data, Stream writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TryDeobfuscate(data, writer), cancellationToken);

	/// <summary>
	/// Deobfuscates the data from the string and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The deobfuscated data.</returns>
	public Task<string> DeobfuscateAsync(string data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<string>(cancellationToken)
			: Task.Run(() => Deobfuscate(data), cancellationToken);

	/// <summary>
	/// Deobfuscates the data from the span and returns the result asynchronously.
	/// </summary>
	/// <param name="data">The data to deobfuscate.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The deobfuscated data.</returns>
	public Task<byte[]> DeobfuscateAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Deobfuscate(data.Span), cancellationToken);
}
