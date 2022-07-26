// /*******************************************************
//  * Copyright (C) 2021-2022 Ryann (Elijah Cyr) <elijahcyr@protonmail.com>
//  *
//  *
//  * PakMan.Pak can not be copied and/or distributed without the express
//  * permission of Ryann
//  *******************************************************/

using System;
using System.IO;
using System.IO.Compression;

namespace PakExplorer.Pak; 

public static class Extensions {
    public static void SkipPakSignature(this BinaryReader reader) {
        reader.BaseStream.Position += 4;
    }
    
    public static void Skip(this BinaryReader reader, int amount) {
        reader.BaseStream.Position += amount;
    }
    
    public static int ReadInt32BE(this BinaryReader reader) {
        var data = reader.ReadBytes(4);
        Array.Reverse(data);
        return BitConverter.ToInt32(data, 0);
    }
    
    public static Stream ReadCompressedData(this BinaryReader pr, int expectedSize) {
        var mem = new MemoryStream();
        pr.Skip(2);
        var deflateStream = new DeflateStream(pr.BaseStream, CompressionMode.Decompress);
        for ( var bytesRead = 0; bytesRead < expectedSize; ) {
            var toRead = 1000; // 1000 byte chunks
            if ( bytesRead + toRead > expectedSize ) toRead = expectedSize - bytesRead;
            var buffer = new byte[toRead];
            deflateStream.Read(buffer, 0, toRead);
            mem.Write(buffer, 0, toRead);
            bytesRead += toRead;
        }

        return mem;
    }

    public static Stream ReadDataStream(this BinaryReader pr, int length) => new MemoryStream(pr.ReadBytes(length));

}