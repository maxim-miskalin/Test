using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private int _poolCapacity = 10;
    [SerializeField] private int _poolMaxSize = 20;
    [SerializeField] private float _initialSpawnTime = 10f;

    private float _timer;
    private ObjectPool<EnemyController> _pool;
    private int _enemyCounter = 0;
    private int _enemyDeath = 0;

    public float Timer => _timer;
    public int EnemyDeath => _enemyDeath;

    private void Awake()
    {
        _pool = new(CreateEnemy, ContineAction, ReturnToPool, Destroy, true, _poolCapacity, _poolMaxSize);
    }

    private void OnEnable()
    {
        _game.StartGame += StartSpawn;
        _game.FinishGame += FinishSpawn;
    }

    private void OnDisable()
    {
        _game.StartGame -= StartSpawn;
        _game.FinishGame -= FinishSpawn;
    }

    public void ReturnToPool(EnemyController enemy)
    {
        _enemyDeath++;
        enemy.gameObject.SetActive(false);
    }

    private void StartSpawn()
    {
        _enemyCounter = 0;
        _enemyDeath = 0;
        _timer = 0;
        StartCoroutine(StartTimer());
        StartCoroutine(GetEnemy());
    }

    private void FinishSpawn()
    {
        StopCoroutine(GetEnemy());
    }

    private IEnumerator GetEnemy()
    {
        while (_game.IsWork)
        {
            _pool.Get();
            _enemyCounter++;


            float wait = _initialSpawnTime;
            wait += _enemyCounter;
            wait -= _timer / 1000f;

            yield return new WaitForSeconds(wait);
        }
    }

    private EnemyController CreateEnemy()
    {
        EnemyController enemy = Instantiate(_enemy);
        enemy.SetBase(this);
        enemy.gameObject.SetActive(false);

        return enemy;
    }

    private void ContineAction(EnemyController enemy)
    {
        enemy.transform.position = this.transform.position;
        enemy.GetComponent<EnemyHealth>().Resurrect();
        enemy.gameObject.SetActive(true);
    }

    private IEnumerator StartTimer()
    {
        _timer++;
        yield return new WaitForSeconds(1f);
    }
}
