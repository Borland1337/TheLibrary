using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMultiPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown PlayerCount;
    [SerializeField] private Transform Content;
    [SerializeField] private Game_Cube game_Cube;
    [SerializeField] private Toggle toggle;

    private void Start()
    {
        PlayerCount.onValueChanged.AddListener(delegate { OnPlayersChange(); });
    }

    private void OnPlayersChange()
    {
        foreach (Transform item in Content)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerCount.value + 1; i++)
        {
            Content.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void SetPlayerCount()
    {
        string[] PlayerNames = new string[4];

        for (int i = 0; i < Content.childCount; i++)
        {
            string currentPlayer = Content.GetChild(i).GetComponent<TMP_InputField>().text;

            if (Content.GetChild(i).gameObject.activeSelf)
            {
                if (currentPlayer == string.Empty)
                {
                    currentPlayer = "Игрок " + (i + 1);
                }
                PlayerNames[i] = currentPlayer;
            }
        }
        game_Cube.SetPlayer(PlayerNames,toggle.isOn);
    }
}