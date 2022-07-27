// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.Text;
using PakExplorer.Es.Antlr;

namespace PakExplorer.Es.Models; 

public class EnforceFile {
    public List<EnforceClass> Classes { get; set; } = new();
    public List<EnforceEnum> Enums { get; set; } = new();

    public List<EnforceFunction> Functions { get; set; } = new();
    public List<EnforceVariable> Variables { get; set; } = new();


    public EnforceFile(EnforceParser.ComputationalUnitContext ctx) {
        if (ctx.globalDeclaration() is { } globalDeclarations) {
            foreach (var globalDeclaration in globalDeclarations) {
                if (globalDeclaration.methodDeclaration() is { } method) Functions.Add(new EnforceFunction(method));
                if (globalDeclaration.fieldDeclaration() is { } field) Variables.Add(new EnforceVariable(field));
            }
        }

        if (ctx.typeDeclaration() is { } typeDeclarations) {
            foreach (var typeDeclaration in typeDeclarations) {
                if (typeDeclaration.classDeclaration() is { } clazz) Classes.Add(new EnforceClass(clazz));
                if (typeDeclaration.enumDeclaration() is { } enumCtx) Enums.Add(new EnforceEnum(enumCtx));
            }
        }
    }

    public override string ToString() {
        var ctxBuilder = new StringBuilder();
        if (Variables.Count != 0) ctxBuilder.Append("//-----------------------------Variables---------------------------------\n");
        Variables.ForEach(v => ctxBuilder.Append(v).Append(';').Append("\n\n"));
        if (Functions.Count != 0) ctxBuilder.Append("//-----------------------------Functions---------------------------------\n");
        Functions.ForEach(f => ctxBuilder.Append(f).Append("\n\n"));
        if (Classes.Count != 0) ctxBuilder.Append("//------------------------------Classes----------------------------------\n");
        Classes.ForEach(c => ctxBuilder.Append(c).Append("\n\n"));
        if (Enums.Count != 0) ctxBuilder.Append("//-------------------------------Enums-----------------------------------\n");
        Enums.ForEach(e => ctxBuilder.Append(e).Append("\n\n"));
        return ctxBuilder.ToString();
    }
}
