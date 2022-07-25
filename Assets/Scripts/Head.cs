using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    [SerializeField]
    private Grid currentGrid;
    private Grid lastGrid;

    private Direction lastDirection = Direction.Right;


    private Image image;
    public Grid CurrentGrid { get => currentGrid; set => currentGrid = value; }
    public Image Image { get => image; set => image = value; }

    private bool alive = true;

    private void Awake()
    {
        image = gameObject.AddComponent<Image>();

    }

    public void Move(Direction direction)
    {
        if (!alive)
            return;



        lastGrid = currentGrid;
        if(lastDirection == ~direction || direction == Direction.None)
        {
            direction = lastDirection;
        }

        if(direction == Direction.Left)
        {
            if(currentGrid.LeftNeighbor != null && currentGrid.LeftNeighbor.Empty)
            {
                currentGrid.Empty = true;
                currentGrid = currentGrid.LeftNeighbor;
                transform.position = currentGrid.transform.position;
                currentGrid.Empty = false;
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Right)
        {
            if (currentGrid.RightNeighbor != null && currentGrid.RightNeighbor.Empty)
            {
                currentGrid.Empty = true;
                currentGrid = currentGrid.RightNeighbor;
                transform.position = currentGrid.transform.position;
                currentGrid.Empty = false;
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Up)
        {
            if (currentGrid.UpNeighbor != null && currentGrid.UpNeighbor.Empty)
            {
                currentGrid.Empty = true;
                currentGrid = currentGrid.UpNeighbor;
                transform.position = currentGrid.transform.position;
                currentGrid.Empty = false;
            }
            else
            {
                Die();
                return;
            }
        }
        else if(direction == Direction.Down)
        {
            if (currentGrid.DownNeighbor != null && currentGrid.DownNeighbor.Empty)
            {
                currentGrid.Empty = true;
                currentGrid = currentGrid.DownNeighbor;
                transform.position = currentGrid.transform.position;
                currentGrid.Empty = false;
            }
            else
            {
                Die();
                return;
            }
        }
        lastDirection = direction;

        if(GameManager.Instance.foodGrid == currentGrid)
        {
            if (lastTail != null)
                AddTail(lastTail.grid);
            else
                AddTail(lastGrid);

            GameManager.Instance.CreateFood();
        }


        MoveTail(lastGrid);

    }


    private void Die()
    {
        alive = false;
    }

    private void AddTail(Grid grid)
    {

        if (firstTail == null)
        {
            Transform canvas = FindObjectOfType<Canvas>().transform;
            firstTail = new Tail();
            lastTail = firstTail;
            lastTail.go = new GameObject();
            Image img = lastTail.go.AddComponent<Image>();
            img.sprite = GameManager.Instance.tailSprite;
            img.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
            lastTail.go.transform.SetParent(canvas);
            lastTail.go.transform.position = grid.transform.position;
            lastTail.grid = grid;
            lastTail.grid.Empty = false;

        }
        else
        {
            Transform canvas = FindObjectOfType<Canvas>().transform;
            Tail temp = new Tail();
            temp.go = new GameObject();
            Image img = temp.go.AddComponent<Image>();

            img.sprite= GameManager.Instance.tailSprite;
            img.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
            temp.go.transform.SetParent(canvas);
            temp.go.transform.position = grid.transform.position;

            temp.next = lastTail;
            lastTail = temp;
            lastTail.grid = grid;
            lastTail.grid.Empty = false;

        }

    }


    // O(1) complexity 
    private void MoveTail(Grid grid)
    {

        if (firstTail == null)
            return;

        if(lastTail.grid != null)
            lastTail.grid.Empty = true;

        lastTail.grid = lastGrid;
        lastTail.go.transform.position = lastGrid.transform.position;

        lastGrid.Empty = false;

        Tail temp = lastTail;
        firstTail.next = temp;
        lastTail = lastTail.next;
        firstTail = temp;
        firstTail.next = null;
    
    }


    private Tail firstTail;
    private Tail lastTail;

    private class Tail
    {

        public Grid grid;
        public Tail next;
        public GameObject go;

    }


}
