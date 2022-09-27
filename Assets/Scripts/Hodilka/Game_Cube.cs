using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Cube : MonoBehaviour, ISceneLoader
{
    [SerializeField] private List<Player> Players = new List<Player>();
    [SerializeField] private Transform[] PlayerIcons;
    [SerializeField] private List<Transform> Steps = new List<Transform>();
    [SerializeField] private List<MapEffect> Effects = new List<MapEffect>();
    [SerializeField] private List<Buildings> Buildings = new List<Buildings>();
    private List<TextAnnouncer> AnnouncerQueue = new List<TextAnnouncer>();
    [SerializeField] private GameObject Player;
    [SerializeField] private TextMeshProUGUI MainText;
    [SerializeField] private GameObject Cubek;
    [SerializeField] private Animator AnimCubek;
    [SerializeField] private GameObject WinMenu;
    [SerializeField] private Button dropButton;
    public GameObject InfoPanel;
    private float time;
    public Player currentPlayer = null;
    private int targetPos;
    private int EndPos;
    private bool ButtonDrop = true;
    private bool DropCubekPhone;

    private void Start()
    {
        StepInit();
    }

    private void FirstAnnounce()
    {
        foreach (Player player in Players)
        {
            player.Viewmodel.position = Steps[0].position;
        }

        AddText("Мы начинаем!", 2f);
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        FindMovePlayer();

        foreach (Player player in Players)
        {
            if (player.isMoved == false & player.SkipMove == false)
            {
                EndPos = 0;
                targetPos = 0;
                yield return new WaitForSeconds(2f);//10
                AddText("Ход игрока " + player.Nickname, 2f);
                dropButton.interactable = true;
                yield return new WaitForSeconds(2f);//10
                AddText("Кубик кидает " + player.Nickname, 2f);
                Cubek.SetActive(true);

                if (ButtonDrop == true)
                {
                    Cubek.transform.GetChild(1).gameObject.SetActive(false);
                    AddText("Потряси телефоном чтобы бросить кубик", 5f);
                    DropCubekPhone = true;
                }
                else
                {
                    Cubek.transform.GetChild(1).gameObject.SetActive(true);
                }

                currentPlayer = player;
                player.isMoved = true;
                break;
            }
            if (player.isMoved == false & player.SkipMove == true)
            {
                AddText("Игрок " + player.Nickname + " проскает ход", 2f);
                player.SkipMove = false;
            }
        }
        yield return null;
    }

    public void DropCubek()
    {
        AnimCubek.enabled = true;
        AddText("Бросаем кубик", 2f);
        StartCoroutine(RandomCubekDrop());
    }

    private IEnumerator RandomCubekDrop()
    {
        dropButton.interactable = false;
        yield return new WaitForSeconds(5f);
        AnimCubek.enabled = false;
        int rnd = Random.Range(1, 6);
        AddText("Выпало число " + rnd.ToString(), 3f);
        yield return new WaitForSeconds(3f);
        Cubek.SetActive(false);
        AddText("Игрок " + currentPlayer.Nickname + " ходит на " + rnd.ToString() + " шагов", 3f);
        yield return new WaitForSeconds(3f);
        MovePlayer(currentPlayer, rnd);
    }

    private void FindMovePlayer()
    {
        int moves = 0;
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i].isMoved == false)
            {
                moves++;
            }
        }
        if (moves == 0)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].isMoved = false;

            }
        }
    }
    private void MovePlayer(Player player, int moves)
    {
        targetPos += moves;
        player.AllMoved += moves;
        if (player.AllMoved >= Steps.Count)
        {
            int ostatok = Steps.Count;
            ostatok -= player.move;
            moves = ostatok - 1;
        }
        StartCoroutine(SmoothMove(moves));
    }

    private bool CheckMoves(Player player)
    {
        if (player.move >= Steps.Count - 1)
        {
            AnnouncerQueue.Clear();
            AddText("Игра завершена!", 150f);
            WinMenu.SetActive(true);
            WinMenu.transform.GetChild(0).GetComponent<TMP_Text>().text = "Игрок " + player.Nickname + " победитель!";
        }

        foreach (Buildings item in Buildings)
        {
            if (item.Id == player.AllMoved)
            {
                VisibleInfo(item, "Игрок " + player.Nickname + " дошел до ");
                return true;
            }
        }

        foreach (MapEffect item in Effects)
        {
            if (item.MoveStart == player.AllMoved)
            {
                switch (item.action)
                {
                    case Action.SkipMove:
                        AddText("Игрок " + player.Nickname + " пропускает след.ход", 5f);
                        currentPlayer.SkipMove = true;
                        break;
                    case Action.Teleport:
                        AddText("Игрок " + player.Nickname + " перемещен на " + (player.AllMoved = +item.MoveEnd) + " шагов вперед", 4f);
                        player.move = item.MoveEnd;
                        break;
                }
            }
        }

        return false;
    }

    private void VisibleInfo(Buildings building, string info)
    {
        InfoPanel.SetActive(true);
        InfoPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = building.Name;
        InfoPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = building.Info;
        InfoPanel.transform.GetChild(3).GetComponent<Image>().sprite = building.Icon;
        InfoPanel.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = info;
    }

    private void AddText(string words, float time)
    {
        TextAnnouncer textAnnouncer = new()
        {
            Text = words,
            Time = time
        };
        AnnouncerQueue.Add(textAnnouncer);
    }

    private IEnumerator SmoothMove(int moves)
    {
        for (int i = 0; i < moves; i++)
        {
            yield return new WaitForSecondsRealtime(1f);
            currentPlayer.move++;
        }
        yield return new WaitForSecondsRealtime(1f);

        if (CheckMoves(currentPlayer) == false)
        {
            StartCoroutine(StartGame());
        }
        yield return null;
    }

    private void Update()
    {
        if (currentPlayer != null)
        {
            currentPlayer.Viewmodel.position = Vector2.Lerp(currentPlayer.Viewmodel.position, Steps[currentPlayer.move].position, Time.deltaTime);
        }

        AnnounceTextDisplay();

        if (DropCubekPhone)
        {
            if (Input.acceleration.x > 1)
            {
                AnnouncerQueue.Clear();
                AddText("Вы потрясли телефон", 2f);
                DropCubekPhone = false;
                DropCubek();
            }
        }
    }

    private void AnnounceTextDisplay()
    {
        if (AnnouncerQueue.Count == 0 & time <= 0)
        {
            MainText.text = string.Empty;
        }

        time -= Time.deltaTime;

        for (int i = 0; i < AnnouncerQueue.Count; i++)
        {
            if (time <= 0)
            {
                if (AnnouncerQueue.Count >= 0)
                {
                    TextAnnouncer text = AnnouncerQueue[i];
                    MainText.text = text.Text;
                    time = text.Time;
                    AnnouncerQueue.Remove(text);
                }
            }
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SetPlayer(string[] playersName, bool acceleration)
    {
        ButtonDrop = acceleration;
        for (int i = 0; i < playersName.Length; i++)
        {
            string name = playersName[i];

            if (name != null)
            {
                Player player = new Player();
                player.Id = i;
                player.Nickname = name;
                player.Viewmodel = PlayerIcons[i];
                Players.Add(player);
            }
        }
        FirstAnnounce();
    }
    private void StepInit()
    {
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].GetChild(0).GetComponent<TMP_Text>().text = i.ToString();
        }
    }
}

[System.Serializable]
public class Buildings
{
    public string Name;
    [TextArea]
    public string Info;
    public Sprite Icon;
    public Action Effect;
    public int Id;
}

public enum Action
{
    None,
    Teleport,
    SkipMove
}

[System.Serializable]
public class TextAnnouncer
{
    public string Text;
    public float Time;
}

[System.Serializable]
public class Player
{
    public string Nickname;
    public int Id;
    public int move = 0;
    public Transform Viewmodel;
    public bool isMoved;
    public int AllMoved;
    public bool SkipMove;
}

[System.Serializable]
public class MapEffect
{
    public string Name;
    public int MoveStart;
    public int MoveEnd;
    public Action action;
}