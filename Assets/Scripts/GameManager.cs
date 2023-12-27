using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endText;
    public Text timeText;
    public Text failScore;
    public GameObject endPanel;
    public Text nameText;

    int fail;

    private float _time = 0.0f;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        int[] images =  { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        //images = images.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        //����
        int random1, random2;
        var temp = 0;

        for (int i = 0; i < images.Length; ++i)
        {
            random1 = Random.Range(0, images.Length);
            random2 = Random.Range(0, images.Length);

            temp = images[random1];
            images[random1] = images[random2];
            images[random2] = temp;
        }
        for (int k = 0; k < images.Length; k++)
        {
            Debug.Log(images[k]);
        }




        var cards = GameObject.Find("Cards").transform;
        for (var i = 0; i < 16; i++)
        {
            var newCard = Instantiate(card, cards, true);
            
            var x = (i / 4) * 1.4f - 2.1f;
            var y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            var rtanName = "rtan" + images[i].ToString();
            newCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        timeText.text = _time.ToString("N2");

        //Ÿ�̸� �ð��� �˹� �� �� ���̸ӿ��� ����ϴ� ��� �ۼ��غ���(�ð��� �Ӱ� ���ϰų� ����� ����������� ����)
        if (_time > 20.0f)
            timeText.color = Color.red;

        if (_time > 30.0f)
        {
            failScore.text = "������ Ƚ�� : " + fail;//string�� int ������ �׳� string���� ���ٲ�
            endPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    
    public void IsMatched()
    {
        fail += 1;//���������Ŵ� fail++
        //Debug.Log(fail);
        //Debug.Log("������ Ƚ��" + fail);
        var firstCardImage = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        var secondCardImage = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        
        if (firstCardImage == secondCardImage)
        {
            nameText.text = "" + firstCardImage;

            firstCard.GetComponent<Card>().DestroyCard();
            secondCard.GetComponent<Card>().DestroyCard();
            
            var cardsLeft = GameObject.Find("Cards").transform.childCount;

            if (cardsLeft == 2)
            {
                failScore.text = "������ Ƚ�� : " + fail;//
                endPanel.SetActive(true);
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            _time += 2.0f;
            teamName("����! 2�� �߰�!");
            firstCard.GetComponent<Card>().CloseCard();
            secondCard.GetComponent<Card>().CloseCard();
        }

        firstCard = null;
        secondCard = null;

        
    }

    void teamName(string name)
    {
        nameText.text = "" + name;
    }
}
