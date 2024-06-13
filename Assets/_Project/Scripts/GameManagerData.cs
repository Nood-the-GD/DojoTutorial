using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // You will need to write this by your self
    [CreateAssetMenu(fileName = "GameManagerData", menuName = "ScriptableObject/GameManagerData")]
    public class GameManagerData : ScriptableObject
    {
        public string privateKey;
        public string accountAddress;
    }
}
