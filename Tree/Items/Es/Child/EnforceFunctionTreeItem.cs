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
using PakExplorer.Es.Models;

namespace PakExplorer.Tree.Items.Es.Child; 

public class EnforceFunctionTreeItem : ITreeItem {
    public readonly EnforceFunction EsFunction;
    public readonly EnforceClass? EsParentClass;
    public string Name { get; set; }
    public EnforceFunctionTreeItem(EnforceFunction func) {
        EsFunction = func;
        EsParentClass = null;
        var nameBuilder = new StringBuilder(EsFunction.IsDeconstructor ? "{deconst} " : "{func} ");
        nameBuilder.Append(func.FunctionName).Append('(');
        
        List<string> paramsToAppend = new();
        
        foreach (var param in func.FunctionParameters) {
            for (var i = 0; i < param.Variables.Count; i++) {
                paramsToAppend.Add(param.VariableType);
            }
        }

        nameBuilder.Append(string.Join(", ", paramsToAppend)).Append("): ").Append(func.FunctionType);
        Name = nameBuilder.ToString();
    }

    public EnforceFunctionTreeItem(EnforceFunction func, EnforceClass parentClass) {
        EsFunction = func;
        EsParentClass = parentClass;
        var nameBuilder = new StringBuilder(string.Equals(parentClass.ClassName, func.FunctionName, StringComparison.CurrentCultureIgnoreCase) 
            ? (func.IsDeconstructor 
                ? "{deconstructor} " 
                : "{constructor} ")
            : "{func}");
        
        nameBuilder.Append(func.FunctionName).Append('(');
        
        List<string> paramsToAppend = new();
        
        foreach (var param in func.FunctionParameters) {
            for (var i = 0; i < param.Variables.Count; i++) {
                paramsToAppend.Add(param.VariableType);
            }
        }

        nameBuilder.Append(string.Join(", ", paramsToAppend)).Append("): ").Append(func.FunctionType);
        Name = nameBuilder.ToString();
    }
    
}