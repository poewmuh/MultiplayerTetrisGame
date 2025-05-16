using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Tetris.Helpers
{
    public static class InputHelper
    {
        public static bool WasAnyButtonDown()
        {
            return WasGamePadPressed() || WasKeyboardPressed() || WasMousePressed();
        }
        
        public static bool WasKeyboardPressed()
        {
            return Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        }

        public static bool WasGamePadPressed()
        {
            if (Gamepad.current == null) return false;
            
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is ButtonControl button && button.wasPressedThisFrame)
                    return true;
            }

            return false;
        }

        public static bool WasMousePressed()
        {
            if (Mouse.current == null) return false;
            
            foreach (var control in Mouse.current.allControls)
            {
                if (control is ButtonControl button && button.wasPressedThisFrame)
                    return true;
            }

            return false;
        }
    }
}