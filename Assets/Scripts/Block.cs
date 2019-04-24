using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{

    public string name;

    public int textureX, textureY, textureX_Top, textureY_Top, textureX_Bottom, textureY_Bottom;


    public Block()
    {
        name = null;
    }


    public Block(string name, int tx, int ty,int txt,int tyt,int txb, int tyb) {
        this.name = name;
        this.textureX = tx;
        this.textureY = ty;
        this.textureX_Top = txt;
        this.textureY_Top = tyt;
        this.textureX_Bottom = txb;
        this.textureY_Bottom = tyb;
    }
}
