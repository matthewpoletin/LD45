using UnityEngine;

namespace Module.Game
{
    [CreateAssetMenu(fileName = "New Game Params", menuName = "Params/Game/GameParams", order = 0)]
    public class GameParams : ScriptableObject
    {
        [SerializeField] private string GameName;
    }
}