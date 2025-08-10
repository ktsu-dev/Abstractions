// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions.Models;

/// <summary>
/// Specifies naive/educational obfuscation algorithms (not cryptographically secure).
/// </summary>
public enum ObfuscationType
{
	/// <summary>
	/// No obfuscation.
	/// </summary>
	None = 0,

	/// <summary>
	/// Atbash monoalphabetic substitution cipher.
	/// </summary>
	Atbash,

	/// <summary>
	/// Caesar shift cipher (ROT-N, parameterized shift).
	/// </summary>
	Caesar,

	/// <summary>
	/// ROT13 letter substitution (fixed Caesar shift by 13).
	/// </summary>
	ROT13,

	/// <summary>
	/// ROT47 substitution (printable ASCII range).
	/// </summary>
	ROT47,

	/// <summary>
	/// Rail Fence transposition cipher.
	/// </summary>
	RailFence,

	/// <summary>
	/// Monoalphabetic substitution with a fixed mapping.
	/// </summary>
	SubstitutionMonoalphabetic,

	/// <summary>
	/// Vigenère polyalphabetic substitution cipher.
	/// </summary>
	Vigenere,

	/// <summary>
	/// XOR bytes with a short repeating key.
	/// </summary>
	XORRepeatingKey,

	/// <summary>
	/// XOR every byte with a single constant byte.
	/// </summary>
	XORSingleByte,
}
