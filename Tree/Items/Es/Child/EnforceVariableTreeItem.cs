// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Text;
using PakExplorer.Es.Models;

namespace PakExplorer.Tree.Items.Es.Child; 

public class EnforceVariableTreeItem : ITreeItem {
    public readonly EnforceVariable EsVariable;
    public string Name { get; set; }
    
    public EnforceVariableTreeItem(EnforceVariable variable) {
        EsVariable = variable;
        var nameBuilder = new StringBuilder("{var} ");
        nameBuilder.Append(string.Join(", ", variable.Variables.Keys)).Append(": ").Append(variable.VariableType);
        Name = nameBuilder.ToString();
    }

    
}