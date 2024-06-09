using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

public enum PlayerType
{
    Knight,
    Wizard
}

public class PlayerSpawner : MonoBehaviorInstance<PlayerSpawner>
{
    [SerializeField] private Player _knight;
    [SerializeField] private Player _wizard;
    [SerializeField] private Transform _playerSpawnPoint;

    public void SpawnPlayer(PlayerType playerType)
    {
        Instantiate(playerType == PlayerType.Knight ? _knight : _wizard, _playerSpawnPoint.position, Quaternion.identity);
    }
}
