// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

/// <summary>
/// Defines a contract for serialization options.
/// </summary>
public interface ISerializationOptions
{
	/// <summary>
	/// The policy for fields when serializing or deserializing.
	/// </summary>
	public MemberPolicy FieldPolicy { get; set; }

	/// <summary>
	/// The policy for properties when serializing or deserializing.
	/// </summary>
	public MemberPolicy PropertyPolicy { get; set; }

	/// <summary>
	/// The policy for enums when serializing or deserializing.
	/// </summary>
	public EnumValuePolicy EnumValuePolicy { get; set; }

	/// <summary>
	/// The policy for dictionaries when serializing or deserializing.
	/// </summary>
	public DictionaryKeyPolicy DictionaryKeyPolicy { get; set; }

	/// <summary>
	/// The policy for references when serializing or deserializing.
	/// </summary>
	public ReferencePolicy ReferencePolicy { get; set; }

	/// <summary>
	/// The policy for boxing when serializing or deserializing.
	/// </summary>
	public BoxingPolicy BoxingPolicy { get; set; }

}

/// <summary>
/// The policy for a member when serializing or deserializing.
/// </summary>
public class MemberPolicy
{
	/// <summary>
	/// The policy for naming the member when serializing.
	/// </summary>
	public NamePolicy NamePolicy { get; set; } = NamePolicy.PascalCase;

	/// <summary>
	/// The policy for case sensitivity when deserializing.
	/// </summary>
	public CaseSensitivityPolicy CaseSensitivityPolicy { get; set; } = CaseSensitivityPolicy.CaseInsensitive;

	/// <summary>
	/// The policy for including the member when serializing.
	/// </summary>
	public InclusionPolicy SerializationInclusionPolicy { get; set; } = InclusionPolicy.IncludeAll;

	/// <summary>
	/// The policy for including the member when deserializing.
	/// </summary>
	public InclusionPolicy DeserializationInclusionPolicy { get; set; } = InclusionPolicy.IncludeAll;
}

/// <summary>
/// The policy for a dictionary when serializing or deserializing.
/// </summary>
public class DictionaryKeyPolicy
{
	/// <summary>
	/// The policy for the key of the dictionary when serializing.
	/// </summary>
	public NamePolicy SerializationPolicy { get; set; } = NamePolicy.PascalCase;

	/// <summary>
	/// The policy for the key of the dictionary when deserializing.
	/// </summary>
	public NamePolicy DeserializationPolicy { get; set; } = NamePolicy.PascalCase;
}

[Flags]
public enum InclusionPolicy
{
	None = 0,
	Include = 1 << 0,
	Public = 1 << 1,
	NonPublic = 1 << 2,
	Instance = 1 << 3,
	Static = 1 << 4,
}

[Flags]
public enum AccessibilityPolicy
{
	Public = 1 << 0,
	NonPublic = 1 << 1,
}

[Flags]

/// <summary>
/// The policy for naming a member when serializing.
/// </summary>
public enum NamePolicy
{
	/// <summary>
	/// Do not change the name.
	/// </summary>
	None = 0,

	/// <summary>
	/// Convert to PascalCase.
	/// </summary>
	PascalCase,

	/// <summary>
	/// Convert to CamelCase.
	/// </summary>
	CamelCase,

	/// <summary>
	/// Convert to SnakeCase.
	/// </summary>
	SnakeCase,

	/// <summary>
	/// Convert to KebabCase.
	/// </summary>
	KebabCase,

	/// <summary>
	/// Convert to MacroCase.
	/// </summary>
	MacroCase,
}

/// <summary>
/// The policy for case sensitivity when deserializing.
/// </summary>
public enum CaseSensitivityPolicy
{
	/// <summary>
	/// Case insensitive.
	/// </summary>
	CaseInsensitive = 0,

	/// <summary>
	/// Case sensitive.
	/// </summary>
	CaseSensitive,
}

/// <summary>
/// The policy for reference handling when serializing.
/// </summary>
public enum ReferencePolicy
{
	/// <summary>
	/// Ignore the reference.
	/// </summary>
	Ignore = 0,

	/// <summary>
	/// Make a copy of the referenced object, excluding circular references which will be ignored.
	/// </summary>
	Copy,

	/// <summary>
	/// Preserve the reference.
	/// </summary>
	Reference,
}

/// <summary>
/// The policy for the value of an enum when serializing.
/// </summary>
[Flags]
public enum EnumValuePolicy
{
	/// <summary>
	/// Use the name of the enum value.
	/// </summary>
	Name = 1 << 0,

	/// <summary>
	/// Use the number of the enum value.
	/// </summary>
	Number = 1 << 1,
}

/// <summary>
/// The policy for boxing when serializing or deserializing.
/// </summary>
public enum BoxingPolicy
{
	/// <summary>
	/// Do not box.
	/// </summary>
	None = 0,

	/// <summary>
	/// Box numeric types.
	/// </summary>
	BoxNumeric,

	/// <summary>
	/// Box derived types.
	/// </summary>
	BoxDerived,

	/// <summary>
	/// Box all types.
	/// </summary>
	BoxAll = BoxNumeric | BoxDerived,
}
