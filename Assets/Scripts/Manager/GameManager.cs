using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private TextMeshProUGUI _TimeText;

    //시작 시간
    private float _startTime;
    //진행 시간
    private float _curTime;
    //초기 플레이어 위치
    private Vector3 _initPosition;
    private Quaternion _initRotation;

    //시간 갱신용 코루틴
    private Coroutine _timeCoroutine;
    private WaitForSeconds _waitTime;

    //게임이 진행중인지 외부 참조용
    public bool IsPlaying
    {
        get
        {
            if(!_player.activeSelf)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public int PlayLevel
    { get 
        { 
            if(_curTime < 10)
            {
                return 0;
            }
            if(_curTime < 20)
            {
                return 1;
            }
            if(_curTime < 30)
            {
                return 2;
            }
            return 3; 
        }
    }

    public void GameStart()
    {
        _startTime = Time.time;
        _player.SetActive(true);
        _player.transform.position = _initPosition;
        _player.transform.rotation = _initRotation;
        _startButton.SetActive(false);
        if (_timeCoroutine == null)
        {
            _timeCoroutine = StartCoroutine(CheckTime());
        }
    }

    public void GameEnd()
    {
        init();
        _startButton.SetActive(true);
        if(_timeCoroutine != null )
        {
            StopCoroutine(_timeCoroutine);
        }
    }
    protected override void init()
    {
        _player.SetActive(false);
        _startTime = -1;
        _initPosition = _player.transform.position;
        _initRotation = _player.transform.rotation;
        _waitTime = new WaitForSeconds(0.01f);
    }
    private void Update()
    {
        if (_player.activeSelf)
        {
            //플레이어가 살아 있을 때 계속해서 시간 측정
            _curTime = Time.time - _startTime;
        }
    }

    private IEnumerator CheckTime()
    {
        while (true)
        {
            _TimeText.text = "Time : " + $"{_curTime:F2}";
            yield return _waitTime;
        }
    }
}
