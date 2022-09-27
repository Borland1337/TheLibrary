using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameMemory : MonoBehaviour, ISceneLoader
{
    public int maxCards;
    
    public GameObject content;

    public ImageScript card1;
    public ImageScript card2;

    public string card1name;
    public string card2name;

    public Sprite[] AllSprites;

    public GridLayoutGroup grid;

    public ImageScript cardPrefab;

    System.Random rng = new System.Random();

    public void Equals(ImageScript card)
    {

        if (card1 == null)
        {
            card1 = card;
            card1name = card1.front.name;
        }

        else
        {
            card2 = card;

            card2name = card2.front.name;
            if (card1.gameObject.name != card2.gameObject.name)
            {
                if (card1.front.name == card2.front.name)
                {
                    StartCoroutine(Delay(card1, card2, 1, true));
                }
                else
                {
                    StartCoroutine(Delay(card1, card2, 1, false));
                    card1 = null;
                    card2 = null;
                }
            }
        }
    }

    private void ChangeCard(int max,int min)
    {
        
        for (int i = AllSprites.Length - 1; i >= 1; i--)
        {
            int j = rng.Next(i + 1);
            var temp = AllSprites[j];
            AllSprites[j] = AllSprites[i];
            AllSprites[i] = temp;
        }

        for (int i = 0; i < maxCards; i++)
        {
            ImageScript card = Instantiate(cardPrefab, grid.transform);
            card.front = AllSprites[i];

            card.name = Random.Range(min, max).ToString();
        }
    }

    private void Start()
    {
        ChangeCard(0,99);

        ChangeCard(100, 200);
    }

    private IEnumerator Delay(ImageScript card1, ImageScript card2, float time, bool isDestroy)
    {
        grid.enabled = false;
        yield return new WaitForSeconds(time);

        card1.ShowBack();
        card2.ShowBack();

        if (isDestroy)
        {
            Destroy(card1.gameObject);
            Destroy(card2.gameObject);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}