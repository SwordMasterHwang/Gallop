using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void OnClickRestart()
    {
        CarController.isExplosion = false; //차량 터짐 여부 초기화
        CarController.isEnding = false; // 엔딩 여부 초기화
        SceneManager.LoadScene("Main");//메인씬 로드
    }
    public void OnClickExit()
    {
        CarController.isExplosion = false; //차량 터짐 여부 초기화
        CarController.isEnding = false; // 엔딩 여부 초기화
        Application.Quit();//종룐
    }
}
