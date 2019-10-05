﻿using UnityEngine;

namespace Nothing
{
    public class PlayerInput : MonoBehaviour
    {
        public KeyCode[] upKeys     = new KeyCode[] { KeyCode.W, KeyCode.UpArrow };
        public KeyCode[] downKeys   = new KeyCode[] { KeyCode.S, KeyCode.DownArrow };
        public KeyCode[] leftKeys   = new KeyCode[] { KeyCode.A, KeyCode.LeftArrow };
        public KeyCode[] rightKeys  = new KeyCode[] { KeyCode.D, KeyCode.RightArrow };

        public KeyCode[] jumpKeys   = new KeyCode[] { KeyCode.Space };

        public Player player;

        private void Update()
        {
            foreach(var key in upKeys)
                if (Input.GetKeyDown(key))
                    player.Up();

            foreach (var key in downKeys)
                if (Input.GetKeyDown(key))
                    player.Down();

            foreach (var key in leftKeys)
                if (Input.GetKeyDown(key))
                    player.Left();

            foreach (var key in rightKeys)
                if (Input.GetKeyDown(key))
                    player.Right();

            foreach (var key in jumpKeys)
                if (Input.GetKeyDown(key))
                    player.Jump();
        }
    }
}