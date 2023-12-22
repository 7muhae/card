using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    public int levelNumber;
    public bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        int clearLevel = 0;
        levelNumber = int.Parse(gameObject.transform.Find("Text").GetComponent<Text>().text); // 텍스트에 적힌 숫자 확인
        if (PlayerPrefs.HasKey("clearLevel") == true) // 해당 데이터가 저장되어 있는지 확인
        {
            clearLevel = PlayerPrefs.GetInt("clearLevel"); // 클리어 레벨을 변수에 대입
        }
        // 현재 레벨을 오픈할지 확인
        if (levelNumber <= clearLevel + 1) { isOpen = true; }
        else { isOpen = false; }
        // 오픈된경우 화면에 표시
        transform.Find("openLevel").gameObject.SetActive(isOpen); // 오픈일시 true
        transform.Find("closeLevel").gameObject.SetActive(!isOpen); // 오픈일시 not(!)연산자로 false
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LevelSelect()
    {
        Debug.Log(levelNumber.ToString() + " " + isOpen.ToString());
        if (!isOpen) { return; } // 열리지않았다면 반응하지않는다
        PlayerPrefs.SetInt("selectLevel", levelNumber); // 선택한 레벨 저장
        SceneManager.LoadScene("MainScene"); // 메인씬으로 이동
    }
}
