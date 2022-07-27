// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;

namespace PakExplorer.Tree; 

public interface IParentTreeItem : ITreeItem {
    public string Name { get; set; }
    public ICollection<ITreeItem> Children { get; }
}