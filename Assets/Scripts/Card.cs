using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator anim;
    public AudioClip flip;
    public AudioSource audioSource;

    public void OpenCard()
    {
        audioSource.PlayOneShot(flip); // 오디오 한번 실행

        anim.SetBool("isOpen", true);
        transform.Find("Front").gameObject.SetActive(true);
        transform.Find("Back").gameObject.SetActive(false);
        
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
        anim.SetBool("isOpen", false);
        transform.Find("Back").GetComponent<SpriteRenderer>().color = Color.gray; // 뒷면 색상을 그레이로 변경
        transform.Find("Back").gameObject.SetActive(true);
        transform.Find("Front").gameObject.SetActive(false);
    }
}
