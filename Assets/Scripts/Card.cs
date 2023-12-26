using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioData;

    private GameObject _cardFront;
    private GameObject _cardBack;
    private Coroutine _closeCardCoroutine;

    private IEnumerator CloseCardAfterDelay(float delay)                                        //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
    {
        yield return new WaitForSeconds(delay);
        CloseCard();
    }

    public void OpenCard()
    {
        var audioSource = Instantiate(audioData);
        audioSource.clip = Resources.Load<AudioClip>("click");
        audioSource.Play(0);
        audioSource.GetComponent<AudioData>().DestroySelf();
        anim.SetBool("isOpen", true);
        transform.Find("Front").gameObject.SetActive(true);
        transform.Find("Back").gameObject.SetActive(false);
        transform.Find("Back").GetComponent<SpriteRenderer>().color = Color.grey;
        
        if (GameManager.Instance.firstCard == null)                                
        {
            GameManager.Instance.firstCard = gameObject;
            _closeCardCoroutine = StartCoroutine(CloseCardAfterDelay(3.0f));         //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
        }
        else
        {
            GameManager.Instance.secondCard = gameObject;
            GameManager.Instance.IsMatched();
            if (_closeCardCoroutine != null)                                           //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
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
        Invoke(nameof(DestroyCardInvoke), 0.5f);
        _closeCardCoroutine = null;                                                //첫번쨰 카드 클릭하고 다음 카드를 안뒤집으면 3초뒤에 다시 뒤집히는 코드
    }
    
    public void CloseCard()
    {
        Invoke(nameof(CloseCardInvoke), 0.5f);
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
        transform.Find("Back").gameObject.SetActive(true);
        transform.Find("Front").gameObject.SetActive(false);
    }
}
