// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Analyzers;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EnumOrderingCodeFixProvider))]
[Shared]
public sealed class EnumOrderingCodeFixProvider : CodeFixProvider
{
	public override ImmutableArray<string> FixableDiagnosticIds => [EnumOrderingAnalyzer.DiagnosticId];

	public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		Diagnostic diagnostic = context.Diagnostics.First();
		SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
		if (root is null)
		{
			return;
		}

		if (root.FindNode(diagnostic.Location.SourceSpan) is not EnumDeclarationSyntax node)
		{
			return;
		}

		context.RegisterCodeFix(
			CodeAction.Create(
				title: "Sort enum and set None = 0",
				createChangedDocument: c => ApplyFixAsync(context.Document, node, c),
				equivalenceKey: "SortEnumAndSetNoneZero"),
			diagnostic);
	}

	private static async Task<Document> ApplyFixAsync(Document document, EnumDeclarationSyntax enumDecl, CancellationToken cancellationToken)
	{
		DocumentEditor editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

		// Compute new member ordering
		SeparatedSyntaxList<EnumMemberDeclarationSyntax> members = enumDecl.Members;
		bool hasNone = members.Any(m => m.Identifier.Text == "None");

		List<EnumMemberDeclarationSyntax> sortedMembers = new();
		if (hasNone)
		{
			EnumMemberDeclarationSyntax noneMember = members.First(m => m.Identifier.Text == "None");
			// Ensure explicit = 0
			EqualsValueClauseSyntax equalsZero = SyntaxFactory.EqualsValueClause(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0)));
			EnumMemberDeclarationSyntax fixedNone = noneMember.WithEqualsValue(equalsZero);
			sortedMembers.Add(fixedNone);
		}

		IOrderedEnumerable<EnumMemberDeclarationSyntax> rest = members.Where(m => m.Identifier.Text != "None").OrderBy(m => m.Identifier.Text, StringComparer.Ordinal);
		foreach (EnumMemberDeclarationSyntax? member in rest)
		{
			// Remove explicit values from others
			EnumMemberDeclarationSyntax fixedMember = member.WithEqualsValue(null);
			sortedMembers.Add(fixedMember);
		}

		// Keep trivia from original enum
		EnumDeclarationSyntax newEnum = enumDecl.WithMembers(SyntaxFactory.SeparatedList(sortedMembers));

		editor.ReplaceNode(enumDecl, newEnum);
		return editor.GetChangedDocument();
	}
}

