// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.IO;
using PakExplorer.Pak;

namespace PakExplorer.Tree.Items; 

public class EntryTreeItem : IFileItem {
    private readonly Pak.PakEntry _pakEntry;
    private readonly Pak.Pak _pak;

    public string Name { get; } 
    
    public string Extension { get; }
    public string FullPath { get; }
    public byte[] EntryData { get; set; }


    public EntryTreeItem(Pak.Pak pak, PakEntry entry) {
        _pak = pak;
        _pakEntry = entry;
        FullPath = entry.Name.Replace("\\", "\0").Replace('/', '\0').Replace('\0', Path.DirectorySeparatorChar);
        Extension = Path.GetExtension(FullPath);
        EntryData = entry.EntryData;
        Name = Path.GetFileName(FullPath);
    }
}