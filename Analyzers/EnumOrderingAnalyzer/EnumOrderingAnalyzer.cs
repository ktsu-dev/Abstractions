// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Analyzers;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EnumOrderingAnalyzer : DiagnosticAnalyzer
{
	public const string DiagnosticId = "KTSU001";

	private static readonly LocalizableString Title = "Enum members should be sorted alphabetically with None = 0 first";
	private static readonly LocalizableString MessageFormat = "Enum '{0}' members should be alphabetically sorted; 'None' (if present) must be first and explicitly set to 0";
	private static readonly LocalizableString Description = "Enforces enum member ordering across the codebase: 'None = 0' first (if present), followed by remaining members in alphabetical order, without explicit numeric values for others.";
	private const string Category = "Styling";

	private static readonly DiagnosticDescriptor Rule = new(
		id: DiagnosticId,
		title: Title,
		messageFormat: MessageFormat,
		category: Category,
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: Description);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();
		context.RegisterSyntaxNodeAction(AnalyzeEnumDeclaration, SyntaxKind.EnumDeclaration);
	}

	private static void AnalyzeEnumDeclaration(SyntaxNodeAnalysisContext context)
	{
		EnumDeclarationSyntax enumDecl = (EnumDeclarationSyntax)context.Node;

		// Ignore empty enums
		if (enumDecl.Members.Count == 0)
		{
			return;
		}

		SeparatedSyntaxList<EnumMemberDeclarationSyntax> members = enumDecl.Members;
		List<string> memberNames = members.Select(m => m.Identifier.Text).ToList();

		// Determine expected ordering
		bool hasNone = memberNames.Contains("None");
		List<string> expectedNames = new();
		if (hasNone)
		{
			expectedNames.Add("None");
		}
		expectedNames.AddRange(memberNames.Where(n => n != "None").OrderBy(n => n, StringComparer.Ordinal));

		// Check alphabetical order and 'None' position
		List<string> actualNames = memberNames;
		bool orderMatches = actualNames.SequenceEqual(expectedNames);

		// Check that others (besides None) do not have explicit values, and None has explicit 0 if present
		bool valuesOk = true;
		if (hasNone)
		{
			EnumMemberDeclarationSyntax noneMember = members.First(m => m.Identifier.Text == "None");
			if (noneMember.EqualsValue is null)
			{
				valuesOk = false;
			}
			else
			{
				Optional<object?> constValue = context.SemanticModel.GetConstantValue(noneMember.EqualsValue.Value);
				if (!constValue.HasValue || constValue.Value is not int intVal || intVal != 0)
				{
					valuesOk = false;
				}
			}
		}

		foreach (EnumMemberDeclarationSyntax member in members)
		{
			if (member.Identifier.Text == "None")
			{
				continue;
			}
			if (member.EqualsValue is not null)
			{
				valuesOk = false;
				break;
			}
		}

		if (!orderMatches || !valuesOk)
		{
			Diagnostic diagnostic = Diagnostic.Create(Rule, enumDecl.Identifier.GetLocation(), enumDecl.Identifier.Text);
			context.ReportDiagnostic(diagnostic);
		}
	}
}

