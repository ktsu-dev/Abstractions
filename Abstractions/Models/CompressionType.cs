// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions.Models;

/// <summary>
/// Specifies the compression algorithm to use for serialized data.
/// </summary>
public enum CompressionType
{
	/// <summary>
	/// No compression.
	/// </summary>
	None = 0,

	/// <summary>
	/// BZip2 compression algorithm.
	/// </summary>
	BZip2,

	/// <summary>
	/// Brotli compression algorithm.
	/// </summary>
	Brotli,

	/// <summary>
	/// Deflate compression algorithm.
	/// </summary>
	Deflate,

	/// <summary>
	/// GZip compression algorithm.
	/// </summary>
	GZip,

	/// <summary>
	/// LZ4 compression algorithm (high speed).
	/// </summary>
	LZ4,

	/// <summary>
	/// LZMA compression algorithm.
	/// </summary>
	LZMA,

	/// <summary>
	/// Snappy compression algorithm.
	/// </summary>
	Snappy,

	/// <summary>
	/// XZ (LZMA2) compression algorithm.
	/// </summary>
	XZ,

	/// <summary>
	/// Zlib compression algorithm.
	/// </summary>
	Zlib,

	/// <summary>
	/// Zstandard (Zstd) compression algorithm.
	/// </summary>
	Zstd,
}
