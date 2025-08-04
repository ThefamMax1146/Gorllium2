using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

namespace XRInput
{
    public enum XRHand
    {
        RightHand,
        LeftHand
    }

    public static class XRInputs
    {
        // Scripts is made by HuhMonke as a better alternative for easyinputs with more features

        #region Actions

        public static event Action OnThumbStickPressed, OnThumbStickReleased;
        public static event Action OnGripPressed, OnGripReleased;
        public static event Action OnTriggerPressed, OnTriggerReleased;
        public static event Action OnPrimaryButtonPressed, OnPrimaryButtonReleased;
        public static event Action OnSecondaryButtonPressed, OnSecondaryButtonReleased;
        public static event Action OnMenuButtonPressed, OnMenuButtonReleased;
        public static event Action OnLeftHandDisconnect, OnRightHandDisconnect;

        private static bool LastGrip, LastTrigger, LastThumbstick, LastPrimary, LastSecondary, LastMenu, LHandConnection, RHandConnection, CheckedR, CheckedL;


        public static void Update()
        {
            CheckHands(XRHand.RightHand);
            CheckHands(XRHand.LeftHand);
        }

        private static void CheckHands(XRHand Hand)
        {
            Checks(GetGripDown(Hand), ref LastGrip, OnGripPressed, OnGripReleased);
            Checks(GetTriggerDown(Hand), ref LastTrigger, OnTriggerPressed, OnTriggerReleased);
            Checks(GetThumbstickDown(Hand), ref LastThumbstick, OnThumbStickPressed, OnThumbStickReleased);
            Checks(GetPrimaryButtonDown(Hand), ref LastPrimary, OnPrimaryButtonPressed, OnPrimaryButtonReleased);
            Checks(GetSecondaryButtonDown(Hand), ref LastSecondary, OnSecondaryButtonPressed, OnSecondaryButtonReleased);
            Checks(GetMenuButtonDown(Hand), ref LastMenu, OnMenuButtonPressed, OnMenuButtonReleased);
            CheckControllerConnectionUpdate();
            CheckControllerConnection(LHandConnection, OnLeftHandDisconnect, ref CheckedL);
            CheckControllerConnection(RHandConnection, OnRightHandDisconnect, ref CheckedR);    
        }

        private static void Checks(bool current,ref bool last, Action OnPressed,Action OnReleased)
        {
            if(current && !last)
            {
                OnPressed?.Invoke();
            }
            else if(!current && last)
            {
                OnReleased?.Invoke();
            }

            last = current;
        }


        #endregion

        #region Controller Connection
        private static void CheckControllerConnection(bool Connected, Action OnDisconnect, ref bool Checked)
        {
            if (!Connected && !Checked)
            {
                OnDisconnect?.Invoke();
                Checked = true;
            }
            if (Connected && Checked)
            {
                Checked = false;
            }
        }

        private static void CheckControllerConnectionUpdate()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevices(devices);

            RHandConnection = false;
            LHandConnection = false;
            foreach(var device in devices)
            {
                if((device.characteristics & InputDeviceCharacteristics.Right) != 0)
                {
                    RHandConnection = true;
                }
                if((device.characteristics & InputDeviceCharacteristics.Left) != 0)
                {
                    LHandConnection = true;
                }
            }
        }
        #endregion

        #region Get Stuff

        private static bool lastMenuButton = false;

        /// <summary>
        /// Checks if the left hand menu button was just pressed this frame
        /// </summary>
        public static bool GetMenuButtonClicked()
        {
            GetBool(XRHand.LeftHand, CommonUsages.menuButton, out bool isPressed);
            bool wasJustPressed = isPressed && !lastMenuButton;
            lastMenuButton = isPressed;
            return wasJustPressed;
        }
        /// <summary>
        /// Checks if the primary button was held down for a certain amount of time
        /// </summary>
        /// <param name="Hand"></param>
        /// <param name="time"></param>
        /// <returns></returns>

        public static bool WasPrimaryButtonHeldDownFor(XRHand Hand, float time)
        {
            GetBool(Hand, CommonUsages.primaryButton, out bool ButtonDown);
            if (ButtonDown)
            {
                return Time.time > time;
            }
            return false;
        }

        /// <summary>
        /// Checks if the secondary button was held down for a certain amount of time
        /// </summary>
        /// <param name="Hand"></param>
        /// <param name="time"></param>
        /// <returns></returns>

        public static bool WasSecondaryButtonHeldDownFor(XRHand Hand, float time)
        {
            GetBool(Hand, CommonUsages.secondaryButton, out bool ButtonDown);
            if (ButtonDown)
            {
                return Time.time > time;
            }
            return false;
        }

        /// <summary>
        /// Checks if menu button is pressed
        /// </summary>
        public static bool GetMenuButtonDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.menuButton, out bool MenuButtonDown);
            return MenuButtonDown;
        }

        /// <summary>
        /// Checks if grip is held down
        /// </summary>
        public static bool GetGripDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.gripButton, out bool GripDown);
            return GripDown;
        }

        /// <summary>
        /// Checks if grip is released
        /// </summary>
        public static bool GetGripUp(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.gripButton, out bool GripDown);
            return !GripDown;
        }

        /// <summary>
        /// Checks if trigger & grip is held down Someone requested this so i added it dont know what to be used for tho
        /// </summary>
        public static bool GetGripAndTriggerDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.triggerButton, out bool TriggerDown);
            GetBool(Hand, CommonUsages.gripButton, out bool GripDown);

            return TriggerDown && GripDown;
        }

        /// <summary>
        /// Checks if trigger held down
        /// </summary>
        public static bool GetTriggerDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.triggerButton, out bool TriggerDown);
            return TriggerDown;
        }

        /// <summary>
        /// Checks if trigger is released
        /// </summary>
        public static bool GetTriggerUp(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.triggerButton, out bool TriggerDown);
            return !TriggerDown;
        }

        /// <summary>
        /// Gets the float of grip
        /// </summary>
        public static float GetGripFloat(XRHand Hand)
        {
            GetFloat(Hand, CommonUsages.grip, out float GripFloat);
            return GripFloat;
        }

        /// <summary>
        /// Gets the float of trigger
        /// </summary>
        public static float GetTriggerFloat(XRHand Hand)
        {
            GetFloat(Hand, CommonUsages.trigger, out float TriggerFloat);
            return TriggerFloat;
        }

        /// <summary>
        /// Checks if primary button is down | A & X Buttons
        /// </summary>
        public static bool GetPrimaryButtonDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.primaryButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Checks if primary button is released | A & X Buttons
        /// </summary>
        public static bool GetPrimaryButtonUp(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.primaryButton, out bool ButtonDown);
            return !ButtonDown;
        }

        /// <summary>
        /// Checks if primary button is touched | A & X Buttons
        /// </summary>
        public static bool GetPrimaryButtonTouched(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.primaryTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Checks if secondary button is down | B & Y Buttons
        /// </summary>
        public static bool GetSecondaryButtonDown(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.secondaryButton, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Checks if secondary button is released | B & Y Buttons
        /// </summary>
        public static bool GetSecondaryButtonUp(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.secondaryButton, out bool ButtonDown);
            return !ButtonDown;
        }

        /// <summary>
        /// Checks if secondary button is touched | B & Y Buttons
        /// </summary>
        public static bool GetSecondaryButtonTouched(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.secondaryTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Checks if Thumbstick button is released
        /// </summary>
        public static bool GetThumbstickUp(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.primary2DAxisTouch, out bool ButtonDown);
            return !ButtonDown;
        }

        /// <summary>
        /// Checks if Thumbstick button is touched
        /// </summary>
        public static bool GetThumbstickTouched(XRHand Hand)
        {
            GetBool(Hand, CommonUsages.primary2DAxisTouch, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Checks if Thumbstick button is held down
        /// </summary>
        public static bool GetThumbstickDown(XRHand Hand)
        {
            GetBool(Hand,CommonUsages.primary2DAxisClick, out bool ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Gets the axis of thumbstick
        /// </summary>
        public static Vector2 GetThumbstickAxis(XRHand Hand)
        {
            GetVector2(Hand, CommonUsages.primary2DAxis, out Vector2 ButtonDown);
            return ButtonDown;
        }

        /// <summary>
        /// Vibrates controller depending on amount & duration
        /// </summary>
        public static IEnumerator Vibrate(XRHand Hand, float Amount, float Duration)
        {
            float time = Time.deltaTime;
            uint channel = 0U;
            InputDevice device;
            if(Hand == XRHand.RightHand)
            {
                device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            }
            else
            {
                device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            }
            while(Time.time < time + Duration)
            {
                device.SendHapticImpulse(channel, Amount,Duration);
                yield return new WaitForSeconds(Duration * 0.9f);
            }
            yield break;
        }

        // U down need to worry about these under its just for easier calling

        // Gets a vector 2 depending on usage for each hand
        static Vector2 GetVector2(XRHand hand, InputFeatureUsage<Vector2> usage, out Vector2 value)
        {
            if(hand == XRHand.RightHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        // Gets the float of a usage depending on hand
        static float GetFloat(XRHand hand, InputFeatureUsage<float> usage, out float value)
        {
            if (hand == XRHand.RightHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }

        // Checks if the usage is value and returns depending on which hand
        static bool GetBool(XRHand hand, InputFeatureUsage<bool> usage, out bool value)
        {
            if(hand == XRHand.RightHand)
            {
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(usage, out value);
                return value;
            }
            else
            {
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(usage, out value);
                return value;
            }
        }
        #endregion

        // Scripts is made by HuhMonke as a better alternative for easyinputs with more features
    }
}