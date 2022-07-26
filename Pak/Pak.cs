using System;
using System.Collections.Generic;
using System.IO;

namespace PakExplorer.Pak;

public class Pak {
    public string FileName;
    public List<PakEntry> PakEntries = new();
    private BinaryReader? _reader;
    
    private int _formSize;
    private int _dataSize;
    private int _entriesSize;
    
    public Pak(string src) {
        FileName = src;
        using (_reader = new BinaryReader(File.OpenRead(src))) {
            ReadForm();
            ReadHead();
            ReadData();
            ReadEntries();
            _reader = null;
        }
    }

    private void ReadForm() {
        _reader.SkipPakSignature(); // "FORM"
        _formSize = _reader.ReadInt32BE(); // form Size
        _reader.SkipPakSignature(); // "PAC1"
    }

    private void ReadHead() {
        _reader.SkipPakSignature(); // "HEAD"
        _reader.Skip(32); // Constant Unknown
    }

    private void ReadData() {
        _reader.SkipPakSignature(); // "DATA"
        _dataSize = _reader.ReadInt32BE(); // data Size
        _reader.Skip(_dataSize); // skip data for now
    }

    private void ReadEntries() {
        _reader.SkipPakSignature(); // "FILE"
        _entriesSize = _reader.ReadInt32BE(); // PakEntries Size ( not count )

        try {
            _reader.Skip(2); // Null
            _reader.Skip(4); // Constant Unknown

            for ( var posEntries = _reader.BaseStream.Position; _reader.BaseStream.Position - posEntries < _entriesSize; ) {
                var entryType = (PakEntryType)_reader.ReadByte();
                int entryNameLength = _reader.ReadByte();
                var entryName = new string(_reader.ReadChars(entryNameLength));

                if ( entryType == PakEntryType.Directory ) ReadEntriesFromDirectory(entryName);
                else PakEntries.Add(new PakEntry(entryName, _reader));
            }
        }
        catch {}
    }

    private void ReadEntriesFromDirectory(string dirName) {
        var childCount = _reader.ReadInt32();

        for ( var i = 0; i < childCount; i++ ) {
	        var entryType = (PakEntryType)_reader.ReadByte();
            int entryNameLength = _reader.ReadByte();
            var entryName = dirName + "\\" + new string(_reader.ReadChars(entryNameLength));

            switch (entryType) {
                case PakEntryType.File:
                    PakEntries.Add(new PakEntry(entryName, _reader));
                    break;
                case PakEntryType.Directory:
                    ReadEntriesFromDirectory(entryName);
                    break;
                default:
                    throw new Exception("Unknown Entry Type");
            }
        }
    }


    public Stream GetEntryData(PakEntry entry) {
        using (_reader = new BinaryReader(File.OpenRead(FileName))) {
            if (entry.CompressionType == PakCompressionType.Zlib) {
                try {
                    _reader.BaseStream.Position = entry.BinaryOffset;
                    var ret =  _reader.ReadCompressedData(entry.OriginalSize);
                    _reader.Close();
                    return ret;
                } catch {
                    _reader.BaseStream.Position = entry.BinaryOffset;
                    var ret = _reader.ReadDataStream(entry.Size);
                    _reader.Close();
                    return ret;
                }
            } else {
                _reader.BaseStream.Position = entry.BinaryOffset;
                var ret = _reader.ReadDataStream(entry.Size);
                _reader.Close();
                return ret;
            }
            _reader = null;
        }
    }
}