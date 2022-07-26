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
using System.Security.Cryptography.X509Certificates;

namespace PakExplorer.Tree.Items; 

public class PakTreeItem : IParentTreeItem {
    public readonly Pak.Pak PakFile;
    public DirectoryTreeItem PakDirectoryRoot;
    
    public string Name { get; set; }
    public ICollection<ITreeItem> Children => PakDirectoryRoot.Children;

    public PakTreeItem(Pak.Pak pak) {
        PakFile = pak;
        Name = pak.FileName;
        GenerateRoot();
    }

    private void GenerateRoot() {
        var root = new DirectoryTreeItem(null);
        foreach (var entry in PakFile.PakEntries) {
            var parent = Path.GetDirectoryName(entry.Name).Trim('/','\\');
            if (string.IsNullOrEmpty(parent)) {
                root.AddEntry(PakFile ,entry);
            } else {
                GetDirectory(root, parent).AddEntry(PakFile, entry);
            }
        }

        PakDirectoryRoot = root;
    }
    
    private static DirectoryTreeItem GetDirectory(DirectoryTreeItem root, string directory) {
        var parent = Path.GetDirectoryName(directory).Trim(Path.PathSeparator);
        if (string.IsNullOrEmpty(parent)) {
            return root.CreateOrGetDirectory(directory);
        } else {
            return GetDirectory(root, parent).CreateOrGetDirectory(Path.GetFileName(directory));
        }
    }

    
}