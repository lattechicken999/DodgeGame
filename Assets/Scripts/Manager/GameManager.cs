using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    [SerializeField] private GameObject _playerPrefeb;

    //시작 시간
    private float _startTime;
    //진행 시간
    private float _curTime;

    private GameObject _player;

    //게임이 진행중인지 외부 참조용
    public bool IsPlaying
    {
        get
        {
            if(_player == null)
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
        if(_player == null)
        {
            _player = Instantiate(_playerPrefeb);
        }
    }

    public void GameEnd()
    {
        init();
    }
    protected override void init()
    {
        _player = null;
        _startTime = -1;
    }
    private void Update()
    {
        if (_player != null)
        {
            //플레이어가 살아 있을 때 계속해서 시간 측정
            _curTime = Time.time - _startTime;
        }
    }
}
