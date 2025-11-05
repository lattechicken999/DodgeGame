using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //플레이어 설정
    [SerializeField] float _mooveSpeed;
    [SerializeField] float _rotateSpeed;
    [SerializeField] int _initHp;

    //플레이어 이동범위 관련
    [SerializeField] Transform _wallTop;
    [SerializeField] Transform _wallDown;
    [SerializeField] Transform _wallLeft;
    [SerializeField] Transform _wallRight;

    //Moving 관련
    private Vector3 _moveDir;
    private Vector2 _userInputVector;
    private Vector3 _curPosition;

    //이동 범위 관련
    private float _minPosX;
    private float _minPosZ;
    private float _maxPosX;
    private float _maxPosZ;

    //플레이어 피 설정
    private int _curHp;

    public void OnWASD(InputAction.CallbackContext ctx)
    {
        _userInputVector = ctx.ReadValue<Vector2>();
        _moveDir = new Vector3(_userInputVector.x, 0, _userInputVector.y);
    }

    public void TakenDamage()
    {
        _curHp -= 1;
        if(_curHp <= 0)
        {
            GameManager.Instance.GameEnd();
        }
    }

    private void Moving()
    {
        if (_moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                                   Quaternion.LookRotation(_moveDir),
                                                                   _rotateSpeed * Time.deltaTime);
            _curPosition = transform.position;
            transform.Translate(Vector3.forward * _mooveSpeed * Time.deltaTime);

            //범위를 벗어나면 복귀
            if(transform.position.x < _minPosX ||
               transform.position.x > _maxPosX ||
               transform.position.z < _minPosZ ||
               transform.position.z > _maxPosZ)
            {
                transform.position = _curPosition;
            }
        }
    }
    private void Start()
    {
        //플레이어 이동 범위 설정
        var selfObj = gameObject.GetComponent<Collider>().bounds.size.x/2;
        _minPosX = _wallLeft.GetComponent<Collider>().bounds.max.x + selfObj;
        _maxPosX = _wallRight.GetComponent<Collider>().bounds.min.x - selfObj;
        _minPosZ = _wallDown.GetComponent<Collider>().bounds.max.z + selfObj;
        _maxPosZ = _wallTop.GetComponent<Collider>().bounds.min.z - selfObj;

        _curHp = _initHp;
    }
    private void Update()
    {
        Moving();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            TakenDamage();
        }
    }
}
