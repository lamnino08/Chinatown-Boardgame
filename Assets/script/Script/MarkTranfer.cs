using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTranfer
{
    public byte fromIndex {get; set; }
    public byte toIndex {get; set; }
    public byte fromTile {get; set; }
    public byte toTile {get; set; }
    public bool isReplace {get; set; }
    public MarkTranfer(byte fromIndex, byte toIndex, byte fromTile, byte toTile)
    {
        this.fromIndex = fromIndex;
        this.toIndex = toIndex;
        this.fromTile = fromTile;
        this.toTile = toTile;
        isReplace = false;
    }
    public void Print()
    {
        Debug.Log($"FromIndex {fromIndex}; toIndex: {toIndex}; fromTile {fromTile}; toTile {toTile}; isReplaced: {isReplace}");
    }
}
