using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;    
   
    public int rows;
    public int cols;


    public Slot[][] slots;

    public Sprite gridSprite;

    public Sprite headSprite;
    public Sprite tailSprite;
    public Sprite foodSprite;

    public RectTransform leftUpRef;

    public Transform canvas;

    public GameObject tailsGO;

    public Head head;

    public GameObject food;
    
    private void Awake()
    {
        instance = this;

    }



    public bool gameOver = false;
    public bool gameCreated = false;
    public void CreateGame(GameObject button)
    {

        GameObject MenuScreen = button.transform.parent.gameObject;
        InputField rowTextField = MenuScreen.transform.GetChild(0).GetComponent<InputField>();
        InputField colTextField = MenuScreen.transform.GetChild(1).GetComponent<InputField>();
        rows = int.Parse(colTextField.text);
        cols = int.Parse(rowTextField.text);

        gameCreated = true;

        leftUpRef.sizeDelta = new Vector2(Screen.height / (cols * 1f), Screen.width / (rows * 1f));
        leftUpRef.position = new Vector2(leftUpRef.sizeDelta.x / 2, Screen.width - leftUpRef.sizeDelta.y / 2);


        slots = GameBuilder.CreateSlots(canvas, leftUpRef, rows, cols, gridSprite);


        tailsGO = new GameObject("Tails");
        tailsGO.transform.SetParent(canvas);

        GameObject headGO = new GameObject("Head");
        headGO.transform.SetParent(canvas);
        head = headGO.AddComponent<Head>();
        head.SetCurrentSlot(slots[0][0]);



        food = new GameObject("Food");
        food.transform.SetParent(canvas);
        Image img = food.AddComponent<Image>();
        img.sprite = foodSprite;
        img.rectTransform.sizeDelta = leftUpRef.sizeDelta;

        CreateFood();
        

        MenuScreen.SetActive(false);

    }

    private const float MinGameSpeed = 1;
    [Range(1.0f, 100.0f)]
    public float gameSpeed=1;

    private float elapsedTime;


    

    Direction dir = Direction.None;
    private void Update()
    {
        if (gameOver)
            return;
        
        if (!gameCreated)
            return;

        /*
        elapsedTime += Time.deltaTime;
        if (elapsedTime > MinGameSpeed / gameSpeed)
        {
            elapsedTime = 0;
            head.ExecuteRoute();
        }
        */
        
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



    public Slot foodSlot;
    
    public void CreateFood()
    {
        foodSlot = GetRandomSlot();
        food.transform.position = foodSlot.transform.position;
        
    }


    private List<Slot> emptySlots = new List<Slot>();
    private Slot GetRandomSlot()
    {

        emptySlots.Clear();

        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[0].Length; j++)
            {
                if(slots[i][j].GetCurrentNode() == null)
                    emptySlots.Add(slots[i][j]);
            }
        }

        if(emptySlots.Count == 0)
        {
            gameOver = true;
            return null;
        }
        else
        {
            int randomIndex = Random.Range(1, emptySlots.Count);
            return emptySlots[randomIndex];

        }

    }



    public void GameSpeed(string s)
    {

        gameSpeed = int.Parse(s);
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