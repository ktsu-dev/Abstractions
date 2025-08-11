// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for serialization providers that can serialize and deserialize objects.
/// </summary>
public interface ISerializationProvider
{
	/// <summary>
	/// Serializes an object to its string representation.
	/// </summary>
	/// <typeparam name="T">The type of object to serialize.</typeparam>
	/// <param name="obj">The object to serialize.</param>
	/// <returns>The serialized string representation of the object.</returns>
	public string Serialize<T>(T obj);

	/// <summary>
	/// Serializes an object to its string representation using the specified type.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="type">The type to use for serialization.</param>
	/// <returns>The serialized string representation of the object.</returns>
	public string Serialize(object obj, Type type);

	/// <summary>
	/// Deserializes a string to an object of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to deserialize to.</typeparam>
	/// <param name="data">The serialized string data.</param>
	/// <returns>The deserialized object.</returns>
	public T Deserialize<T>(string data);

	/// <summary>
	/// Deserializes a string to an object of the specified type.
	/// </summary>
	/// <param name="data">The serialized string data.</param>
	/// <param name="type">The type to deserialize to.</param>
	/// <returns>The deserialized object.</returns>
	public object Deserialize(string data, Type type);

	/// <summary>
	/// Asynchronously serializes an object to its string representation.
	/// </summary>
	/// <typeparam name="T">The type of object to serialize.</typeparam>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous serialization operation.</returns>
	public Task<string> SerializeAsync<T>(T obj, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<string>(cancellationToken)
			: Task.Run(() => Serialize(obj), cancellationToken);

	/// <summary>
	/// Asynchronously serializes an object to its string representation using the specified type.
	/// </summary>
	/// <param name="obj">The object to serialize.</param>
	/// <param name="type">The type to use for serialization.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous serialization operation.</returns>
	public Task<string> SerializeAsync(object obj, Type type, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<string>(cancellationToken)
			: Task.Run(() => Serialize(obj, type), cancellationToken);

	/// <summary>
	/// Asynchronously deserializes a string to an object of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to deserialize to.</typeparam>
	/// <param name="data">The serialized string data.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous deserialization operation.</returns>
	public Task<T> DeserializeAsync<T>(string data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<T>(cancellationToken)
			: Task.Run(() => Deserialize<T>(data), cancellationToken);

	/// <summary>
	/// Asynchronously deserializes a string to an object of the specified type.
	/// </summary>
	/// <param name="data">The serialized string data.</param>
	/// <param name="type">The type to deserialize to.</param>
	/// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
	/// <returns>A task that represents the asynchronous deserialization operation.</returns>
	public Task<object> DeserializeAsync(string data, Type type, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<object>(cancellationToken)
			: Task.Run(() => Deserialize(data, type), cancellationToken);
}
