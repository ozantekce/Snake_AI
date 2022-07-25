using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

   
    public int cols;
    public int rows;


    public Grid[][] grids;

    public Sprite gridSprite;

    public RectTransform leftUpRef;

    private void Awake()
    {

        Transform canvas = FindObjectOfType<Canvas>().transform;

        leftUpRef.sizeDelta = new Vector2(Screen.height/cols, Screen.width/rows);
        leftUpRef.position = new Vector2(leftUpRef.sizeDelta.x/2, Screen.width- leftUpRef.sizeDelta.y/2);
        grids = GridCreater.CreateGrids(canvas, leftUpRef, cols, rows, gridSprite);

    }


    private const float MinGameSpeed = 5;
    [Range(1.0f, 10.0f)]
    public float gameSpeed=1;

    private float elapsedTime;
    
    private void Update()
    {



    }





}
