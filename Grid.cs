using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    int gridWidth;
    int gridHeight;
    float tileWidth;
    float tileHeight;
    GameObject[,] tileObjects;

   public Grid(int p_width, int p_height, GameObject p_tile)
    {
        this.gridWidth = p_width;
        this.gridHeight = p_height;
        this.tileWidth = p_tile.GetComponent<SpriteRenderer>().bounds.size.x;
        this.tileHeight = p_tile.GetComponent<SpriteRenderer>().bounds.size.y;
        this.tileObjects = new GameObject[gridWidth, gridHeight];
    }
    
    bool validGridPosition(int gridXVal, int gridYVal)
    {
        if(gridXVal < gridWidth && gridXVal > 0 && gridYVal < gridHeight && gridYVal > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int roundWorldSpace(float worldPosition, float tileDimension)
    {
        float displacement = worldPosition % tileDimension;
        float roundedXWorldSpace = (int)(worldPosition / tileDimension);
        if (displacement > tileDimension / 2)
            return (int)(roundedXWorldSpace * tileDimension + tileDimension);
        else
            return (int)(roundedXWorldSpace * tileDimension);
    }

    public void spawnGridValue(float worldXPos, float worldYPos, GameObject newTile)
    {
        int gridXIndex = roundWorldSpace(worldXPos, tileWidth);
        int gridYIndex = roundWorldSpace(worldYPos, tileHeight);
        if (validGridPosition(gridXIndex, gridYIndex))
        {
            ////Need someway of overwriting the value in the array and deleting the realworld item too
            if (tileObjects[gridXIndex, gridYIndex] != null)
            {
                Destroy(tileObjects[gridXIndex, gridYIndex]);
            }
            tileObjects[gridXIndex, gridYIndex] = newTile;
            Instantiate(tileObjects[gridXIndex, gridYIndex], new Vector2(gridXIndex * tileWidth, gridYIndex * tileHeight), newTile.transform.rotation);
        }
    }

    public void destroyGridValue(float worldXPos, float worldYPos)
    {
        int gridXIndex = roundWorldSpace(worldXPos, tileWidth);
        int gridYIndex = roundWorldSpace(worldYPos, tileHeight);
        if (validGridPosition(gridXIndex, gridYIndex))
        {
            if (tileObjects[gridXIndex, gridYIndex] != null)
            {
                Destroy(tileObjects[gridXIndex, gridYIndex]);
            }
        }
    }
}
