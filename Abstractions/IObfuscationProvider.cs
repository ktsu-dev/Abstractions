// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

/// <summary>
/// Interface for obfuscation providers that can apply reversible or one-way obfuscation.
/// </summary>
public interface IObfuscationProvider
{
	/// <summary>
	/// Obfuscates the specified data.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the obfuscation (e.g., shift, key).</param>
	/// <returns>The obfuscated data.</returns>
	public byte[] Obfuscate(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null);

	/// <summary>
	/// Deobfuscates the specified data (where applicable).
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the deobfuscation (e.g., shift, key).</param>
	/// <returns>The deobfuscated data.</returns>
	public byte[] Deobfuscate(byte[] obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null);

	/// <summary>
	/// Asynchronously obfuscates the specified data.
	/// </summary>
	/// <param name="data">The data to obfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the obfuscation (e.g., shift, key).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous obfuscation operation.</returns>
	public Task<byte[]> ObfuscateAsync(byte[] data, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously deobfuscates the specified data.
	/// </summary>
	/// <param name="obfuscatedData">The obfuscated data to deobfuscate.</param>
	/// <param name="parameters">Optional parameters controlling the deobfuscation (e.g., shift, key).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that represents the asynchronous deobfuscation operation.</returns>
	public Task<byte[]> DeobfuscateAsync(byte[] obfuscatedData, IReadOnlyDictionary<string, object?>? parameters = null, CancellationToken cancellationToken = default);
}

