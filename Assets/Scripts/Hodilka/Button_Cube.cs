using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Cube : MonoBehaviour, IPointerClickHandler
{
    public int Id;
    [SerializeField] private Game_Cube cube;
    [SerializeField] private bool ApplyButton;

    public void PlayerPressed()
    {
        cube.StartCoroutine(cube.StartGame());
        cube.InfoPanel.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ApplyButton == false)
        {
            if (transform.GetChild(0).gameObject.activeSelf)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}