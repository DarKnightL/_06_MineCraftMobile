using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLiList : MonoBehaviour
{

    public static List<Block> blockList = new List<Block>();

    void Start()
    {
        
    }

  
    void Update()
    {
        
    }

    private void Awake()
    {
        Block dirt = new Block("dirt", 2, 15, 2, 15, 2, 15);
    }
}
