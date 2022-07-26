// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakExplorer can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using PakExplorer.Pak;

namespace PakExplorer.Tree.Files; 

public class GenericTreeFile : FileBase{
    public GenericTreeFile(Pak.Pak pak, PakEntry entry) : base(pak, entry) { }
}