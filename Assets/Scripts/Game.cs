using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerHealth _player;
    [SerializeField] private Canvas _statistics;
    [SerializeField] private Canvas _management;
    [SerializeField] private TextMeshProUGUI _counterPoints;
    [SerializeField] private List<Spawner> _spawners;

    private float _counter = 0;

    public event Action StartGame;
    public event Action FinishGame;

    public bool IsWork => _player.IsLife;

    private void OnEnable()
    {
        _player.DieHero += FinishedGame;
    }

    private void OnDisable()
    {
        _player.DieHero -= FinishedGame;
    }

    public void LaunchGame()
    {
        _statistics.gameObject.SetActive(false);
        _management.gameObject.SetActive(true);

        if (!_player.IsLife)
            _player.Resurrect();

        StartGame?.Invoke();
    }

    private void CountPoints()
    {
        foreach (Spawner spawner in _spawners)
            _counter += spawner.Timer + spawner.EnemyDeath;
    }

    private void FinishedGame()
    {
        _management.gameObject.SetActive(false);
        StartCoroutine(EnableStatistics());
        FinishGame?.Invoke();
    }

    private IEnumerator EnableStatistics()
    {
        _statistics.gameObject.SetActive(true);
        CountPoints();
        yield return new WaitForSeconds(5f);
        _counterPoints.text = $"{_counter} points";
    }
}
