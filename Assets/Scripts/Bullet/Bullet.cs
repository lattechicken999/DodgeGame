using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
}
