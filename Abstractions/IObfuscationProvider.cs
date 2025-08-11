// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for obfuscation providers that can apply reversible or one-way obfuscation.
/// </summary>
public interface IObfuscationProvider
{
	/// <summary>
	/// Obfuscates the specified data (synchronous, canonical input type).
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the obfuscation (e.g., shift, key).</param>
	/// <returns>The obfuscated data.</returns>
	public byte[] Obfuscate(ReadOnlySpan<byte> data, IReadOnlyDictionary<string, object?>? parameters = null);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters.</param>
	/// <returns>The obfuscated data.</returns>
	public byte[] Obfuscate(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null) => Obfuscate(data.AsSpan(), parameters);

	/// <summary>
	/// Deobfuscates the specified data (where applicable; synchronous, canonical input type).
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the deobfuscation (e.g., shift, key).</param>
	/// <returns>The deobfuscated data.</returns>
	public byte[] Deobfuscate(ReadOnlySpan<byte> obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null);

	/// <summary>
	/// Convenience overload that forwards to the canonical span-based method.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters.</param>
	/// <returns>The deobfuscated data.</returns>
	public byte[] Deobfuscate(byte[] obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null) => Deobfuscate(obfuscatedData.AsSpan(), parameters);

	/// <summary>
	/// Asynchronously obfuscates the specified data (canonical async input type).
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the obfuscation (e.g., shift, key).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous obfuscation operation.</returns>
	public Task<byte[]> ObfuscateAsync(ReadOnlyMemory<byte> data, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Obfuscate(data.Span, parameters), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous obfuscation operation.</returns>
	public Task<byte[]> ObfuscateAsync(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default)
		=> ObfuscateAsync(data.AsMemory(), parameters, cancellationToken);

	/// <summary>
	/// Asynchronously deobfuscates the specified data (canonical async input type).
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the deobfuscation (e.g., shift, key).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous deobfuscation operation.</returns>
	public Task<byte[]> DeobfuscateAsync(ReadOnlyMemory<byte> obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default)
		=> cancellationToken.IsCancellationRequested
			? Task.FromCanceled<byte[]>(cancellationToken)
			: Task.Run(() => Deobfuscate(obfuscatedData.Span, parameters), cancellationToken);

	/// <summary>
	/// Convenience overload that forwards to the canonical memory-based async method.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous deobfuscation operation.</returns>
	public Task<byte[]> DeobfuscateAsync(byte[] obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default)
		=> DeobfuscateAsync(obfuscatedData.AsMemory(), parameters, cancellationToken);
}
