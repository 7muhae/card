using UnityEngine;

public class CardShow : MonoBehaviour
{
    public Animator animator;

    public void DoAnimation(int num)
    {
        if (num == 2)
        {
            transform.localPosition = new Vector2(-4, -4);
        }
        animator.Play("show" + num);
    }
}
