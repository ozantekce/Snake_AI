using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{

    private Image image;
    private int x, y;


    private void Awake()
    {
        image = gameObject.AddComponent<Image>();
    }


    public RectTransform RectTransform
    {

        get => image.rectTransform;
    }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public Image Image { get => image; set => image = value; }
}
