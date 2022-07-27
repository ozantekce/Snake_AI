using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Head : SnakeNode
{

    private Direction lastDirection = Direction.Right;
    [SerializeField]
    private bool alive = true;

    private Tail firstTail, lastTail;

    private Slot headLastSlot;

    private int limit = 5;
    private Queue<Slot> lastSlots = new Queue<Slot>();

    private void AddLastSlot()
    {
        if (lastTail == null)
        {
            lastSlots.Enqueue(GetCurrentSlot());
            if(lastSlots.Count > limit) { lastSlots.Dequeue(); }
        }
        else
        {
            lastSlots.Enqueue(lastTail.GetCurrentSlot());
            if (lastSlots.Count > limit) { lastSlots.Dequeue(); }
        }

        /*
        foreach (Slot ls in lastSlots)
        {
            Debug.Log(ls);
        }
        */
    }


    private void Awake()
    {
        base.Awake();
        Image.sprite = GameManager.Instance.headSprite;
        Image.rectTransform.sizeDelta = GameManager.Instance.leftUpRef.sizeDelta;


         gridArea = new bool[GameManager.Instance.rows][];
        for (int i = 0; i < gridArea.Length; i++)
        {
            gridArea[i] = new bool[GameManager.Instance.cols];
        }
    }


    private bool[][] gridArea;

    private void ResetGridArea()
    {
        for (int i = 0; i < gridArea.Length; i++)
        {
            for (int j = 0; j < gridArea[i].Length; j++)
            {
                if (GameManager.Instance.slots[j][i].GetCurrentNode() == null)
                {
                    gridArea[j][i] = false;
                }
                else
                {
                    gridArea[j][i] = true;
                }
            }
        }

    }

    private List<Slot> GetSnakeSlots()
    {
        List<Slot> slots = new List<Slot>();
        
        if (lastTail != null)
        {
            Tail current = lastTail;
            while(current!=null)
            {
                slots.Add(current.GetCurrentSlot());
                current = current.NextTail;
            }
        }
        slots.Add(GetCurrentSlot());
        return slots;

    }


    private void ExecuteMovementOnGridArea(Direction dir, List<Slot> snakeSlots)
    {
        
        if(dir == Direction.Left)
        {
            gridArea[snakeSlots[0].LeftNeighbor.X][snakeSlots[0].LeftNeighbor.Y] = true;
            if(snakeSlots.Count>1)
                gridArea[snakeSlots[snakeSlots.Count-1].X][snakeSlots[snakeSlots.Count-1].Y] = false;
            else
                gridArea[snakeSlots[0].X][snakeSlots[0].Y] = false;

            snakeSlots.Add(snakeSlots[0]);

            if (snakeSlots.Count > 1)
            {
                snakeSlots.RemoveAt(snakeSlots.Count-1);
            }
        }
        else if(dir == Direction.Right)
        {
            gridArea[snakeSlots[0].RightNeighbor.X][snakeSlots[0].RightNeighbor.Y] = true;
            if (snakeSlots.Count > 1)
                gridArea[snakeSlots[snakeSlots.Count - 1].X][snakeSlots[snakeSlots.Count - 1].Y] = false;
            else
                gridArea[snakeSlots[0].X][snakeSlots[0].Y] = false;

        }
        else if(dir == Direction.Up)
        {
            gridArea[snakeSlots[0].UpNeighbor.X][snakeSlots[0].UpNeighbor.Y] = true;
            if (snakeSlots.Count > 1)
                gridArea[snakeSlots[snakeSlots.Count - 1].X][snakeSlots[snakeSlots.Count - 1].Y] = false;
            else
                gridArea[snakeSlots[0].X][snakeSlots[0].Y] = false;

        }
        else if(dir == Direction.Down) 
        {
            gridArea[snakeSlots[0].DownNeighbor.X][snakeSlots[0].DownNeighbor.Y] = true;
            if (snakeSlots.Count > 1)
                gridArea[snakeSlots[snakeSlots.Count - 1].X][snakeSlots[snakeSlots.Count - 1].Y] = false;
            else
                gridArea[snakeSlots[0].X][snakeSlots[0].Y] = false;

        }

    }
    



    public void Move(Direction direction)
    {

        if (!alive)
            return;


        AddLastSlot();
        headLastSlot = GetCurrentSlot();

        if (lastDirection == ~direction || direction == Direction.None)
        {
            direction = lastDirection;
        }

        if(direction == Direction.Left)
        {
            if(GetCurrentSlot().LeftNeighbor != null && GetCurrentSlot().LeftNeighbor.GetCurrentNode() == null)
            {
                SetCurrentSlot(GetCurrentSlot().LeftNeighbor);
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Right)
        {
            if (GetCurrentSlot().RightNeighbor != null && GetCurrentSlot().RightNeighbor.GetCurrentNode() == null)
            {
                SetCurrentSlot(GetCurrentSlot().RightNeighbor);
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Up)
        {
            if (GetCurrentSlot().UpNeighbor != null && GetCurrentSlot().UpNeighbor.GetCurrentNode() == null)
            {
                SetCurrentSlot(GetCurrentSlot().UpNeighbor);
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Down)
        {
            if (GetCurrentSlot().DownNeighbor != null && GetCurrentSlot().DownNeighbor.GetCurrentNode() == null)
            {
                SetCurrentSlot(GetCurrentSlot().DownNeighbor);
            }
            else
            {
                Die();
                return;
            }
        }
        lastDirection = direction;

        MoveTails();
        bool foodAte = false;
        if (GameManager.Instance.foodSlot == GetCurrentSlot())
        {
            AddTail();
            foodAte = true;
        }


        

        if (foodAte)
        {
            GameManager.Instance.CreateFood();
        }
        
    }



    private void Die()
    {
        alive = false;
    }


    private void AddTail()
    {


        if (firstTail == null)
        {
            GameObject tempGO = new GameObject();
            firstTail = tempGO.AddComponent<Tail>();
            lastTail = firstTail;
            lastTail.SetCurrentSlot(lastSlots.ElementAt(lastSlots.Count - 1));
            lastTail.transform.SetParent(GameManager.Instance.tailsGO.transform);
        }
        else
        {
            GameObject tempGO = new GameObject();
            tempGO.transform.SetParent(GameManager.Instance.tailsGO.transform);
            Tail temp = tempGO.AddComponent<Tail>();
            temp.NextTail = lastTail;
            temp.SetCurrentSlot(lastSlots.ElementAt(lastSlots.Count - 1));
            lastTail = temp;
            lastTail.transform.SetParent(GameManager.Instance.tailsGO.transform);
        }

    }

    // O(1) complexity 
    private void MoveTails()
    {
        
        if (firstTail == null)
            return;
        
        Tail temp = lastTail;
        firstTail.NextTail = temp;
        lastTail = lastTail.NextTail;
        firstTail = temp;
        firstTail.NextTail = null;
        firstTail.SetCurrentSlot(headLastSlot);

    }



    



}
