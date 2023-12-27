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
    private bool _isOpen = false;

    public void OpenCard()
    {
        if (_isOpen)
            return;
        _isOpen = true;
        
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("click");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        anim.SetBool("isOpen", true);
        
        transform.Find("Back").GetComponent<SpriteRenderer>().color = Color.grey;
        
        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = gameObject;
            GameManager.Instance.CardFlipCoroutine();
        }
        else
        {
            GameManager.Instance.secondCard = gameObject;
            GameManager.Instance.IsMatched();
        }
    }
    
    public void DestroyCard()
    {
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("correct");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        Invoke(nameof(DestroyCardInvoke), 1f);
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
    
    public void CloseCardInvoke()
    {
        _isOpen = false;
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("fail");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        anim.SetBool("isOpen", false);
    }
}
