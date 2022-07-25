using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;    
   
    public int cols;
    public int rows;


    public Grid[][] grids;

    public Sprite gridSprite;

    public Sprite headSprite;
    public Sprite tailSprite;

    public RectTransform leftUpRef;



    public Head head;
    private void Awake()
    {
        instance = this;

    }


    private void Start()
    {
        //Transform canvas = FindObjectOfType<Canvas>().transform;


    }

    private bool gameCreated = false;
    public void CreateGame(GameObject button)
    {

        GameObject MenuScreen = button.transform.parent.gameObject;
        InputField rowTextField = MenuScreen.transform.GetChild(0).GetComponent<InputField>();
        InputField colTextField = MenuScreen.transform.GetChild(1).GetComponent<InputField>();
        cols = int.Parse(colTextField.text);
        rows = int.Parse(rowTextField.text);

        gameCreated = true;
        Transform canvas = FindObjectOfType<Canvas>().transform;

        leftUpRef.sizeDelta = new Vector2(Screen.height / (rows * 1f), Screen.width / (cols * 1f));
        leftUpRef.position = new Vector2(leftUpRef.sizeDelta.x / 2, Screen.width - leftUpRef.sizeDelta.y / 2);


        grids = GridCreater.CreateGrids(canvas, leftUpRef, cols, rows, gridSprite);



        GameObject headGO = new GameObject("Head");
        headGO.transform.SetParent(canvas);
        head = headGO.AddComponent<Head>();
        head.CurrentGrid = grids[0][0];
        head.transform.position = head.CurrentGrid.transform.position;
        head.Image.sprite = headSprite;
        head.Image.rectTransform.sizeDelta = leftUpRef.sizeDelta;

        MenuScreen.SetActive(false);

    }

    private const float MinGameSpeed = 5;
    [Range(1.0f, 10.0f)]
    public float gameSpeed=1;

    private float elapsedTime;



    Direction dir = Direction.None;
    private void Update()
    {
        if (!gameCreated)
            return;

        int up = (int)Input.GetAxisRaw("Vertical");
        int right = (int)Input.GetAxisRaw("Horizontal");

        if (up > 0)
        {
            dir = Direction.Up;
        }
        else if (up < 0)
        {
            dir = Direction.Down;
        }
        else if (right > 0)
        {
            dir = Direction.Right;
        }
        else if (right < 0)
        {
            dir = Direction.Left;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime > MinGameSpeed / gameSpeed)
        {
            elapsedTime = 0;
            head.Move(dir);
            dir = Direction.None;
        }

    }




    public static GameManager Instance { get => instance; set => instance = value; }


}
public enum Direction
{
    None = 0,
    Up = 1,
    Down = ~Up,
    Right = 2,
    Left = ~Right,

}