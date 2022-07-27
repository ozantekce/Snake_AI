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

    private bool[][] gridArea;
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

    [SerializeField]
    private List<Slot> path = new List<Slot>();
    private int D = 10; //Heuristic distance cost per step
    private Slot lastVisitedNode;

    void FindThePath()
    {
        ResetGridArea();
        Slot startNode = GetCurrentSlot();
        Slot goalNode = GameManager.Instance.foodSlot;

        AStarAlgorithm(startNode, goalNode);
        PathTracer(startNode, goalNode);
    }

    

    void AStarAlgorithm(Slot startNode, Slot goalNode)
    {  

        if (startNode == goalNode)
        {
            return;
        }
        else
        {
            List<Slot> frontier = new List<Slot>();
            startNode.gCost = 0;
            startNode.hCost = Mathf.Abs(startNode.X - goalNode.X) + Mathf.Abs(startNode.Y - goalNode.Y);

            frontier.Add(startNode);
            HashSet<Slot> explored = new HashSet<Slot>();

            while (frontier.Count != 0)
            {

                frontier = frontier.OrderBy(x => x.FCost).ToList();
                Slot state = frontier.First();
                frontier.Remove(state);
                explored.Add(state);

                if (state == goalNode)
                    return;
                else
                {
                    List<Slot> neighbors = state.Neighbors;
                    foreach (Slot neighbor in neighbors)
                    {
                        int tempG = state.gCost + 1;
                        int tempH = GetDistance(neighbor, goalNode);
                        int tempF = tempG + tempH;
                        if (neighbor.GetCurrentNode() ==null && !explored.Contains(neighbor) && !frontier.Contains(neighbor))
                        {
                            neighbor.ParentSlot = state;
                            neighbor.gCost = tempG;
                            neighbor.hCost = tempH;
                            frontier.Add(neighbor);

                        }
                        else if (frontier.Contains(neighbor) && tempF < neighbor.FCost)
                        {

                            neighbor.ParentSlot = state;
                            neighbor.gCost = tempG;
                            neighbor.hCost = tempH;
                        }
                    }
                }
            }
        }

    }


    void PathTracer(Slot startNode, Slot goalNode)
    {
        lastVisitedNode = startNode;
        path.Clear();
        Slot currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.ParentSlot;
        }

        //reverse path to get it sorted right
        path.Reverse();
    }

    int GetDistance(Slot a, Slot b)
    {
        int distX = Mathf.Abs(a.X - b.X);
        int distZ = Mathf.Abs(a.Y - b.Y);

        return D * (distX + distZ);
    }




    public void Move(Direction direction)
    {

        if (!alive)
            return;


        FindThePath();


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
