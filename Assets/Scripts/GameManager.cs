using System.Collections;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endPanel;
    public GameObject countGameObject;
    public AudioSource audioData;
    public AudioSource bgmSource;
    public Sprite[] sprites;
    public Text timeText;
    public Text highScoreText;
    public Text countText;
    public Text nameText;
    public Text resultText;
    public int count;
    public int result;

    private float _time = 60.0f;
    private int _cardLeftNum;
    private bool _isEmergency;
    private Coroutine _closeCardCoroutine;

    [SerializeField]
    private Slider timeSlider;      //슬라이더 코드
    private float _timeLimit = 60f;  //슬라이더 코드
    private float _currentTime;      //슬라이더 코드
    
    private void Awake()
    {
        Instance = this;
        bgmSource = null;
        _closeCardCoroutine = null;
    }
    
    private void Start()
    {
        _currentTime = _timeLimit;                  // 슬라이더 코드
        StartCoroutine(CountDownTimerRoutine());    // 슬라이더 코드
        
        count = 0;
        _cardLeftNum = 0;
        _isEmergency = false;
        
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("start");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        bgmSource = Instantiate(audioData);
        bgmSource.clip = Resources.Load<AudioClip>("bgm");
        bgmSource.pitch = 0.95f;
        bgmSource.loop = true;
        bgmSource.Play(0);
        
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
        int[] sourcesImages = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
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

        //images = images.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        for (int i = 0; i < images.Length; ++i)
        {
            var random1 = Random.Range(0, images.Length);
            var random2 = Random.Range(0, images.Length);

            var temp = images[random1];
            images[random1] = images[random2];
            images[random2] = temp;
        }

        var cards = GameObject.Find("Cards").transform;
        var j = 0;
        for (var i = 0; i < 20; i++)
        {
            if (levelArr[i] > selectLevel) { continue; } // 레벨을 확인하고 배치한다
            _cardLeftNum++;
            var newCard = Instantiate(card, cards, true);

            var x = (i % 4) * 1.4f - 2.1f;
            var y = (i / 4) * 1.4f - 4.0f;
            newCard.transform.position = new Vector3(x, y, 0);
            // var rtanName = "rtan" + images[i].ToString();
            var spriteName = sprites[images[j++]].name;
            // newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spriteName);
        }
        
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        timeText.text = _time.ToString("N2");
        countText.text = "count : " + count;
        if (_time <= 0.0f)
        {
            endPanel.SetActive(true);
            countGameObject.SetActive(true);
            Time.timeScale = 0.0f;
            _time = 0.0f;
        }
        else if (_time <= 10.0f)
        {
            var t = Mathf.PingPong(Time.time, 1.0f);
            
            var color = Color.Lerp(Color.red, Color.white, t);
            timeText.color = color;
            
            var scale = Mathf.Lerp(1.5f, 1.0f, t);
            timeText.transform.localScale = new Vector3(scale, scale, scale);

            if (_isEmergency) return;
            _isEmergency = true;
            bgmSource.pitch = 1.05f;
        }
    }
    
    public void IsMatched()
    {
        count++;
        
        var firstCardImage = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        var secondCardImage = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;

        if (_closeCardCoroutine != null)    //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
        {
            StopCoroutine(_closeCardCoroutine);
            _closeCardCoroutine = null;
        }
        
        if (firstCardImage == secondCardImage)
        {
            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();

            nameText.text = firstCardImage.Substring(0, firstCardImage.Length - 1);

            _cardLeftNum -= 2;
            if (_cardLeftNum == 0)
            {
                endPanel.SetActive(true);
                countGameObject.SetActive(true);
                countText.text = "count : " + count;
                result = 50 + (int)(_time * 2f - count);
                resultText.text = "점수 : " + result;
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            nameText.text = "실패!\n 2초 감소!";
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
            _time -= 2.0f;       // 시간 빼기 
            _currentTime -= 2.0f;  //슬라이더
        }
        
        firstCard = null;
        secondCard = null;
    }

    public void CardFlipCoroutine()
    {
        _closeCardCoroutine = StartCoroutine(CloseCardAfterDelay(3.0f));
    }
    
    /// <summary>
    /// 슬라이더 제어 메소드
    /// </summary>
    private IEnumerator CountDownTimerRoutine()   
    {
        while (_currentTime >= 0)
        {
            _currentTime -= Time.deltaTime;
            timeSlider.value = _currentTime / _timeLimit;
            yield return null;
        }
    }
    
    private IEnumerator CloseCardAfterDelay(float delay)    //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
    {
        yield return new WaitForSeconds(delay);
        firstCard.GetComponent<Card>().CloseCardInvoke();
        firstCard = null;
        _closeCardCoroutine = null;
    }
}
