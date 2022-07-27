using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    private Image image;
    [SerializeField]
    private int x,y;

    public int gCost;
    public int hCost;

    public int FCost
    {
        get { return gCost + hCost; }
    }


    [SerializeField]
    private Slot upNeighbor, downNeighbor , leftNeighbor, rightNeighbor;

    private List<Slot> neighbors;

    private Slot parentSlot;

    [SerializeField]
    private SnakeNode currentNode;

    
    private SnakeNode savedNode;


    private readonly static Dictionary<SnakeNode, Slot> keyValuePairs = CreateKeyValuePair();
    private static Dictionary<SnakeNode,Slot> CreateKeyValuePair()
    {
        Dictionary<SnakeNode, Slot> temp = new Dictionary<SnakeNode, Slot>();
        return temp;
    }


    public void ReloadSavedNode()
    {
        currentNode = savedNode;
    }

    private void Awake()
    {
        image = gameObject.AddComponent<Image>();

    }

    private void Start()
    {
        neighbors = new List<Slot>();
        if (UpNeighbor != null)
            neighbors.Add(upNeighbor);
        if (DownNeighbor != null)
            neighbors.Add(downNeighbor);
        if (LeftNeighbor != null)
            neighbors.Add(leftNeighbor);
        if (RightNeighbor != null)
            neighbors.Add(rightNeighbor);

    }

    public SnakeNode GetCurrentNode()
    {
        return currentNode;
    }

    public void SetCurrentNode(SnakeNode node)
    {
        currentNode = node;
    }



    public RectTransform RectTransform
    {

        get => image.rectTransform;
    }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public Image Image { get => image; set => image = value; }
    public Slot UpNeighbor { get => upNeighbor; set => upNeighbor = value; }
    public Slot DownNeighbor { get => downNeighbor; set => downNeighbor = value; }
    public Slot LeftNeighbor { get => leftNeighbor; set => leftNeighbor = value; }
    public Slot RightNeighbor { get => rightNeighbor; set => rightNeighbor = value; }

    public static Dictionary<SnakeNode, Slot> KeyValuePairs => keyValuePairs;

    public List<Slot> Neighbors { get => neighbors; set => neighbors = value; }
    public Slot ParentSlot { 
        
        get => parentSlot;

        set
        { 
         parentSlot = value;

        }
    
    }



}
