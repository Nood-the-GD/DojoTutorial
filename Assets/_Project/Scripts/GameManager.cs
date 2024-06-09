using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public class GameManager : MonoBehaviorInstance<GameManager>
{
    public Action<bool> onEndGame;

    public Action onGameStart;

    private bool _isPlaying;

    void Awake()
    {
        _isPlaying = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        _isPlaying = true;
    }

    public void CheckWin()
    {
        if (BattleManager.Instance.GetState() == BattleState.Win)
        {
            _isPlaying = false;
            onEndGame?.Invoke(true);
        }
        else
        {
            onEndGame?.Invoke(false);
            _isPlaying = false;
        }
    }

    public void PlayGame()
    {
        _isPlaying = true;
        onGameStart?.Invoke();
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }
}
