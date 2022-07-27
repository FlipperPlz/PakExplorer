// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using PakExplorer.Es.Antlr;

namespace PakExplorer.Es.Models; 

public class EnforceEnum {
    public string? EnumAnnotation { get; set; } = null;
    public bool SealedEnum { get; set; } = false;
    public bool ModdedEnum { get; set; } = false;
    public string? EnumParent { get; set; } = null;
    public string EnumName { get; set; } = string.Empty;

    public Dictionary<string, string?> EnumValues { get; set; } = new();
    
    public EnforceEnum(EnforceParser.EnumDeclarationContext ctx) {
        if (ctx.Parent is EnforceParser.TypeDeclarationContext typeDeclaration) {
            if (typeDeclaration.annotation() is { } annotation) {
                EnumAnnotation = ctx.Start.InputStream.GetText(new Interval(annotation.Start.StartIndex,
                    annotation.Stop.StopIndex));
            }
            
            if (typeDeclaration.classModifier() is { } classModifierContext) {
                if (classModifierContext.SEALED() is { }) SealedEnum = true;
                if (classModifierContext.MODDED() is { }) ModdedEnum = true;
            }
        }

        if (ctx.identifier() is { } identifier) {
            EnumName = ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex,
                identifier.Stop.StopIndex));
        }

        if (ctx.classOrEnumExtension() is { } extension) {
            if (extension.typeType() is { } type) {
                EnumParent = ctx.Start.InputStream.GetText(new Interval(type.Start.StartIndex,
                    type.Stop.StopIndex));
            }
        }

        if (ctx.enumBody() is { } body) {
            foreach (var enumValueContext in body.enumValue()) {
                var name = string.Empty;
                string? value = null;

                if (enumValueContext.identifier() is { } enumId) {
                    name = ctx.Start.InputStream.GetText(new Interval(enumId.Start.StartIndex,
                        enumId.Stop.StopIndex));
                }

                if (enumValueContext.expression() is { } expression) {
                    value = ctx.Start.InputStream.GetText(new Interval(expression.Start.StartIndex,
                        expression.Stop.StopIndex));
                }
                EnumValues.Add(name, value);
            }
        }
    }

    public override string ToString() {
        var ctxBuilder = new StringBuilder();
        if (EnumAnnotation is not null) ctxBuilder.Append(EnumAnnotation).Append('\n');
        if (ModdedEnum) ctxBuilder.Append("modded ");
        if (SealedEnum) ctxBuilder.Append("sealed ");
        ctxBuilder.Append("enum ").Append(EnumName);
        if (EnumParent is not null) ctxBuilder.Append(" : ").Append(EnumParent);
        ctxBuilder.Append(" {\n\t");

        var enumValues = new List<string>();
        foreach (var (k, v) in EnumValues) {
            var val = new StringBuilder(k);
            if (v is not null) val.Append(" = ").Append(v);
            enumValues.Add(val.ToString());
            
        }

        ctxBuilder.Append(string.Join(",\n\t", enumValues)).Append('\n');
        ctxBuilder.Append('}');

        return ctxBuilder.ToString();
    }
}