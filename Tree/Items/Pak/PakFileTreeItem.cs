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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PakExplorer.Pak;
using PakExplorer.Tree.Items.Es;

namespace PakExplorer.Tree.Items; 

public class PakTreeItem : IParentTreeItem {
    public readonly Pak.Pak PakFile;
    public PakDirectoryTreeItem PakPakDirectoryRoot;
    public string Name { get; set; }
    public ICollection<ITreeItem> Children => PakPakDirectoryRoot.Children;

    public PakTreeItem(Pak.Pak pak) {
        PakFile = pak;
        Name = pak.FileName;
        GenerateRoot();
    }

    private void GenerateRoot() {
        var root = new PakDirectoryTreeItem(null);
        foreach (var entry in PakFile.PakEntries) {
            var parent = Path.GetDirectoryName(entry.Name).Trim('/','\\');
            if (string.IsNullOrEmpty(parent)) {
                root.AddEntry(PakFile ,entry);
            } else {
                GetDirectory(root, parent).AddEntry(PakFile, entry);
            }
        }

        PakPakDirectoryRoot = root;
    }
    
    private static PakDirectoryTreeItem GetDirectory(PakDirectoryTreeItem root, string directory) {
        var parent = Path.GetDirectoryName(directory).Trim(Path.PathSeparator);
        if (string.IsNullOrEmpty(parent)) {
            return root.CreateOrGetDirectory(directory);
        } else {
            return GetDirectory(root, parent).CreateOrGetDirectory(Path.GetFileName(directory));
        }
    }

    
}