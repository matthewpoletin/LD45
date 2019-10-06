using System;

namespace Module.Game.Level.Phase
{
    [Serializable]
    public enum PhaseType
    {
        Timeout = 0,
        EnemiesKilled = 1,
        BossKill = 2
    }
}