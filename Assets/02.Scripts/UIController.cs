using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text durability;
    public Text time;

    public CarController car;
    public static float pointdurability;
    public static float pointtime;
    
    public GameObject menu;
    public GameObject sucess;
    public GameObject fail;
    private void Start()
    {
        //차량내구도, 차량스피드, 남은시간 초기화, 게임 멈춤 해제
        pointdurability = 100f;
        pointtime = 30f;
        Time.timeScale = 1;
        
        durability.text = pointdurability.ToString() +"%";
        time.text = pointtime.ToString();
        
        menu.SetActive(false);
        sucess.SetActive(false);
        fail.SetActive(false);
    }

    void Update()
    {
        durability.text = pointdurability.ToString() +"%";
        
        //1초씩 시간 감소
        if (pointtime < 0)
            pointtime = 0f;
        else
            pointtime -= Time.deltaTime;

        time.text = pointtime.ToString("N0");//소수점 보이지 않게
        
        if (Input.GetButtonDown("Cancel"))//메뉴 ESC키
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);//가리기
            }
            else
            {
                menu.SetActive(true);//보이기
            }
        }

        if (CarController.isExplosion)
        {
            menu.SetActive(true);//보이기
            fail.SetActive(true);//보이기
        }
        
        if (CarController.isEnding)
        {
            menu.SetActive(true);//보이기
            sucess.SetActive(true);//보이기
        }
        
        if (pointtime == 0)
        {
            menu.SetActive(true);//보이기
            fail.SetActive(true);//보이기
            Time.timeScale = 0;
        }
        
    }

    public void OnDamageCar()
    {
        //차량 내구도가 0초보다 높다면
        if (pointdurability > 0)
        {
            pointdurability -= 15f;
            
            //차량 내구도가 0이거나 음수라면
            if (pointdurability <= 0)
            {
                pointdurability = 0f;
                car.Explosion();
            }
        }
    }
    
    public void OnDamage()
    {
        //차량 내구도가 0초보다 높다면
        if (pointdurability > 0)
        {
            pointdurability -= 20f;
            
            //차량 내구도가 0이거나 음수라면
            if (pointdurability <= 0)
            {
                pointdurability = 0f;
                car.Explosion();
            }
        }
    }
    
    public void OnDamage2()
    {
        //차량 내구도가 0초보다 높다면
        if (pointdurability > 0)
        {
            pointdurability -= 5f;
            
            //차량 내구도가 0이거나 음수라면
            if (pointdurability <= 0)
            {
                pointdurability = 0f;
                car.Explosion();
            }
        }
    }
    
    public void OnTimePlus()
    {
        //시간 회복
        pointtime += 30f;
    }
    
    public void OnDurabilityPlus()
    {
        //내구도 회복
        if (pointdurability < 100)
        {
            pointdurability += 20f;
        }
        else
        {
            pointdurability = 100f;
        }
    }
    
}
