using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontal;//좌우
    private float vertical;//상하
    private float nowsteerAngle;//현재 바퀴 조향각도
    private float nowbreakPower;//현재 브레이크 힘
    private bool isBreak;//브레이크 여부

    public float enginePower;//바퀴를 회전시키는 힘
    public float breakPower;//브레이크 힘
    public float maxsteerAngle;//바퀴의 최대 조향각도
    
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private WheelFrictionCurve rearLeftWheelforward;
    private WheelFrictionCurve rearLeftWheelsideways;
    private WheelFrictionCurve rearRightWheelforward;
    private WheelFrictionCurve rearRightWheelsideways;
    
    public UIController UIController;
    public ParticleManager ParticleManager;
    public CarSound CarSound;
    public GameObject Car;
    public ItemSound ItemSound;

    public static bool isExplosion = false; 
    public static bool isEnding = false; 
    private void Start()//각 콜라이더의 마찰 연결
    {
        rearLeftWheelforward = rearLeftWheelCollider.forwardFriction;
        rearLeftWheelsideways = rearLeftWheelCollider.sidewaysFriction;
        rearRightWheelforward = rearRightWheelCollider.forwardFriction;
        rearRightWheelsideways = rearRightWheelCollider.sidewaysFriction;
    }

    private void FixedUpdate()//물리엔진이 적용된 Rigidbody를 사용하므로 update가 아닌 fixed 사용
    {
        GetInput();//플레이어 키보드 입력
        Handleing();//휠
        Steering();//휠 회전
        Wheels();//휠 업데이트
    }
    private void GetInput()
    {
        horizontal = Input.GetAxis("Horizontal");//좌우 이동
        vertical = Input.GetAxis("Vertical");//상하 이동

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || 
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))//주행 사운드
        {
            CarSound.OnVertical();
        }

        if (vertical!=0)//상하 이동을 시작하면
        {//브레이크 초기화
            frontLeftWheelCollider.brakeTorque = 0f;
            frontRightWheelCollider.brakeTorque = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//드리프트 사운드
        {
            CarSound.OnDrift();
        }
        
        if (Input.GetKey(KeyCode.LeftShift))//왼쪽 쉬프트 입력시 드리프트
        {//드리프트
            
            //뒤쪽 왼쪽 바퀴 마찰계수 조정
            rearLeftWheelforward.stiffness = 0.6f;
            rearLeftWheelCollider.forwardFriction = rearLeftWheelforward;
            rearLeftWheelsideways.stiffness = 0.6f;
            rearLeftWheelCollider.sidewaysFriction = rearLeftWheelsideways;
            
            //뒤쪽 오른쪽 바퀴 마찰계수 조정
            rearRightWheelforward.stiffness = 0.6f;
            rearRightWheelCollider.forwardFriction = rearRightWheelforward;
            rearRightWheelsideways.stiffness = 0.6f;
            rearRightWheelCollider.sidewaysFriction = rearRightWheelsideways;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))//왼쪽 쉬프트 입력 취소시 드리프트 중단
        {//드리프트 중단
            
            //뒤쪽 왼쪽 바퀴 마찰계수 조정
            rearLeftWheelforward.stiffness = 1f;
            rearLeftWheelCollider.forwardFriction = rearLeftWheelforward;
            rearLeftWheelsideways.stiffness = 1f;
            rearLeftWheelCollider.sidewaysFriction = rearLeftWheelsideways;
            
            //뒤쪽 오른쪽 바퀴 마찰계수 조정
            rearRightWheelforward.stiffness = 1f;
            rearRightWheelCollider.forwardFriction = rearRightWheelforward;
            rearRightWheelsideways.stiffness = 1f;
            rearRightWheelCollider.sidewaysFriction = rearRightWheelsideways;
        }
        
        isBreak = Input.GetKey(KeyCode.Space);//스페이스 입력시 자동차 브레이크
        
        if (Input.GetKeyDown(KeyCode.Space))//브레이크 사운드
            CarSound.OnBreak();
    }
    
    private void Handleing()
    {//바퀴 회전 속도 (4륜 구동)
        frontLeftWheelCollider.motorTorque = vertical * enginePower;
        frontRightWheelCollider.motorTorque = vertical * enginePower;
        rearLeftWheelCollider.motorTorque = vertical * enginePower;
        rearRightWheelCollider.motorTorque = vertical * enginePower;
        
        nowbreakPower = isBreak ? breakPower : 0f;//브레이크가 참이라면
        if (isBreak)
        {
            Break();
        }
    }

    private void Break()
    {//모든 바퀴 브레이크
        frontLeftWheelCollider.brakeTorque = nowbreakPower;
        frontRightWheelCollider.brakeTorque = nowbreakPower;
        rearLeftWheelCollider.brakeTorque = nowbreakPower;
        rearRightWheelCollider.brakeTorque = nowbreakPower;

        isBreak = false;
    }

    private void Steering()
    {//바퀴 조향각도 조절
        nowsteerAngle = maxsteerAngle * horizontal;
        frontLeftWheelCollider.steerAngle = nowsteerAngle;
        frontRightWheelCollider.steerAngle = nowsteerAngle;
    }

    private void Wheels()
    {//바퀴 회전
        SingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        SingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        SingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        SingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void SingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;//바퀴 위치
        Quaternion rotation;//바퀴 상대회전
        wheelCollider.GetWorldPose(out position, out rotation);//휠의 전체 공간 pose를 가져옴
        wheelTransform.rotation = rotation;
        wheelTransform.position = position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CarSound.OnCrush();//충돌 사운드 재생
        
        if(collision.gameObject.tag == "Structure")//빌딩 데미지
            UIController.OnDamage();
        else if(collision.gameObject.tag == "Car")//자동차 데미지
            UIController.OnDamageCar();
        else if(collision.gameObject.tag == "Structure2")//기타 건축물 데미지
            UIController.OnDamage2();
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemSound.OnPlaySound();//아이템 및 도착 지점 사운드 재생
        
        if (other.gameObject.tag == "Timeplus")//시간 증가
        {
            UIController.OnTimePlus();
        }
        else if (other.gameObject.tag == "Durabilityplus")//내구도 회복
        {
            UIController.OnDurabilityPlus();
        }
        else
        {
            isEnding = true;
            Invoke("Stop",2f);//게임 정지
        }
            
        
        Destroy(other.gameObject);
    }

    public void Explosion()//데미지 누적시 차량 폭파
    {
        ParticleManager.Explosion();
        CarSound.OnExplosion();
        Car.SetActive(false);
        isExplosion = true;
        Invoke("Stop",2f);//게임 정지
    }

    private void Stop()
    {
        Time.timeScale = 0;//게임 정지
    }
}
