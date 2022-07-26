// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using Antlr4.Runtime.Misc;
using PakExplorer.Es.Antlr;

namespace PakExplorer.Es.Models; 

public class EnforceVariable {
    public string? VariableAnnotation { get; set; }
    public List<string> VariableModifiers { get; set; } = new();
    public string VariableType { get; set; }
    
    public Dictionary<string, string?> Variables { get; set; } = new();

    public EnforceVariable(EnforceParser.FieldDeclarationContext ctx) {
        if (ctx.annotation() is { } annotation) {
            VariableAnnotation =
                ctx.Start.InputStream.GetText(new Interval(annotation.Start.StartIndex + 1,
                    annotation.Stop.StopIndex - 1));
        }

        if (ctx.variableModifier() is { } variableModifiers) {
            foreach (var modifier in variableModifiers) {
                VariableModifiers.Add(ctx.Start.InputStream.GetText(new Interval(modifier.Start.StartIndex, modifier.Stop.StopIndex)));
            }
        }
        
        if (ctx.typeType() is { } typeType) {
            VariableType = ctx.Start.InputStream.GetText(new Interval(typeType.Start.StartIndex,
                    typeType.Stop.StopIndex));
        }

        if (ctx.variableDeclarators().variableDeclarator() is { } variableDeclarators) {
            foreach (var variableDeclarator in variableDeclarators) {
                var identifier = string.Empty;
                string? value = null;

                if (variableDeclarator.variableDeclaratorId() is { } identifierCtx) {
                    identifier = ctx.Start.InputStream.GetText(new Interval(identifierCtx.Start.StartIndex,
                        identifierCtx.Stop.StopIndex));
                }

                if (variableDeclarator.variableInitializer() is { } variableInitializer) {
                    value = ctx.Start.InputStream.GetText(new Interval(variableInitializer.Start.StartIndex,
                        variableInitializer.Stop.StopIndex));
                }
                Variables.Add(identifier, value);
            }
        }
        
        
    }
    
    public EnforceVariable(EnforceParser.FormalParameterContext ctx) {
        var variableName = string.Empty;

        if (ctx.formalParameterDefined() is { } formalParameter) {
            var variableValue = string.Empty;
            
            if (formalParameter.parameterModifier() is { } dParameterModifiers) {
                foreach (var modifier in dParameterModifiers) {
                    VariableModifiers.Add(ctx.Start.InputStream.GetText(new Interval(modifier.Start.StartIndex, modifier.Stop.StopIndex)));
                }
            }

            if (formalParameter.typeTypeOrVoid() is { } dTypeTypeOrVoid) {
                VariableType = ctx.Start.InputStream.GetText(new Interval(dTypeTypeOrVoid.Start.StartIndex,
                    dTypeTypeOrVoid.Stop.StopIndex));
            }

            if (formalParameter.variableDeclarator() is { } variableDeclaratorContext) {
                if (variableDeclaratorContext.variableDeclaratorId() is { } identifier) {
                    variableName =
                        ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex,
                            identifier.Stop.StopIndex));
                }

                if (variableDeclaratorContext.variableInitializer() is { } variableInitializer) {
                    variableValue = ctx.Start.InputStream.GetText(new Interval(variableInitializer.Start.StartIndex,
                        variableInitializer.Stop.StopIndex));
                }
                Variables.Add(variableName, variableValue);
            }
        } else if (ctx.formalParameterUndefined() is { } formalParameterUndefined) {
            if (formalParameterUndefined.parameterModifier() is { } udParameterModifiers) {
                foreach (var modifier in udParameterModifiers) {
                    VariableModifiers.Add(ctx.Start.InputStream.GetText(new Interval(modifier.Start.StartIndex, modifier.Stop.StopIndex)));
                }
            }
            
            if (formalParameterUndefined.typeTypeOrVoid() is { } udTypeTypeOrVoid) {
                VariableType = ctx.Start.InputStream.GetText(new Interval(udTypeTypeOrVoid.Start.StartIndex,
                    udTypeTypeOrVoid.Stop.StopIndex));
            }

            if (formalParameterUndefined.variableDeclaratorId() is { } identifier) {
                variableName =
                    ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex,
                        identifier.Stop.StopIndex));
            }
            Variables.Add(variableName, null);
        }
        
    }
    
    public EnforceVariable(EnforceParser.LocalVariableDeclarationContext ctx) {
        if (ctx.variableModifier() is { } variableModifiers) {
            foreach (var modifier in variableModifiers) {
                VariableModifiers.Add(
                    ctx.Start.InputStream.GetText(new Interval(modifier.Start.StartIndex, modifier.Stop.StopIndex)));
            }
        }

        if (ctx.localVariableDeclarationAssumptuative() is { } aVariableDeclaration) {
            var variableValue = string.Empty;
            var variableName = string.Empty;
            VariableType = "auto";
            if (aVariableDeclaration.identifier() is { } identifier) {
                variableName =
                    ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex,
                        identifier.Stop.StopIndex));
            }

            if (aVariableDeclaration.expression() is { } expression) {
                variableValue = ctx.Start.InputStream.GetText(new Interval(expression.Start.StartIndex,
                    expression.Stop.StopIndex));
            }

            Variables.Add(variableName, variableValue);
        } else if (ctx.localVariableDeclarationRegular() is { } rVariableDeclaration) {
            if (rVariableDeclaration.typeType() is { } rTypeType) {
                VariableType = ctx.Start.InputStream.GetText(new Interval(rTypeType.Start.StartIndex,
                    rTypeType.Stop.StopIndex));
            }

            if (rVariableDeclaration.variableDeclarators().variableDeclarator() is not
                { } variableDeclaratorContext) return;
            foreach (var variableDeclarator in variableDeclaratorContext) {
                string? variableValue = null;
                var variableName = string.Empty;

                if (variableDeclarator is { } identifier) {
                    variableName =
                        ctx.Start.InputStream.GetText(new Interval(identifier.Start.StartIndex,
                            identifier.Stop.StopIndex));
                }

                if (variableDeclarator.variableInitializer() is { } variableInitializer) {
                    variableValue = ctx.Start.InputStream.GetText(new Interval(variableInitializer.Start.StartIndex,
                        variableInitializer.Stop.StopIndex));
                }
                Variables.Add(variableName, variableValue);
            }
        }

    }

}