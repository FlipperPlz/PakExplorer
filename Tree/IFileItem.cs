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

namespace PakExplorer.Tree; 

public interface IFileItem : ITreeItem {
    public string Extension { get; }
    public string FullPath { get; }
    public byte[] EntryData { get; set; }
}