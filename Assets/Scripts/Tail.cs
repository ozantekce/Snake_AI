using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : SnakeNode
{

    private Tail nextTail;


    private void Awake()
    {
        base.Awake();
        Image.sprite = GameManager.Instance.tailSprite;
        Image.rectTransform.sizeDelta = GameManager.Instance.leftUpRef.sizeDelta;

    }

    public Tail NextTail { get => nextTail; set => nextTail = value; }

}
