using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Shooter : MonoBehaviour
{
    //목표
    [SerializeField] Transform _target;
    //총알 프리팹
    [SerializeField] GameObject _bulletPrefeb;
    //단계별 발사 쿨타임
    [SerializeField] float[] _shootCoolTimes;
    //단계별 동시발사 갯수
    [SerializeField] int[] _shootSameTime;

    //총알 쏠 위치 저장
    private List<Transform> _localSpawnPoints;
    //총알 미리 소환
    private List<GameObject> _bulletPool;

    //코루틴 저장용
    private Coroutine _levelCheckCoroutine;
    private Coroutine _shootingCoroutine;

    //레벨별 슈팅 쿨타임 관리용 
    private List<WaitForSeconds> _waitShootCools;
    //레벨 체크용 wait
    private WaitForSeconds _waitLevel;
    //랜덤 뽑기용 랜드
    private System.Random _rnd;

    //레벨 저장용
    private int _curLevel;

    /// <summary>
    /// 총알 발사용 함수
    /// </summary>
    /// <param name="spawnPoint">총알이 발사되는 위치</param>
    private void FireBullet(Transform spawnPoint)
    {
        //오브젝트 중 비활성화 된 총알이 있다면 발사
        foreach(var bullet in _bulletPool)
        {
            if(!bullet.activeSelf)
            {
                bullet.transform.position = spawnPoint.position;
                bullet.transform.rotation = spawnPoint.rotation;
                bullet.gameObject.SetActive(true);
                return;
            }
        }
        //모든 총알이 사용중이라면 새로 생성
        var newBullet = Instantiate(_bulletPrefeb);
        newBullet.transform.position = spawnPoint.position;
        newBullet.transform.rotation = spawnPoint.rotation;
        _bulletPool.Add(newBullet);
    }
    
    /// <summary>
    /// 전탄발사
    /// 외부에서 호출할 경우 사용
    /// </summary>
    public void Salvo()
    {
        foreach (var localPoints in _localSpawnPoints)
        {
            localPoints.LookAt(_target);
            FireBullet(localPoints);
        }
    }
}

public partial class Shooter : MonoBehaviour
{
    private void Awake()
    {
        _waitShootCools = new List<WaitForSeconds>();
        _bulletPool = new List<GameObject>();
        _localSpawnPoints = new List<Transform>();
    }
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _localSpawnPoints.Add(transform.GetChild(i));
            //총알 미리 생성
            _bulletPool.Add(Instantiate(_bulletPrefeb));
            _bulletPool.Last().gameObject.SetActive(false);
        }

        foreach (var coolTime in _shootCoolTimes)
        {
            _waitShootCools.Add(new WaitForSeconds(coolTime));
        }
        _waitLevel = new WaitForSeconds(1);
    }
    private void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            if(_shootingCoroutine == null)
            {
                _shootingCoroutine = StartCoroutine(AutoAttack());
            }
            if (_levelCheckCoroutine == null)
            {
                _levelCheckCoroutine = StartCoroutine(LevelCheck());
            }
        }
        else
        {
            if (_shootingCoroutine != null)
            {
                StopCoroutine(_shootingCoroutine);
            }
            if(_levelCheckCoroutine != null)
            {
                StopCoroutine(_levelCheckCoroutine);
            }
            
        }

    }
}

public partial class Shooter : MonoBehaviour
{
    /// <summary>
    /// 자동 공격용 함수
    /// 동시에 몇발을 몇초에 한번 발사하는지
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoAttack()
    {
        for(int i =0;i< _shootSameTime[_curLevel];i++)
        {
            //랜덤으로 가져와 발사하기
            //같은거 가져오면 뭐.. 아쉬운거지
            FireBullet(_localSpawnPoints[_rnd.Next(0, _localSpawnPoints.Count)]);
        }
       yield return _waitShootCools[_curLevel];
    }
    /// <summary>
    /// 1초마다 레벨 체크
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelCheck()
    {
        _curLevel = GameManager.Instance.PlayLevel;
        yield return _waitLevel;
    }
}