using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{

    private Image image;
    [SerializeField]
    private int x,y;

    [SerializeField]
    private Grid upNeighbor, downNeighbor , leftNeighbor, rightNeighbor;

    [SerializeField]
    private bool empty;
    

    private void Awake()
    {
        image = gameObject.AddComponent<Image>();
        empty = true;

    }

    private void Start()
    {
        SetNeighbors();

    }

    private void SetNeighbors()
    {

        leftNeighbor = GetNeighbor(-1,0);
        rightNeighbor = GetNeighbor(1,0);
        upNeighbor = GetNeighbor(0,-1);
        downNeighbor = GetNeighbor(0,1);

    }


    private Grid GetNeighbor(int deltaX, int deltaY)
    {
        int nX = X+deltaX, nY = Y+deltaY;

        if(nY >= 0 && nY < GameManager.Instance.cols
            && nX >= 0 && nX < GameManager.Instance.rows)
        {
            return GameManager.Instance.grids[nX][nY];
        }
        else
        {
            return null;
        }

    }

    public RectTransform RectTransform
    {

        get => image.rectTransform;
    }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public Image Image { get => image; set => image = value; }
    public Grid UpNeighbor { get => upNeighbor; set => upNeighbor = value; }
    public Grid DownNeighbor { get => downNeighbor; set => downNeighbor = value; }
    public Grid LeftNeighbor { get => leftNeighbor; set => leftNeighbor = value; }
    public Grid RightNeighbor { get => rightNeighbor; set => rightNeighbor = value; }
    public bool Empty { get => empty; set => empty = value; }
}
