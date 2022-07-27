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
using System.Security.Cryptography.X509Certificates;
using PakExplorer.Pak;
using PakExplorer.Tree.Files;
using PakExplorer.Tree.Items.Es;

namespace PakExplorer.Tree.Items; 

public class PakDirectoryTreeItem : IParentTreeItem {
    public string? Name { get; set; }
    private List<ITreeItem>? merged;
    public readonly List<PakDirectoryTreeItem> Directories = new();
    public readonly List<FileBase> Files = new();


    public ICollection<ITreeItem> Children {
        get
        {
            if (merged == null)
            {
                merged = Directories.OrderBy(d => d.Name).Cast<ITreeItem>().Concat(Files.OrderBy(f => f.Name)).ToList();
            }
            return merged;
        }
    }

    public PakDirectoryTreeItem CreateOrGetDirectory(string childName) {
        var existing = Directories.FirstOrDefault(d => string.Equals(d.Name, childName));
        if (existing != null) return existing;
        existing = new PakDirectoryTreeItem(childName);
        Directories.Add(existing);
        merged = null;
        return existing;
    }
    
    internal IEnumerable<FileBase> AllFiles => Directories.SelectMany(static d => d.AllFiles).Concat(Files);

    public PakDirectoryTreeItem(string? name) => Name = name;
    
    internal void AddEntry(Pak.Pak pak, PakEntry entry) {
        switch (Path.GetExtension(entry.Name)) {
            default:
                Files.Add(new GenericTreeFile(pak, entry));
                break;
            
        }
        merged = null;
    }
    
}