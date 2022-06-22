using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public Vector3 firstGridPos;

    public float unit;
    public int rows;
    public int cols;

    public Transform ParentTransfrom;


    public Vector3[][] grids;

    public Texture gridTexture,headTexture;


    private int headRow, headCol;
    private GameObject head;


    private void Start()
    {

        grids = new Vector3[rows][];

        for (int i = 0; i < rows; i++)
        {
            grids[i] = new Vector3[cols];
            for (int j = 0; j < cols; j++)
            {
                grids[i][j] = firstGridPos+new Vector3(j*unit,i*unit);
                
                GameObject cube =GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = grids[i][j];
                cube.transform.parent = ParentTransfrom;
                cube.GetComponent<Renderer>().material.mainTexture = gridTexture;
                
            }

        }

        Camera cam = Camera.main;
        cam.transform.position = firstGridPos+new Vector3(
            ((cols-1)*unit - firstGridPos.x)/2f
            ,((rows-1)*unit - firstGridPos.y)/2f,-10);
        
        int size = Mathf.Max(cols, rows);

        cam.orthographicSize = (unit/2)*(size);


        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        headRow = rows / 2;
        headCol = cols / 2;
        head.transform.position = grids[headRow][headCol] +new Vector3(0,0,-1);
        head.GetComponent<Renderer>().material.mainTexture= headTexture;
        this.head = head;


    }


    private const float MinGameSpeed = 5;
    [Range(1.0f, 10.0f)]
    public float gameSpeed=1;

    private float elapsedTime;
    
    private void Update()
    {
        
        elapsedTime += Time.deltaTime;

        if(elapsedTime > MinGameSpeed / gameSpeed)
        {
            elapsedTime = 0;

            Move();

        }


    }


    private Direction lastDirection = Direction.Right;
    
    private void Move()
    {

        int up = (int)Input.GetAxisRaw("Vertical");
        int right = (int)Input.GetAxisRaw("Horizontal");

        Direction dir = Direction.None;
        if (up > 0)
        {
            dir = Direction.Up;
        }else if(up < 0)
        {
            dir=Direction.Down;
        }
        else if (right > 0)
        {
            dir = Direction.Right;
        }
        else if (right < 0)
        {
            dir = Direction.Left;
        }


        if(dir == ~lastDirection || dir == Direction.None)
        {
            dir = lastDirection;
        }
        int oldRow = headRow, oldCol = headCol;
        if (dir == Direction.Right)
        {
            headCol++;
            head.transform.position = grids[headRow][headCol] + new Vector3(0, 0, -1);
        }
        else if (dir == Direction.Left)
        {
            headCol--;
            head.transform.position = grids[headRow][headCol] + new Vector3(0, 0, -1);
        }
        else if (dir == Direction.Up)
        {
            headRow++;
            head.transform.position = grids[headRow][headCol] + new Vector3(0, 0, -1);
        }
        else if (dir == Direction.Down)
        {
            headRow--;
            head.transform.position = grids[headRow][headCol] + new Vector3(0, 0, -1);
        }

        lastDirection = dir;

        MoveTail(oldRow, oldCol);

    }


    private void AddTail(int row,int col)
    {

        if(firstTail == null)
        {
            firstTail = new Tail();
            lastTail = firstTail;
            lastTail.go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        else
        {


        }

    }



    // O(1) complexity 
    private void MoveTail(int row,int col)
    {

        if (firstTail == null)
            return;

        lastTail.row = row;
        lastTail.col = col;

        Tail temp = lastTail;
        firstTail.next = temp;
        lastTail = lastTail.next;
        firstTail = temp;
        firstTail.next = null;
        firstTail.go.transform.position = grids[firstTail.row][firstTail.col] + new Vector3(0, 0, -1);

    }


    private Tail firstTail;
    private Tail lastTail;

    private class Tail
    {

        public int row,col;
        public Tail next;
        public GameObject go;

    }


    public enum Direction
    {
        None =0,
        Up = 1,
        Down = ~Up,
        Right = 2,
        Left = ~Right,

    }

}
