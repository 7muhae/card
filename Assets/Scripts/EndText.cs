using UnityEngine;
using UnityEngine.SceneManagement;

public class EndText : MonoBehaviour
{
    public void RetryGame()
    {
        //SceneManager.LoadScene("MainScene");
        // 클리어레벨이 없거나, 클리어레벨이 지금 클리어한 레벨보다 낮은경우
        if (PlayerPrefs.HasKey("clearLevel") == false || PlayerPrefs.GetInt("clearLevel") < PlayerPrefs.GetInt("selectLevel"))
        {
            PlayerPrefs.SetInt("clearLevel", PlayerPrefs.GetInt("selectLevel")); // 클리어레벨 저장
        }
        SceneManager.LoadScene("StartScene"); // 메인메뉴로 돌아가기
    }
}
