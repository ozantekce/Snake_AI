using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SnakeNode : MonoBehaviour
{

    private Image image;
    public Image Image { get => image; set => image = value; }



    protected void Awake()
    {
        Transform parent = GameManager.Instance.canvas;
        transform.SetParent(parent);
        image = gameObject.AddComponent<Image>();
        
    }



    public void ChangeSlot(Slot slot)
    {


    }

    public Slot GetCurrentSlot()
    {

        if (Slot.KeyValuePairs.ContainsKey(this))
        {
            return Slot.KeyValuePairs[this];
        }
        else
        {
            return null;
        }

    }


    public void SetCurrentSlot(Slot slot)
    {
        if(slot == null)
        {
            Debug.Log("h");
            return;
        }

        if (Slot.KeyValuePairs.ContainsKey(this))
        {
            Slot.KeyValuePairs[this].SetCurrentNode(null);
            Slot.KeyValuePairs[this] = slot;
            Slot.KeyValuePairs[this].SetCurrentNode(this);
        }
        else
        {
            Slot.KeyValuePairs.Add(this, slot);

        }

        transform.position = slot.transform.position;
    }



}
