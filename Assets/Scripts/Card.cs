using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioData;
    public CardShow cardShow;
    public GameObject cardFront;

    private int _num = 0;
    private GameObject _cardBack;
    private Coroutine _closeCardCoroutine;

    public void OpenCard()
    {
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("click");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        anim.SetBool("isOpen", true);
        
        transform.Find("Back").GetComponent<SpriteRenderer>().color = Color.grey;
        
        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = gameObject;
            _closeCardCoroutine = StartCoroutine(CloseCardAfterDelay(3.0f)); 
        }
        else
        {
            GameManager.Instance.secondCard = gameObject;
            GameManager.Instance.IsMatched();
            if (_closeCardCoroutine != null)    //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
            {
                StopCoroutine(_closeCardCoroutine);    
                _closeCardCoroutine = null;
            }
        }
    }
    
    public void DestroyCard()
    {
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("correct");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        Invoke(nameof(DestroyCardInvoke), 1f);
        _closeCardCoroutine = null;      
    }
    
    public void CloseCard()
    {
        Invoke(nameof(CloseCardInvoke), 0.75f);
    }

    public void Show()
    {
        if (_num == 0)
            _num = Random.Range(1, 5);
        
        cardFront.SetActive(true);
        cardShow.DoAnimation(_num);
    }

    private void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }
    
    private void CloseCardInvoke()
    {
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("fail");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        anim.SetBool("isOpen", false);
    }
    
    private IEnumerator CloseCardAfterDelay(float delay)    //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
    {
        yield return new WaitForSeconds(delay);
        CloseCard();
    }
}
