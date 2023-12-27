using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endText;
    public Sprite[] sprites;
    public Text timeText;
    public Text highScoreText;
    public AudioSource audioSource;
    public AudioClip match;

    private float _time = 0.0f;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        int selectLevel = PlayerPrefs.GetInt("selectLevel"); // 시작화면에서 선택한 레벨 불러오기
        string scoreKeyName = selectLevel + "LevelScore"; // PlayerPrefs 에서 사용할 KeyName을 만든다
        if (PlayerPrefs.HasKey(scoreKeyName) == true) // 현재레벨에 최고기록이 있는지 확인
        {
            float score = PlayerPrefs.GetFloat(scoreKeyName);
            highScoreText.text = "최고: " + score.ToString("N2"); // 최고기록 화면상단에 표시하기
        }
        else
        {
            highScoreText.text = ""; // 빈 텍스트로 만들기
        }


        // 원본 이미지 카드 배열
        int[] sourcesImages = { 0, 1, 2, 3, 4, 5, 6, 7 ,6 ,5 };
        // 게임에 사용할 비어있는 카드 배열
        int[] images = new int[4 + (selectLevel * 4)];
        // 난이도에 따른 카드 개수 조절(최소 1, 최대 4)
        for (var i = 0; i < 2 + (selectLevel * 2); i++)
        {
            // 카드가 짝이 맞도록 2개씩 넣기
            images[i * 2] = sourcesImages[i];
            images[i * 2 + 1] = sourcesImages[i];
        }
        // 레벨별 배치도(상하 반전 주의, 숫자 개수 주의(숫자1 8개, 숫자234 4개))
        int[] levelArr = {
            4, 3, 3, 4,
            2, 1, 1, 2,
            1, 1, 1, 1,
            2, 1, 1, 2,
            4, 3, 3, 4 
        };

        // 섞기
        images = images.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        
        var cards = GameObject.Find("Cards").transform;
        // 변수 i는 화면에 배치할 카드위치계산에 필요
        // 변수 j는 화면에 배치할 카드순서를 세는데 필요
        var j = 0; 
        for (var i = 0; i < 20; i++)
        {
            if (levelArr[i] > selectLevel) { continue; } // 레벨을 확인하고 배치한다
            var newCard = Instantiate(card, cards, true);

            var x = (i % 4) * 1.4f - 2.1f;
            var y = (i / 4) * 1.4f - 4.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            // var rtanName = "rtan" + images[i].ToString();
            var spriteName = sprites[images[j++]].name; // 카드가 배치되면 카드순서를 1증가시킨다
            // newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
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
            Time.timeScale = 0.0f;
        }
    }
    
    public void IsMatched()
    {
        var firstCardImage = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        var secondCardImage = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstCardImage == secondCardImage)
        {
            audioSource.PlayOneShot(match); // 오디오 재생

            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();
            
            var cardsLeft = GameObject.Find("Cards").transform.childCount;
            if (cardsLeft == 2)
            {
                endText.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }
}
