using UnityEngine;

namespace Nothing
{
    public interface IPlayerControllable
    {
        void Right();
        void Left();
        void Down();
        void Up();

        void Jump();
    }
}