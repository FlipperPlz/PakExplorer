// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakMan.Pak can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/


using System.IO;

namespace PakExplorer.Pak; 

public sealed class PakEntry {
    internal int BinaryOffset { get; private set; }
    internal int Size { get; private set; }
    internal int OriginalSize { get; private set; }
    internal string Name { get; private set; }
    internal readonly PakCompressionType CompressionType;
    private byte[] UnknownData { get; set; }

    public PakEntry( string entryName, BinaryReader pr ) {
        Name = entryName;
        BinaryOffset = pr.ReadInt32();
        Size = pr.ReadInt32();
        OriginalSize = pr.ReadInt32();
        pr.Skip(4); // Null
        CompressionType = (PakCompressionType)pr.ReadInt32BE();
        UnknownData = pr.ReadBytes(4); // Unknown
    }
}