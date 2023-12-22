using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Animator anim;
    public AudioSource audioData;

    private GameObject _cardFront;
    private GameObject _cardBack;

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
        Invoke(nameof(DestroyCardInvoke), 0.5f);
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
