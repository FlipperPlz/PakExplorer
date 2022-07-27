// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.Text;
using PakExplorer.Es.Models;

namespace PakExplorer.Tree.Items.Es.Child; 

public class EnforceFunctionTreeItem : ITreeItem {
    public string Name { get; set; }
    public EnforceFunctionTreeItem(EnforceFunction func) {
        var nameBuilder = new StringBuilder("{func} ");
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