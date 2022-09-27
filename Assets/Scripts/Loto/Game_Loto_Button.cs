using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game_Loto_Button : MonoBehaviour
{
    [SerializeField] private GameObject Checkmark;
    [SerializeField] private Color TrueColor;
    [SerializeField] private Color FalseColor;
    private Game_Loto Game_Loto;
    [HideInInspector] public bool isTrue;

    private void Start()
    {
        Game_Loto = FindObjectOfType<Game_Loto>();
    }

    public void ButtonClick()
    {
        if (isTrue)
        {
            Game_Loto.ButtonInteractable();
            Checkmark.SetActive(true);
            Checkmark.GetComponent<Image>().color = TrueColor;
            StartCoroutine(WaitSeconds(2f));
        }
        else
        {
            Game_Loto.Mistakes++;
            Checkmark.SetActive(true);
            Checkmark.GetComponent<Image>().color = FalseColor;
        }
    }

    private IEnumerator WaitSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Game_Loto.Next();
    }
}