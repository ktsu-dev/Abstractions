// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions.Models;

/// <summary>
/// Specifies the type of filesystem context.
/// </summary>
public enum FileSystemType
{
	/// <summary>
	/// Isolated filesystem context (e.g., sandboxed per-user or per-app instance).
	/// </summary>
	Isolated,

	/// <summary>
	/// Shared filesystem context (e.g., common/shared across users or components).
	/// </summary>
	Shared,
}

