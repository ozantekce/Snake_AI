using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreater : MonoBehaviour
{

    public static Grid[][] CreateGrids(Transform canvas, RectTransform leftUpRef, int numberOfColumns
        , int numberOfRows, Sprite sprite)
    {

        GameObject gridGO = new GameObject("Grids");
        gridGO.transform.SetParent(canvas);
        gridGO.transform.position= leftUpRef.position;
        Grid[][] grids = new Grid[numberOfRows][];
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i] = new Grid[numberOfColumns];
        }

        float deltaX = leftUpRef.rect.width;
        float deltaY = -leftUpRef.rect.height;

        float currentX = leftUpRef.transform.position.x, currentY = leftUpRef.transform.position.y;

        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                GameObject go = new GameObject("Grid" + (j + (i * numberOfRows)));
                grids[j][i] = go.AddComponent<Grid>();
                grids[j][i].X = j;
                grids[j][i].Y = i;
                grids[j][i].transform.SetParent(gridGO.transform);
                grids[j][i].transform.position = new Vector3(currentX, currentY, 0);
                grids[j][i].RectTransform.sizeDelta = leftUpRef.sizeDelta;
                grids[j][i].Image.sprite = sprite;
                
                currentX += deltaX;


            }
            currentX = leftUpRef.transform.position.x;
            currentY += deltaY;

        }

        return grids;

    }



}
