using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void OnShootAction();
        public delegate void OnMoveAction(Vector3 moveVector);
        public delegate void OnAimAction(Vector3 rotationVector);

        public static event OnShootAction onShoot;
        public static event OnMoveAction onMove;
        public static event OnAimAction onAim;
        private const float rotationThreshold = 0.0625f;

        private void Update()
        {
            if (CubeInputs.GetFiring < -0.3f)
            {
                onShoot();
            }

            Vector3 moveVector = new Vector3(CubeInputs.GeHorizontalMovement, 0f, CubeInputs.GeVerticalMovement);
            if (moveVector.sqrMagnitude != 0f)
            {
                onMove(moveVector);
            }

            Vector3 rotationVector = Vector3.Normalize(new Vector3(CubeInputs.GetHorizontalAimDirection, 0.0f, CubeInputs.GetVerticalAimDirection));
            //Makes sure player is activally trying to rotate
            if (rotationVector.sqrMagnitude > rotationThreshold)
            {
                onAim(rotationVector);
            }
        }
    }

    public class MenuController : MonoBehaviour
    {
        public delegate void OnPauseAction();

        public static event OnPauseAction onPause;

        private void Update()
        {
            if (CubeInputs.IsPausing)
            {
                onPause();
            }
        }
    }
}