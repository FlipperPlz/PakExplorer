// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PakExplorer.Tree.Items.Es; 

public class ScriptPakTreeItem : IParentTreeItem {
    public string Name { get; set; }
    public ICollection<ITreeItem> Children => (ICollection<ITreeItem>) Scripts;

    public ICollection<ITreeItem> Scripts = new List<ITreeItem>();

    public ScriptPakTreeItem(PakTreeItem pacTreeItem) {
        Name = pacTreeItem.Name;
        foreach (var pakScriptEntry in pacTreeItem.PakFile.PakEntries.Where(e => Path.GetExtension(e.Name) == ".c")) {
            Scripts.Add(new EnforceScriptTreeItem(pakScriptEntry));
        }
    }
}