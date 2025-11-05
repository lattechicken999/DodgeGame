using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour,IGameEnd
{
    [SerializeField] float _speed;
    //[SerializeField] float _range;

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            transform.Translate(Vector3.forward* _speed*Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.AddSubscriber(this);
    }
    public void NotifyGameEnd()
    {
        gameObject.SetActive(false);
    }
}
