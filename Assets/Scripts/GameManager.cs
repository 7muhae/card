using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endText;
    public GameObject countGameObject;
    public AudioSource audioData;
    public AudioSource bgmSource;
    public Sprite[] sprites;
    public Text timeText;
    public Text countText;
    public Text nameText;
    public int count;

    private float _time = 0.0f;

    private void Awake()
    {
        Instance = this;
        bgmSource = null;
    }
    
    private void Start()
    {
        count = 0;
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("start");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        
        int[] images = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        images = images.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        var cards = GameObject.Find("Cards").transform;
        for (var i = 0; i < 16; i++)
        {
            var newCard = Instantiate(card, cards, true);
            
            var x = (i / 4) * 1.4f - 2.1f;
            var y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            var spriteName = sprites[images[i]].name;
            newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spriteName);
        }

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        timeText.text = _time.ToString("N2");
        
        if (_time > 30.0f)
        {
            endText.SetActive(true);
            countGameObject.SetActive(true);
            countText.text = "count : " + count.ToString();
            Time.timeScale = 0.0f;
        }
        else if (_time > 20.0f)
        {
            if (!bgmSource)
            {
                bgmSource = Instantiate(audioData);
                bgmSource.clip = Resources.Load<AudioClip>("bgm");
                bgmSource.Play(0);
            }

            timeText.color = Color.red;
        }
    }
    
    public void IsMatched()
    {
        count++;
        
        var firstCardImage = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        var secondCardImage = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();

            nameText.text = firstCardImage.Substring(0, firstCardImage.Length - 1);
            
            var cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endText.SetActive(true);
                countGameObject.SetActive(true);
                countText.text = "count : " + count.ToString();
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            nameText.text = "실패";
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }
        
        firstCard = null;
        secondCard = null;
    }
}
