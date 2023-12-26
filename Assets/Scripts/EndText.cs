using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndText : MonoBehaviour
{
    public Text timeText; // 현재점수를 알기위한 변수 선언
    
    // EndText 클릭 이벤트
    public void RetryGame()
    {        
        float score = Convert.ToSingle(timeText.text); // 현재 점수
        int selectLevel = PlayerPrefs.GetInt("selectLevel"); // 현재 레벨
        string scoreKeyName = selectLevel + "LevelScore"; // "1LevelScore", "2LevelScore", "3LevelScore"... 등 PlayerPrefs.Get...() 등 에 사용될 문자열

        // 목적: 게임을 클리어했다면 최고클리어레벨과 최고기록 갱신하기
        // 조건: 제한시간안에 클리어했는가(30초 이상일시 클리어 실패)
        // 코드: 목적과 동일
        if (score < 30f)
        {
            // 목적: 최고 클리어레벨 갱신하기
            // 조건1: 클리어한 최고 레벨이 없거나
            // 조건2: 지금 클리어한 레벨이 기존에 클리어했던 레벨보다 높다면
            // 코드: 현재레벨을 클리어한 최고 레벨로 기록
            if (PlayerPrefs.HasKey("clearLevel") == false || PlayerPrefs.GetInt("clearLevel") < PlayerPrefs.GetInt("selectLevel"))
            {
                PlayerPrefs.SetInt("clearLevel", selectLevel); // 클리어레벨로 저장
            }

            // 목적: 최고기록 갱신하기
            // 조건1: 최고기록이 없는가?
            // 조건2: 현재기록이 최고기록보다 높은가?
            // 코드: 현재기록을 최고기록으로 갱신하기
            if (PlayerPrefs.HasKey(scoreKeyName) == false || score > PlayerPrefs.GetFloat(scoreKeyName))
            {
                PlayerPrefs.SetFloat(scoreKeyName, score); // 최고기록으로 저장
            }
        }

        SceneManager.LoadScene("StartScene"); // 시작메뉴로 돌아가기
    }
}
