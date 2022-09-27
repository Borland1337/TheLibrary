using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour,IPointerClickHandler
{
    public int id;
    [SerializeField] private Image image;
    [SerializeField] private Sprite back;
    [SerializeField] public Sprite front;

    public GameMemory temp;

    public bool isFront = false;

    public void Start()
    {
        temp=FindObjectOfType<GameMemory>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFront)
        {
            //ShowBack();
        }
        else
        {
            image.sprite = front;
            isFront = true;
            temp.Equals(this);
        }
    }

    public void ShowBack()
    {
        image.sprite = back;
        isFront = false;
    }
}