using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Loto : MonoBehaviour, ISceneLoader
{
    [SerializeField] private Transform ContentPage;
    [SerializeField] private GameObject PrefabLoto;
    [SerializeField] private int MaxQuestions = 4;
    [SerializeField] private TextMeshProUGUI AnnounceText;
    [SerializeField] private TextMeshProUGUI WinText;
    [SerializeField] private TextMeshProUGUI StatsText;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    private List<Sprite> UsedSprites = new List<Sprite>();
    [SerializeField] private List<Questions> Loto = new List<Questions>();
    [SerializeField] private GameObject ReturnButtons;
    [SerializeField] private GameObject Particles;
    [HideInInspector] public int Mistakes;
    private int RandomQuestion;
    private int QuestionsCount;

    private void Start()
    {
        QuestionsCount = Loto.Count;
        PlayGame();
    }

    private void PlayGame()
    {
        int randomSpawnTrue = Random.Range(0, MaxQuestions);
        UsedSprites.Clear();

        for (int b = 0; b < ContentPage.childCount; b++)
        {
            Destroy(ContentPage.GetChild(b).gameObject);
        }

        UsedSprites.AddRange(sprites);

        RandomQuestion = Random.Range(0, Loto.Count);

        UsedSprites.Remove(Loto[RandomQuestion].TrueIcon);

        for (int i = 0; i < MaxQuestions; i++)
        {
            if (i == randomSpawnTrue)
            {
                SpawnTrue();
            }
            if (i < 3)
            {
                int RandomQuestionFalse = Random.Range(0, UsedSprites.Count);
                GameObject spawnedLotoFalse = Instantiate(PrefabLoto, ContentPage);
                spawnedLotoFalse.transform.GetChild(0).GetComponent<Image>().sprite = UsedSprites[RandomQuestionFalse];
                Game_Loto_Button loto_Button = spawnedLotoFalse.GetComponent<Game_Loto_Button>();
                loto_Button.isTrue = false;
                UsedSprites.Remove(UsedSprites[RandomQuestionFalse]);
            }
        }
    }
    private void SpawnTrue()
    {
        GameObject spawnedLoto = Instantiate(PrefabLoto, ContentPage);
        spawnedLoto.transform.GetChild(0).GetComponent<Image>().sprite = Loto[RandomQuestion].TrueIcon;
        Game_Loto_Button loto_Button = spawnedLoto.GetComponent<Game_Loto_Button>();
        loto_Button.isTrue = true;
        UsedSprites.Remove(Loto[RandomQuestion].TrueIcon);
        AnnounceText.text = Loto[RandomQuestion].Question;
    }

    public void Next()
    {
        Loto.Remove(Loto[RandomQuestion]);
        if (Loto.Count <= 0)
        {
            for (int b = 0; b < ContentPage.childCount; b++)
            {
                Destroy(ContentPage.GetChild(b).gameObject);
            }
            ReturnButtons.SetActive(true);
            Particles.SetActive(true);
            WinText.text = "Вы победитель!";
            AnnounceText.text = string.Empty;

            StatsText.text = "Отвечено на вопросов: " + QuestionsCount.ToString() + "\n" + "Допущено ошибок: " + Mistakes.ToString();
        }
        else
        {
            PlayGame();
        }
    }

    public void ButtonInteractable()
    {
        for (int i = 0; i < ContentPage.childCount; i++)
        {
            ContentPage.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}

[System.Serializable]
public class Questions
{
    public string Name;
    public Sprite TrueIcon;
    [TextArea]
    public string Question;
}