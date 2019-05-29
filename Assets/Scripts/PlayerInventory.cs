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

    Block dragItem;
    int dragItemNum;


    bool showInventory;

    void Start()
    {
        inventoryItems = new Block[width, height];
        inventoryNums = new int[width, height];
        inventoryItems[5, 3] = BlockList.GetBlock("dirt");
        inventoryNums[5, 3] = 64;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            showInventory = !showInventory;
        }
    }


    private void OnGUI()
    {
        Event e = Event.current;
        float space = 5;

        if (showInventory == false)
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

                GUI.DrawTexture(slotPos, slotBackground);

                Block b = inventoryItems[x, y];
                int n = inventoryNums[x, y];

                if (b != null)
                {

                    Rect blockPos = new Rect(slotPos.x + space / 2, slotPos.y + space / 2, slotPos.width - space, slotPos.height - space);
                    GUI.DrawTexture(blockPos, b.itemView);
                    GUI.Label(slotPos, n.ToString());
                    if (slotPos.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
                    {
                        //Debug.Log("MouseInUI");
                        HideDragItem(x, y);
                    }
                }
                else
                if (slotPos.Contains(e.mousePosition) && e.type == EventType.MouseDown && e.button == 0)
                {
                    ShowDragItem(x, y);
                }





            }
        }

        ShowDragItemWithMousePosition(e, space);
    }



    void HideDragItem(int x, int y)
    {

        dragItem = inventoryItems[x, y];
        dragItemNum = inventoryNums[x, y];

        inventoryItems[x, y] = null;
        inventoryNums[x, y] = 0;

    }


    void ShowDragItemWithMousePosition(Event e, float space)
    {
        if (dragItem != null)
        {
            GUI.DrawTexture(new Rect(e.mousePosition.x, e.mousePosition.y, slotBackground.width - space, slotBackground.height - space), dragItem.itemView);
            GUI.Label(new Rect(e.mousePosition.x, e.mousePosition.y, slotBackground.width - space, slotBackground.height - space), dragItemNum.ToString());
        }

    }


    void ShowDragItem(int x, int y)
    {
        if (dragItem != null)
        {
            inventoryItems[x, y] = dragItem;
            inventoryNums[x, y] = dragItemNum;

            dragItem = null;
            dragItemNum = 0;
        }

    }
}
