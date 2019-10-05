using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Nothing
{
    public enum Line
    {
        Left,
        Middle,
        Right
    }

    public class Player : MonoBehaviour, IPlayerControllable
    {
        public Line currentLine = Line.Middle;
        public float lineWidth = 3;
        public float lineChangeDuration = 1;

        [field: SerializeField]
        public bool IsChangingLine { get; private set; } = false;

        [SerializeField]
        private float currentVelocity = 0;

        public PlayerInput input;

        private void Awake()
        {
            input.controllable = this;
        }

        public void Up()
        {
        }

        public void Down()
        {
        }

        public void Jump()
        {
        }

        public void Left()
        {
            if (currentLine == Line.Left)
                return;

            if (currentLine == Line.Middle)
                currentLine = Line.Left;
            else if (currentLine == Line.Right)
                currentLine = Line.Middle;

            OnLineChange();
        }

        public void Right()
        {
            if (currentLine == Line.Right)
                return;

            if (currentLine == Line.Middle)
                currentLine = Line.Right;
            else if (currentLine == Line.Left)
                currentLine = Line.Middle;

            OnLineChange();
        }


        private void OnLineChange()
        {
            IsChangingLine = true;
        }

        public void Update()
        {
            if (IsChangingLine)
                UpdateLinePosition();
        }

        private bool ApproxEqual(float a, float b, float tolerance) => Mathf.Abs(a - b) < tolerance;

        private void UpdateLinePosition()
        {
            var targetX = currentLine == Line.Left ? -lineWidth :
                        currentLine == Line.Middle ? 0 :
                        currentLine == Line.Right ? lineWidth : 0;

            var newX = Mathf.SmoothDamp(transform.localPosition.x, targetX, ref currentVelocity, lineChangeDuration);

            if (ApproxEqual(transform.localPosition.x, targetX, .01f))
            {
                IsChangingLine = false;
                newX = targetX;
            }

            transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
