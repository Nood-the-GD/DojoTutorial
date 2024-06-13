using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NOOD;
using UnityEngine;

namespace Game
{
    public enum PlayerType
    {
        Knight,
        Wizard
    }

    public class PlayerSpawner : MonoBehaviorInstance<PlayerSpawner>
    {
        [SerializeField] private PlayerMain _knight;
        [SerializeField] private PlayerMain _wizard;
        [SerializeField] private Transform _playerSpawnPoint;

        // Spawn player using contract
        public async void SpawnPlayer(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Knight:
                    await GameManager.Instance.actions.spawn(GameManager.Instance.masterAccount, new Character.Horseman());
                    break;
                case PlayerType.Wizard:
                    await GameManager.Instance.actions.spawn(GameManager.Instance.masterAccount, new Character.Magician());
                    break;
            }
        }
        
        // Spawn player in local with prefab base on the return data after running actions.spawn command
        public GameObject SpawnPlayerLocal(Character character, string hexCode, uint gameId)
        {
            PlayerMain player;
            if(character.GetType() == typeof(Character.Horseman))
            {
                player = Instantiate(_knight, null);
            }
            else
            {
                player = Instantiate(_wizard, null);
            }
            player.transform.position = _playerSpawnPoint.position;
            player.hexCode = hexCode;
            player.gameId = gameId;
            return player.gameObject;
        }
    }
}