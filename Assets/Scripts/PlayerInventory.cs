using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private Texture2D slotBackground;
    [SerializeField]
    private int width = 9;
    [SerializeField]
    private int height = 5;

    public Block[,] inventoryItems;
    public int[,] inventoryNums;


    bool showInventory;

    void Start()
    {
        inventoryItems = new Block[width, height];
        inventoryNums = new int[width, height];
        inventoryItems[0, 0] = BlockList.GetBlock("dirt");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            showInventory =! showInventory;
        }
    }


    private void OnGUI()
    {
        if (showInventory==false)
        {
            return;
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int inventoryWidth = slotBackground.width * width;
                int inventoryHeight = slotBackground.height * height;
                Rect offset = new Rect(Screen.width / 2 - inventoryWidth / 2, Screen.height / 2 - inventoryHeight / 2, inventoryWidth, inventoryHeight);
                Rect slotPos = new Rect(offset.x + slotBackground.width * x, offset.y + slotBackground.height * y, slotBackground.width, slotBackground.height);

                GUI.DrawTexture(slotPos,slotBackground);
            }
        }
    }
}
