// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for serialization providers that can serialize and deserialize objects.
/// </summary>
public interface ISerializationProvider
{
	/// <summary>
	/// Tries to serialize the specified object into the destination buffer without allocating.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="destination">The destination buffer to write the serialized data to.</param>
	/// <returns>The result of the serialization operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public (bool Success, int BytesWritten) TrySerialize(object obj, Span<byte> destination);

	/// <summary>
	/// Tries to deserialize the specified data into an object.
	/// </summary>
	/// <param name="data">The data to deserialize.</param>
	/// <returns>The deserialized object.</returns>
	public object? Deserialize(ReadOnlySpan<byte> data);

	/// <summary>
	/// Tries to deserialize the specified data into a specific type.
	/// </summary>
	/// <typeparam name="T">The type to deserialize into.</typeparam>
	/// <param name="data">The data to deserialize.</param>
	/// <returns>The deserialized object.</returns>
	public T? Deserialize<T>(ReadOnlySpan<byte> data)
		=> (T?)Deserialize(data);

	/// <summary>
	/// Tries to serialize the specified object into the destination buffer asynchronously.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="destination">The destination buffer to write the serialized data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The result of the serialization operation. Success=false with BytesWritten set to required size when destination is too small.</returns>
	public Task<(bool Success, int BytesWritten)> TrySerializeAsync(object obj, Memory<byte> destination, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<(bool Success, int BytesWritten)>(cancellationToken)
			: Task.Run(() => TrySerialize(obj, destination.Span), cancellationToken);

	/// <summary>
	/// Tries to deserialize the specified data into an object asynchronously.
	/// </summary>
	/// <param name="data">The data to deserialize.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The deserialized object.</returns>
	public Task<object?> DeserializeAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<object?>(cancellationToken)
			: Task.Run(() => Deserialize(data.Span), cancellationToken);

	/// <summary>
	/// Tries to deserialize the specified data into a specific type asynchronously.
	/// </summary>
	/// <typeparam name="T">The type to deserialize into.</typeparam>
	/// <param name="data">The data to deserialize.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The deserialized object.</returns>
	public Task<T?> DeserializeAsync<T>(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<T?>(cancellationToken)
			: Task.Run(() => Deserialize<T>(data.Span), cancellationToken);

	/// <summary>
	/// Serializes the specified object.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>A string containing the serialized data.</returns>
	public string Serialize(object obj)
	{
		long estimatedSize = 1024;
		Span<byte> destination = new byte[estimatedSize];
		(bool success, int bytesWritten) = TrySerialize(obj, destination);
		if (!success)
		{
			if (bytesWritten <= 0)
			{
				throw new InvalidOperationException("Serialization failed to produce output with the allocated buffer.");
			}
			destination = new byte[bytesWritten];
			(success, bytesWritten) = TrySerialize(obj, destination);
			if (!success)
			{
				throw new InvalidOperationException("Serialization failed to produce output with the allocated buffer.");
			}
		}

		return Encoding.UTF8.GetString(destination[..bytesWritten]);
	}
}
