using UnityEngine;
using UnityEngine.SceneManagement;

public class EndText : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
