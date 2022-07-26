// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System;
using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using PakExplorer.Es.Antlr;

namespace PakExplorer.Es.Models; 

public class EnforceClass {
    public string? ClassAnnotation { get; set; } = null;
    public bool SealedClass { get; set; } = false;
    public bool ModdedClass { get; set; } = false;
    
    public string ClassName { get; set; } = string.Empty;
    public string? ParentClass { get; set; } = null;

    public List<EnforceFunction> Functions { get; set; } = new();
    public List<EnforceVariable> Variables { get; set; } = new();


    public EnforceClass(EnforceParser.ClassDeclarationContext ctx) {
        if (ctx.Parent is EnforceParser.TypeDeclarationContext typeDeclaration) {
            if (typeDeclaration.annotation() is { } annotation) {
                ClassAnnotation = ctx.Start.InputStream.GetText(new Interval(annotation.Start.StartIndex,
                    annotation.Stop.StopIndex));
            }
            
            if (typeDeclaration.classModifier() is { } classModifierContext) {
                if (classModifierContext.SEALED() is { }) SealedClass = true;
                if (classModifierContext.MODDED() is { }) ModdedClass = true;
            }
        }

        if (ctx.identifier() is { } identifierContext) {
            ClassName = ctx.Start.InputStream.GetText(new Interval(identifierContext.Start.StartIndex,
                identifierContext.Stop.StopIndex));
        }

        if (ctx.classOrEnumExtension() is { } extension) {
            if (extension.typeType() is { } typeType) {
                ParentClass = ctx.Start.InputStream.GetText(new Interval(typeType.Start.StartIndex,
                    typeType.Stop.StopIndex));
            } 
        }

        if (ctx.classBody() is not { } body || body.globalDeclaration() is not { } globalDeclarations) return;
        foreach (var globalDeclaration in globalDeclarations) {
            if (globalDeclaration.methodDeclaration() is { } method) Functions.Add(new EnforceFunction(method));
            if (globalDeclaration.fieldDeclaration() is { } field) Variables.Add(new EnforceVariable(field));
        }

    }
}

