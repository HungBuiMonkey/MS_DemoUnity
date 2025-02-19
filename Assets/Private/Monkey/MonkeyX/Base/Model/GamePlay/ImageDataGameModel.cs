using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataGameModel 
{
    public int ID { get; private set; }
    public Sprite Sprite { get; private set; }
    public int CategoryID { get; private set; }

    public ImageDataGameModel(int iD, Sprite sprite, int categoryID)
    {
        ID = iD;
        Sprite = sprite;
        CategoryID = categoryID;
    }
}
