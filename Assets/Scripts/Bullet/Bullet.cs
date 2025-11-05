using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IGameEnd
{
    [SerializeField] float _speed;
    //[SerializeField] float _range;

    private void Update()
    {
        //활성화 중에는 앞으로 전진
        if(gameObject.activeSelf)
        {
            transform.Translate(Vector3.forward* _speed*Time.deltaTime);
        }
    }
    
    //충돌시 비활성화
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    //게임 종료되면 모든 탄환은 비활성화
    private void Start()
    {
        GameManager.Instance.AddSubscriber(this);
    }
    public void NotifyGameEnd()
    {
        gameObject.SetActive(false);
    }
}
