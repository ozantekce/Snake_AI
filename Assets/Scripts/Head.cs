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

        Debug.Log("-----------------------------------------------------");
        foreach (Slot ls in lastSlots)
        {
            Debug.Log(ls);
        }
        
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


    private int D = 10; //Heuristic distance cost per step
    private Slot lastVisitedNode;

    List<Slot> FindThePath(Slot startNode, Slot goalNode,bool deneme)
    {
        ResetGridArea();
        gridArea[goalNode.X][goalNode.Y] = false;
        gridArea[startNode.X][startNode.Y] = false;
        /*
        Slot startNode = GetCurrentSlot();
        Slot goalNode = GameManager.Instance.foodSlot;
        */
        List<Slot> path = new List<Slot>();

        bool c1 = AStarAlgorithm(startNode, goalNode);
        PathTracer(startNode, goalNode,path);


        if (deneme && lastTail!=null)
        {
            List<Slot> tempPath = new List<Slot>();
            gridArea[denemeSlot.X][denemeSlot.Y] = false;
            gridArea[goalNode.X][goalNode.Y] = false;
            bool c2 = AStarAlgorithm(goalNode, denemeSlot);

            if (!c2)
                return null;
            //PathTracer(goalNode, denemeSlot, tempPath);


        }

        return path;

    }

    private int maxAStar = 9999;
    private Slot denemeSlot;
    bool AStarAlgorithm(Slot startNode, Slot goalNode)
    {  

        if (startNode == goalNode)
        {
            return false;
        }
        else
        {
            List<Slot> snakeSlots = new List<Slot>();
            if (lastTail != null)
            {
                Tail current = lastTail;
                while (current != null)
                {
                    snakeSlots.Add(current.GetCurrentSlot());
                    current = current.NextTail;
                }
            }
            snakeSlots.Add(GetCurrentSlot());


            List<Slot> frontier = new List<Slot>();
            startNode.gCost = 0;
            startNode.hCost = Mathf.Abs(startNode.X - goalNode.X) + Mathf.Abs(startNode.Y - goalNode.Y);

            frontier.Add(startNode);
            HashSet<Slot> explored = new HashSet<Slot>();
            Slot state = null;
            int counter = 0;
            while (frontier.Count != 0)
            {
                counter++;
                if (maxAStar <= counter)
                {
                    Debug.Log("Error 1");
                    return false;
                }

                frontier = frontier.OrderBy(x => x.FCost).ToList();
                
                Slot nextState = frontier.First();
                state = nextState;

                frontier.Remove(state);
                explored.Add(state);

                if (state == goalNode)
                {
                    denemeSlot = snakeSlots[0];
                    return true;

                }
                else
                {

                    List<Slot> neighbors = state.Neighbors;
                    foreach (Slot neighbor in neighbors)
                    {
                        int tempG = state.gCost + 1;
                        int tempH = GetDistance(neighbor, goalNode);
                        int tempF = tempG + tempH;
                        if (gridArea[neighbor.X][neighbor.Y] == false && !explored.Contains(neighbor) && !frontier.Contains(neighbor))
                        {
                            gridArea[neighbor.X][neighbor.Y] = true;
                            if (snakeSlots.Count > 1)
                                gridArea[snakeSlots[snakeSlots.Count - 1].X][snakeSlots[snakeSlots.Count - 1].Y] = false;
                            else
                                gridArea[snakeSlots[0].X][snakeSlots[0].Y] = false;

                            snakeSlots.Add(neighbor);

                            if (snakeSlots.Count > 1)
                            {
                                snakeSlots.RemoveAt(snakeSlots.Count - 1);
                            }

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
            Debug.Log("Error 2");
            return false;
        }

    }


    void PathTracer(Slot startNode, Slot goalNode, List<Slot> path)
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


    [SerializeField]
    List<Slot> pathToFood = new List<Slot>();
    [SerializeField]
    List<Slot> pathToTail = new List<Slot>();


    public void Move(Direction direction)
    {

        if (!alive)
            return;


        if(pathToFood == null || pathToFood.Count == 0)
        {
            pathToFood = FindThePath(GetCurrentSlot(), GameManager.Instance.foodSlot,true);
        }

        if (pathToTail.Count == 0 && lastTail != null)
        {
            Debug.Log("lastt :"+lastSlots.ElementAt(lastSlots.Count - 1));
            pathToTail = FindThePath(GetCurrentSlot(), lastSlots.ElementAt(lastSlots.Count - 1),false);
        }
        

        
        headLastSlot = GetCurrentSlot();


        if(pathToFood != null)
        {
            if (pathToFood[0] == GetCurrentSlot().LeftNeighbor)
            {
                direction = Direction.Left;
            }
            else if (pathToFood[0] == GetCurrentSlot().RightNeighbor)
            {
                direction = Direction.Right;
            }
            else if (pathToFood[0] == GetCurrentSlot().UpNeighbor)
            {
                direction = Direction.Up;
            }
            else if (pathToFood[0] == GetCurrentSlot().DownNeighbor)
            {
                direction = Direction.Down;
            }
            pathToFood.RemoveAt(0);
            

        }
        else
        {
            if (pathToTail[0] == GetCurrentSlot().LeftNeighbor)
            {
                direction = Direction.Left;
            }
            else if (pathToTail[0] == GetCurrentSlot().RightNeighbor)
            {
                direction = Direction.Right;
            }
            else if (pathToTail[0] == GetCurrentSlot().UpNeighbor)
            {
                direction = Direction.Up;
            }
            else if (pathToTail[0] == GetCurrentSlot().DownNeighbor)
            {
                direction = Direction.Down;
            }
            pathToTail.RemoveAt(0);

        }

        pathToTail.Clear();


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

        AddLastSlot();
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
