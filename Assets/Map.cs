using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    GameObject head,tail,tail1;
    
    Queue<GameObject> queue;

    GameObject[][] points;


    const int rows = 10;
    const int cols = 22;


    void Start()
    {

        points = new GameObject[rows][];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new GameObject[cols];
            for (int j = 0; j < points[i].Length; j++)
            {
                points[i][j] = transform.GetChild(i).transform.GetChild(j).gameObject;
            }
        }
        queue = new Queue<GameObject>();
        queue.Enqueue(tail1);
        queue.Enqueue(tail);
        
        head.transform.position = points[headX][headY].transform.position;
        tail.transform.position = points[0][1].transform.position;
        tail1.transform.position = points[0][0].transform.position;

    }
    

    enum Direction
    {
        up,down,left,right
    }
    private Direction currentDirection = Direction.left;


    int headX=0,headY=2;

    float time=0;
    float delay=1f;

    void Update()
    {
        InputMethod();

        time+=Time.deltaTime;
        if (time > delay)
        {
            Move();
            time = 0;
        }

    }


    private void InputMethod()
    {

        if(currentDirection == Direction.left || currentDirection == Direction.right)
        {
            float v = Input.GetAxisRaw("Vertical");
            if(v == 1)
                currentDirection = Direction.up;
            else if(v==-1)
                currentDirection = Direction.down;
        }
        else
        {
            float h = Input.GetAxisRaw("Horizontal");
            if (h == 1)
                currentDirection = Direction.left;
            else if (h == -1)
                currentDirection = Direction.right;
        }


    }

    private void Move(){

        GameObject last = queue.Dequeue();
        last.transform.position = points[headX][headY].transform.position;
        queue.Enqueue(last);
        if (currentDirection == Direction.left)
        {
            headY++;
        }else if(currentDirection == Direction.right)
        {
            headY--;
        }else if(currentDirection == Direction.up)
        {
            headX--;
        }
        else
        {
            headX++;
        }

        if(headX >= rows || headX < 0)
        {
            Debug.Log("border");
            return;
        }
        if (headY >= cols || headY < 0)
        {
            Debug.Log("border");
            return;
        }

        head.transform.position = points[headX][headY].transform.position;

    }




}
