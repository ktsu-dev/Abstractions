// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions.Models;

/// <summary>
/// Specifies a hash algorithm
/// </summary>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "The names need to match the names of the algorithms.")]
public enum HashType
{
	/// <summary>
	/// No hash algorithm.
	/// </summary>
	None = 0,

	/// <summary>
	/// CRC32 hash algorithm.
	/// </summary>
	CRC32,

	/// <summary>
	/// CRC64 hash algorithm.
	/// </summary>
	CRC64,

	/// <summary>
	/// FNV-1 hash algorithm.
	/// </summary>
	FNV1,

	/// <summary>
	/// FNV-1A hash algorithm.
	/// </summary>
	FNV1A,

	/// <summary>
	/// MD5 hash algorithm.
	/// </summary>
	MD5,

	/// <summary>
	/// SHA-1 hash algorithm.
	/// </summary>
	SHA1,

	/// <summary>
	/// SHA-2 hash algorithm.
	/// </summary>
	SHA2,

	/// <summary>
	/// SHA-256 hash algorithm.
	/// </summary>
	SHA256,

	/// <summary>
	/// SHA-2-256 hash algorithm.
	/// </summary>
	SHA2_256,

	/// <summary>
	/// SHA-2-512 hash algorithm.
	/// </summary>
	SHA2_512,

	/// <summary>
	/// SHA-3 hash algorithm.
	/// </summary>
	SHA3,

	/// <summary>
	/// SHA-3-256 hash algorithm.
	/// </summary>
	SHA3_256,

	/// <summary>
	/// SHA-3-512 hash algorithm.
	/// </summary>
	SHA3_512,

	/// <summary>
	/// SHA-512 hash algorithm.
	/// </summary>
	SHA512,
}
