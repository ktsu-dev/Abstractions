// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for hash providers that can hash data.
/// </summary>
public interface IHashProvider
{
	/// <summary>
	/// Hashes the specified data (synchronous, canonical input type).
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <returns>The binary hash of the data.</returns>
	public byte[] Hash(ReadOnlySpan<byte> data);

	/// <summary>
	/// Asynchronously hashes the specified data (canonical async input type).
	/// Default implementation wraps the synchronous <see cref="Hash(ReadOnlySpan{byte})"/>.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The binary hash of the data.</returns>
	public Task<byte[]> HashAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Hash(data.Span), cancellationToken);

	/// <summary>
	/// Asynchronously hashes data from a stream. Implementers should read the stream and compute the hash.
	/// </summary>
	/// <param name="data">The stream to read and hash.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The binary hash of the stream contents.</returns>
	public Task<byte[]> HashAsync(Stream data, CancellationToken cancellationToken = default);


	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <returns>The binary hash of the data.</returns>
	public byte[] Hash(byte[] data) => Hash(data.AsSpan());

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="data">The data to hash.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The binary hash of the data.</returns>
	public Task<byte[]> HashAsync(byte[] data, CancellationToken cancellationToken = default)
		=> HashAsync(data.AsMemory(), cancellationToken);
}
