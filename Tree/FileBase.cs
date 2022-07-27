// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using PakExplorer.Pak;

namespace PakExplorer.Tree; 

public abstract class FileBase : ITreeItem {
    public readonly Pak.Pak Pak;
    public readonly Pak.PakEntry PakEntry;
    
    public string Extension { get; }
    public string FullPath { get; }
    public byte[] EntryData { get; set; }
    public string Name { get; set; }

    public FileBase(Pak.Pak pak, PakEntry entry) {
        Pak = pak;
        PakEntry = entry;
        FullPath = entry.Name.Replace("\\", "\0").Replace('/', '\0').Replace('\0', Path.DirectorySeparatorChar);
        Extension = Path.GetExtension(FullPath);
        EntryData = entry.EntryData;
        Name = Path.GetFileName(FullPath);
    }
}