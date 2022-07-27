// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using PakExplorer.Es.Antlr;

namespace PakExplorer.Es.Models; 

public class EnforceFunction {
    public string? FunctionAnnotation { get; set; }
    public List<string> FunctionModifiers { get; set; } = new();
    public string FunctionName { get; set; } = string.Empty;
    public string FunctionType { get; set; } = string.Empty;

    public List<EnforceVariable> FunctionParameters { get; set; } = new();
    public string FunctionBody { get; set; } = string.Empty;

    public bool IsDeconstructor { get; set; } = false;

    public EnforceFunction(EnforceParser.MethodDeclarationContext ctx) {
        if (ctx.annotation() is { } annotation) {
            FunctionAnnotation =
                ctx.Start.InputStream.GetText(new Interval(annotation.Start.StartIndex, annotation.Stop.StopIndex));
        }

        if (ctx.methodModifier() is { } modifiers) {
            foreach (var modifier in modifiers) {
                FunctionModifiers.Add(ctx.Start.InputStream.GetText(new Interval(modifier.Start.StartIndex,
                    modifier.Stop.StopIndex)));
            }
        }

        if (ctx.typeTypeOrVoid() is { } type) {
            FunctionType =
                ctx.Start.InputStream.GetText(new Interval(type.Start.StartIndex, type.Stop.StopIndex));
        }

        if (ctx.TILDE() is not null) IsDeconstructor = true;

        if (ctx.identifier() is { } identifier) {
            FunctionName = ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex, identifier.Stop.StopIndex));
        }

        if (ctx.formalParameters() is not null) {
            if(ctx.formalParameters().formalParameterList() is not null) {
                if (ctx.formalParameters().formalParameterList().formalParameter() is { } formalParameters) {
                    foreach (var parameter in formalParameters) FunctionParameters.Add(new EnforceVariable(parameter));
                }
            }
            
        }
        

        if (ctx.methodBody() is { } methodBody) {
            FunctionBody = ctx.Start.InputStream.GetText(new Interval(methodBody.Start.StartIndex, methodBody.Stop.StopIndex));

        }
    }


    public override string ToString() {
        var ctxBuilder = new StringBuilder();
        if (FunctionAnnotation is not null) ctxBuilder.Append(FunctionAnnotation).Append('\n');
        ctxBuilder.Append(string.Join(' ', FunctionModifiers)).Append(' ').Append(FunctionType).Append(' ');
        if (IsDeconstructor) ctxBuilder.Append('~');
        ctxBuilder.Append(FunctionName).Append('(').Append(string.Join(", ", FunctionParameters)).Append(") ")
            .Append(FunctionBody);
        return ctxBuilder.ToString();
    }
}