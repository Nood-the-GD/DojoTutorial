using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NOOD;
using System.Collections;
using System.Threading.Tasks;
using Dojo;
using Dojo.Starknet;

namespace Game
{
    public class GameManager : MonoBehaviorInstance<GameManager>
    {
        public Action<bool> onEndGame;

        // Add some field to interact with dojo world
        [SerializeField] private WorldManager _worldManager;
        [SerializeField] private WorldManagerData _worldManagerData;
        [SerializeField] private GameManagerData _gameManagerData;

        public Actions actions;
        public JsonRpcClient provider;
        public Account masterAccount;

        public Action onGameStart;

        private bool _isPlaying;
        private uint _gameId;
        private Dictionary<FieldElement, string> _spawnedAccount = new();
        private List<GameObject> _spawnedObject = new List<GameObject>();

        void Awake()
        {
            _isPlaying = false;
        }

        IEnumerator Start()
        {
            provider = new JsonRpcClient(_worldManagerData.rpcUrl);
            masterAccount = new Account(provider, new SigningKey(_gameManagerData.privateKey), new FieldElement(_gameManagerData.accountAddress));

            _worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InnitEntity);
            foreach(var entity in _worldManager.Entities())
            {
                InnitEntity(entity);
            }
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

        #region Actions
        public async void PlayerAction(SkillType skillType)
        {
            await actions.action(masterAccount, skillType);
        }
        #endregion

        public void PlayGame()
        {
            _isPlaying = true;
            onGameStart?.Invoke();
        }

        public bool IsPlaying()
        {
            return _isPlaying;
        }

        // This is our game logic, you don't need to do this if your game is different
        private void GetGameID()
        {
            foreach(var entity in _worldManager.Entities())
            {
                if (entity.TryGetComponent<Counter>(out Counter counter)) 
                {
                    _gameId = counter.counter;
                }
            }    
        }

        private void SpawnPlayerAndEnemy()
        {
            foreach(var g in _spawnedObject)
            {
                Destroy(g);
            }
            _spawnedObject.Clear();

            foreach(var entity in _worldManager.Entities())
            {
                if(entity.TryGetComponent<Health>(out Health health))
                {
                    if(NoodyCustomCode.CompareHexStrings(health.entityId.Hex(), "0x676f626c696e") && health.gameId == _gameId)
                    {
                        // Only spawn 1 enemy with the suitable gameId
                        _spawnedObject.Add(EnemySpawner.Instance.SpawnEnemy(_gameId));
                    }    
                }
                if(entity.TryGetComponent<Player>(out Player player))
                {
                    // Only spawn 1 player and give it hexCode and gameId
                    _spawnedObject.Add(PlayerSpawner.Instance.SpawnPlayerLocal(player.character, player.player.Hex(), _gameId));
                }
            }
        }

        private void InnitEntity(GameObject entity)
        {
            GetGameID();
            SpawnPlayerAndEnemy();
            
            foreach (var account in _spawnedAccount)
            {
                if(account.Value == null)
                {
                    _spawnedAccount[account.Key] = entity.name;
                    break;
                }
            }
        }
    }
}
