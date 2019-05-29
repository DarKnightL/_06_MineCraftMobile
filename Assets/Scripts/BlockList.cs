using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour
{

    public static List<Block> blockList = new List<Block>();
    public Texture dirtTexture;


    void Start()
    {

    }


    void Update()
    {

    }

    private void Awake()
    {
       
        Block dirt = new Block("dirt", 2, 15, 2, 15, 2, 15);
        Block grass = new Block("grass", 3, 15, 0, 15, 2, 15);
        blockList.Add(dirt);
        blockList.Add(grass);

        dirt.SetTexture(dirtTexture);
      
    }


    public static Block GetBlock(string name)
    {
        foreach (Block b in blockList)
        {
            if (name == b.name)
            {
                return b;
            }
        }
        return null;
    }


  

}
