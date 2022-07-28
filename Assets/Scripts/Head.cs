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
        Debug.Log("-----------------------------------------------------");
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

    }



    private int D = 10; //Heuristic distance cost per step
    private Slot lastVisitedNode;

    List<Slot> FindThePath(Slot startNode, Slot goalNode, bool [][] gridArea)
    {

        List<Slot> path = new List<Slot>();

        AStarAlgorithm(startNode, goalNode);

        PathTracer(startNode,goalNode,path);

        return path;

    }

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
            while (frontier.Count != 0)
            {


                frontier = frontier.OrderBy(x => x.FCost).ToList();
                
                Slot nextState = frontier.First();
                state = nextState;

                frontier.Remove(state);
                explored.Add(state);

                if (state == goalNode)
                {
                    return true;
                }
                else
                {

                    List<Slot> neighbors = state.Neighbors;
                    ResetGridArea();

                    snakeSlots = new List<Slot>();
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

                    foreach (Slot neighbor in neighbors)
                    {
                        int tempG = state.gCost + 1;
                        int tempH = GetDistance(neighbor, goalNode);
                        int tempF = tempG + tempH;
                        if ( (gridArea[neighbor.X][neighbor.Y] == false || snakeSlots[snakeSlots.Count-1] == neighbor)
                            
                            && !explored.Contains(neighbor) && !frontier.Contains(neighbor))
                        {
                            gridArea[neighbor.X][neighbor.Y] = true;

                            gridArea[snakeSlots[snakeSlots.Count - 1].X][snakeSlots[snakeSlots.Count - 1].Y] = false;

                            snakeSlots.Add(neighbor);
                            snakeSlots.RemoveAt(snakeSlots.Count - 1);

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




    private bool firstTime = true;



    private List<Slot> path1;
    private bool path1Finded;
    private bool[][] gridArea1;

    // path1 bulundugu zaman tail in konumu
    private Slot path2Goal;

    // path from current pos to food
    private void FindPath1()
    {
        path1Finded = false;
        Slot first = GetCurrentSlot();
        Slot goal = GameManager.Instance.foodSlot;
        gridArea1 = CreateGridArea1();
        path1 = FindThePath(first, goal, gridArea1);

        if(path1 != null)
        {
            path1Finded = true;
        }

    }

    private bool[][] CreateGridArea1()
    {

        return null;
    }



    private List<Slot> path2;
    private bool path2Finded;
    private bool[][] gridArea2;
    // path from food pos to tail pos
    private void FindPath2()
    {
        path2Finded = false;
        Slot first = GameManager.Instance.foodSlot;
        Slot goal = path2Goal;
        gridArea2 = CreateGridArea2();
        path2 = FindThePath(first, goal, gridArea2);

        if (path2 != null)
        {
            path2Finded = true;
        }

    }

    private bool[][] CreateGridArea2()
    {

        return null;
    }




    // path from current pos to tail pos
    private List<Slot> path3;
    private bool path3Finded;
    private bool[][] gridArea3;
    private void FindPath3()
    {

        path3Finded = false;
        Slot first = GetCurrentSlot();
        Slot goal = lastSlots.ElementAt(lastSlots.Count - 1);
        gridArea3 = CreateGridArea3();
        path3 = FindThePath(first, goal, gridArea3);

        if (path3 != null)
        {
            path3Finded = true;
        }

    }

    private bool[][] CreateGridArea3()
    {

        return null;
    }


    private void ExecutePath1()
    {


    }

    private void ExecutePath2()
    {


    }

    private void ExecutePath3()
    {


    }



    public void Move(Direction direction)
    {

        if (!alive)
            return;


        if (firstTime)
        {
            FindPath1();
            ExecutePath1();
        }
        else
        {
            FindPath1();
            if (path1Finded)
            {
                FindPath2();
                if (path2Finded)
                {
                    ExecutePath1();
                }
                else
                {
                    FindPath3();
                    ExecutePath3();
                }
            }
            else
            {
                FindPath3();
                ExecutePath3();
            }

        }






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
