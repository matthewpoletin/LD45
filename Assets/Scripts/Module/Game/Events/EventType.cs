using System;

namespace Module.Game.Events
{
    [Serializable]
    public enum EventType
    {
        PlayerAttack = 0,
        PlayerDamaged = 1,
        PlayerJump = 2,
        PlayerHumiliate = 3,
        ToothSpawn = 4,
        TongueSpawn = 5,
        TongueAim = 6,
        TongueAttack = 7,
        TongueDeath = 8,
    }
}