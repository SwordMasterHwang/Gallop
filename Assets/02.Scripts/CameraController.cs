using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;
    public float translateSpeed;
    public float rotationSpeed;
    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }
  
    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);//타겟에 offset으로 지정한 Vector3
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);//두 개의 Vector3 값을 정하면 비율만큼의 적당한 Vector3값을 리턴
    }
  
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);//타깃을 기준으로 회전
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);//두 개의 회전 값을 정하면 비율만큼의 적당한 회전값을 리턴
    }
}
