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
[SuppressMessage("Maintainability", "CA1510:Use ArgumentNullException throw helper", Justification = "Not available in netstandard")]
public interface ISerializationProvider
{
	/// <summary>
	/// Tries to serialize the specified object into the destination buffer without allocating.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="writer">The writer to write the serialized data to.</param>
	/// <returns>True if the serialization was successful, false otherwise.</returns>
	public bool TrySerialize(object obj, TextWriter writer);

	/// <summary>
	/// Tries to deserialize the specified data into an object.
	/// </summary>
	/// <param name="data">The data to deserialize.</param>
	/// <returns>The deserialized object.</returns>
	public object? Deserialize(ReadOnlySpan<byte> data);

	/// <summary>
	/// Tries to deserialize the specified data into an object from a text reader.
	/// </summary>
	/// <param name="reader">The reader to read the serialized data from.</param>
	/// <returns>The deserialized object.</returns>
	/// <remarks>The default implementation reads all of the data from the reader and passes it to the <see cref="Deserialize(ReadOnlySpan{byte})"/> method.</remarks>
	public object? Deserialize(TextReader reader)
	{
		if (reader is null)
		{
			throw new ArgumentNullException(nameof(reader));
		}

		string data = reader.ReadToEnd();
		byte[] bytes = Encoding.UTF8.GetBytes(data);
		return Deserialize(bytes);
	}

	/// <summary>
	/// Tries to deserialize the specified data into a specific type.
	/// </summary>
	/// <typeparam name="T">The type to deserialize into.</typeparam>
	/// <param name="data">The data to deserialize.</param>
	/// <returns>The deserialized object.</returns>
	public T? Deserialize<T>(ReadOnlySpan<byte> data)
		=> (T?)Deserialize(data);

	/// <summary>
	/// Tries to deserialize the specified data into a specific type from a text reader.
	/// </summary>
	/// <typeparam name="T">The type to deserialize into.</typeparam>
	/// <param name="reader">The reader to read the serialized data from.</param>
	/// <returns>The deserialized object.</returns>
	/// <remarks>The default implementation reads all of the data from the reader and passes it to the <see cref="Deserialize{T}(ReadOnlySpan{byte})"/> method.</remarks>
	public T? Deserialize<T>(TextReader reader)
		=> (T?)Deserialize(reader);

	/// <summary>
	/// Tries to serialize the specified object into the destination buffer asynchronously.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="writer">The writer to write the serialized data to.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>True if the serialization was successful, false otherwise.</returns>
	public Task<bool> TrySerializeAsync(object obj, TextWriter writer, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<bool>(cancellationToken)
			: Task.Run(() => TrySerialize(obj, writer), cancellationToken);

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
	/// Tries to deserialize the specified data into a specific type asynchronously from a text reader.
	/// </summary>
	/// <typeparam name="T">The type to deserialize into.</typeparam>
	/// <param name="reader">The reader to read the serialized data from.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The deserialized object.</returns>
	public Task<T?> DeserializeAsync<T>(TextReader reader, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<T?>(cancellationToken)
			: Task.Run(() => Deserialize<T>(reader), cancellationToken);

	/// <summary>
	/// Serializes the specified object.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>A string containing the serialized data.</returns>
	public string Serialize(object obj)
	{
		using StringWriter writer = new();
		TrySerialize(obj, writer);
		return writer.ToString();
	}

	/// <summary>
	/// Serializes the specified object.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="writer">The writer to write the serialized data to.</param>
	public void Serialize(object obj, TextWriter writer)
		=> TrySerialize(obj, writer);
}
