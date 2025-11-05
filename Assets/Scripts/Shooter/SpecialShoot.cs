using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialShoot : MonoBehaviour
{
    //확률 받기
    [SerializeField,Range(0,100)] float _shootProbability;
    //쏠지 말지 체크하는 시간
    [SerializeField] float _coolTime;
    [SerializeField] Shooter[] _shooters;

    private Coroutine _SpecialShootRoutine;
    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_coolTime);
    }

    private void Update()
    {
        if(GameManager.Instance.IsPlaying)
        {
            if(_SpecialShootRoutine == null)
            {
                _SpecialShootRoutine = StartCoroutine(Shoot());
            }

        }
        else
        {
            if (_SpecialShootRoutine != null)
            {
                StopCoroutine(_SpecialShootRoutine);
                _SpecialShootRoutine = null;
            }
               
        }
    }
    private IEnumerator Shoot()
    {
        while(true)
        {
            if(Random.value < _shootProbability/100)
            {
                foreach(var shooter in _shooters)
                {
                    shooter.Salvo();
                }
            }
            yield return _delay;
        }
    }

}
