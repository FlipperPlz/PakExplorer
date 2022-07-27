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
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using PakExplorer.Es;
using PakExplorer.Es.Antlr;
using PakExplorer.Es.Models;
using PakExplorer.Pak;
using PakExplorer.Tree.Items.Es.Child;

namespace PakExplorer.Tree.Items.Es; 

public class EnforceScriptTreeItem : IParentTreeItem {
    public string Name { get; set; }

    public ICollection<ITreeItem> Children {
        get {
            var col = new List<ITreeItem>();
            col.AddRange(ClassTreeItems);
            col.AddRange(FunctionTreeItems);
            col.AddRange(VariableTreeItems);
            return col;
        }
        
    }

    public List<EnforceClassTreeItem> ClassTreeItems;
    public List<EnforceFunctionTreeItem> FunctionTreeItems;
    public List<EnforceVariableTreeItem> VariableTreeItems;

    public EnforceFile Scope { get; private set; }
    private readonly PakEntry _pakEntry;

    public EnforceScriptTreeItem(PakEntry pakScriptEntry) {
        _pakEntry = pakScriptEntry;
        Name = _pakEntry.Name;
        GenerateScope();
    }

    private void GenerateScope() {
        var lexer = new EnforceLexer(CharStreams.fromString(Encoding.UTF8.GetString(_pakEntry.EntryData)));
        var parser = new EnforceParser(new CommonTokenStream(lexer));
        var listener = new EnforcePreParser();
        new ParseTreeWalker().Walk(listener, parser.computationalUnit());
        Scope = listener.EsFile;
        ClassTreeItems = Scope.Classes.Select(static clazz => new EnforceClassTreeItem(clazz)).ToList();
        FunctionTreeItems = Scope.Functions.Select(static func => new EnforceFunctionTreeItem(func)).ToList();
        VariableTreeItems = Scope.Variables.Select(static var => new EnforceVariableTreeItem(var)).ToList();
    }

    public override string ToString() => Scope.ToString();
}