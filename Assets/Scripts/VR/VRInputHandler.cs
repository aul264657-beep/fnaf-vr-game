using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

namespace FNAFVR.VR
{
    /// <summary>
    /// Handles VR controller input for FNAF VR
    /// Manages hand presence, grab interactions, and button inputs
    /// </summary>
    public class VRInputHandler : MonoBehaviour
    {
        [Header("XR Input")]
        [SerializeField] private InputDevice leftController;
        [SerializeField] private InputDevice rightController;

        [Header("Hand References")]
        [SerializeField] private Transform leftHandAnchor;
        [SerializeField] private Transform rightHandAnchor;

        // Input feature names
        private InputFeatureUsage<Vector2> primaryAxis;
        private InputFeatureUsage<bool> primaryButton;
        private InputFeatureUsage<bool> gripButton;
        private InputFeatureUsage<bool> triggerButton;

        // Events for input
        public delegate void InputEvent(InputSource source, InputAction action);
        public event InputEvent OnInput;

        private bool isInitialized = false;

        private void Start()
        {
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithRole(InputDeviceRole.LeftHanded, devices);
            if (devices.Count > 0)
                leftController = devices[0];

            devices.Clear();
            InputDevices.GetDevicesWithRole(InputDeviceRole.RightHanded, devices);
            if (devices.Count > 0)
                rightController = devices[0];

            primaryAxis = CommonUsages.primary2DAxis;
            primaryButton = CommonUsages.primaryButton;
            gripButton = CommonUsages.gripButton;
            triggerButton = CommonUsages.triggerButton;

            isInitialized = leftController.isValid || rightController.isValid;

            if (isInitialized)
                Debug.Log("VR Controllers initialized successfully");
            else
                Debug.LogWarning("No VR controllers found. Using fallback input.");
        }

        private void Update()
        {
            if (!isInitialized)
                return;

            UpdateLeftController();
            UpdateRightController();
        }

        private void UpdateLeftController()
        {
            if (!leftController.isValid)
                return;

            // Check for grab/grip action
            if (leftController.TryGetFeatureValue(gripButton, out bool gripPressed) && gripPressed)
            {
                OnInput?.Invoke(InputSource.LeftHand, InputAction.Grab);
            }

            // Check for trigger action
            if (leftController.TryGetFeatureValue(triggerButton, out bool triggerPressed) && triggerPressed)
            {
                OnInput?.Invoke(InputSource.LeftHand, InputAction.Trigger);
            }

            // Check for thumbstick movement
            if (leftController.TryGetFeatureValue(primaryAxis, out Vector2 thumbstick))
            {
                if (thumbstick.magnitude > 0.1f)
                {
                    OnInput?.Invoke(InputSource.LeftHand, InputAction.Move);
                }
            }
        }

        private void UpdateRightController()
        {
            if (!rightController.isValid)
                return;

            // Check for grab/grip action
            if (rightController.TryGetFeatureValue(gripButton, out bool gripPressed) && gripPressed)
            {
                OnInput?.Invoke(InputSource.RightHand, InputAction.Grab);
            }

            // Check for trigger action
            if (rightController.TryGetFeatureValue(triggerButton, out bool triggerPressed) && triggerPressed)
            {
                OnInput?.Invoke(InputSource.RightHand, InputAction.Trigger);
            }

            // Check for primary button
            if (rightController.TryGetFeatureValue(primaryButton, out bool buttonPressed) && buttonPressed)
            {
                OnInput?.Invoke(InputSource.RightHand, InputAction.UIInteract);
            }
        }

        public Transform GetHandAnchor(InputSource hand)
        {
            return hand == InputSource.LeftHand ? leftHandAnchor : rightHandAnchor;
        }

        public InputDevice GetController(InputSource hand)
        {
            return hand == InputSource.LeftHand ? leftController : rightController;
        }

        public bool IsControllerValid(InputSource hand)
        {
            InputDevice device = GetController(hand);
            return device.isValid;
        }
    }

    public enum InputSource
    {
        LeftHand,
        RightHand,
        Keyboard
    }

    public enum InputAction
    {
        Grab,
        Trigger,
        Move,
        UIInteract,
        Menu
    }
}
