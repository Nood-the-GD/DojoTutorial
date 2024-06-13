using System.Collections;
using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using NOOD;
using UnityEngine;

namespace Game
{
    // This class use to find entity base on hexCode and gameId
    public class EntityManager : MonoBehaviorInstance<EntityManager>
    {
        [SerializeField] private WorldManager _worldManager;
        
        public T GetEntity<T>(string hexCode, uint gameId) where T : ModelInstance
        {
            foreach(var entity in _worldManager.Entities())
            {
                if(entity.TryGetComponent<T>(out T component))
                {
                    FieldElement entityId = (FieldElement)component.Model.Members["entityId"].value;
                    uint _gameId = (uint)component.Model.Members["gameId"].value;
                    if(NoodyCustomCode.CompareHexStrings(hexCode, entityId.Hex()) && _gameId == gameId)
                    {
                        return (T)component;
                    }
                }
            }
            Debug.LogError("can't find component: " + typeof(T).Name);
            return null;
        }    
    }
}
