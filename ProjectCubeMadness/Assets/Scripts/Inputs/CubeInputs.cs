using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a centralized input class to make it easier to make changes in the future.
/// </summary>
public class CubeInputs
{
    private static CubeInputs instance;

    public static bool IsPausing {
        get {
#if UNITY_EDITOR_OSX
            return Input.GetKeyDown(KeyCode.Joystick1Button9);
#else
            return Input.GetKeyDown(KeyCode.Joystick1Button7);
#endif
        }
    }

    public static float GetFiring { get{ return Input.GetAxis("Fire");  } }
    public static float GeHorizontalMovement { get { return Input.GetAxis("Horizontal.LStick"); } }
    public static float GeVerticalMovement { get { return Input.GetAxis("Vertical.LStick"); } }
    public static float GetHorizontalAimDirection { get { return Input.GetAxis("Horizontal.RStick"); } }
    public static float GetVerticalAimDirection { get { return Input.GetAxis("Vertical.RStick"); } }
}
