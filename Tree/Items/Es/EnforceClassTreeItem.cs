// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using PakExplorer.Es.Models;
using PakExplorer.Tree.Items.Es.Child;

namespace PakExplorer.Tree.Items.Es; 

public class EnforceClassTreeItem : IParentTreeItem {
    private readonly EnforceClass _esClazz;
    public string Name { get; set; }

    public ICollection<ITreeItem> Children {
        get {
            var col = new List<ITreeItem>();
            col.AddRange(FunctionTreeItems);
            col.AddRange(VariableTreeItems);
            return col;
        }
    }
    public IEnumerable<ITreeItem> FunctionTreeItems => _esClazz.Functions.Select(static func => new EnforceFunctionTreeItem(func));
    public IEnumerable<ITreeItem> VariableTreeItems => _esClazz.Variables.Select(static var => new EnforceVariableTreeItem(var));
    public EnforceClassTreeItem(EnforceClass clazz) {
        var nameBuilder = new StringBuilder("{class} ");
        if (clazz.ModdedClass) nameBuilder.Append("[modded]");
        if (clazz.SealedClass) nameBuilder.Append("[sealed]");
        nameBuilder.Append(clazz.ClassName);
        if (clazz.ParentClass is not null and not "") nameBuilder.Append(": ").Append(clazz.ParentClass);
        Name = nameBuilder.ToString();
        _esClazz = clazz;
    }
}