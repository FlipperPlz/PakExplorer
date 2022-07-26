// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PakExplorer.Pak;

namespace PakExplorer.Tree.Items; 

public class DirectoryTreeItem : IParentTreeItem {
    public string? Name { get; set; }
    private List<ITreeItem>? merged;
    public readonly List<DirectoryTreeItem> Directories = new();
    public readonly List<EntryTreeItem> Files = new();

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

    public DirectoryTreeItem CreateOrGetDirectory(string childName) {
        var existing = Directories.FirstOrDefault(d => string.Equals(d.Name, childName));
        if (existing != null) return existing;
        existing = new DirectoryTreeItem(childName);
        Directories.Add(existing);
        merged = null;
        return existing;
    }
    
    internal IEnumerable<EntryTreeItem> AllFiles => Directories.SelectMany(static d => d.AllFiles).Concat(Files);

    public DirectoryTreeItem(string? name) => Name = name;
    
    internal void AddEntry(Pak.Pak pak, PakEntry entry) {
        Files.Add(new EntryTreeItem(pak, entry));
        merged = null;
    }


}