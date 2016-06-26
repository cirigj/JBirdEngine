// uncomment the following line to use this script
//#define JBIRDVR

#if JBIRDVR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViveInputHandler : MonoBehaviour {

    public enum ControllerID {
        unbound,
        left,
        right,
    }

    public enum AxisID {
        System = 0,
        ApplicationMenu = 1,
        Grip = 2,
        DPad_Left = 3,
        DPad_Up = 4,
        DPad_Right = 5,
        DPad_Down = 6,
        A = 7,
        Touchpad = 8,
        Trigger = 9,
        Axis2 = 10,
        Axis3 = 11,
        Axis4 = 12,
    }

    private class InputAxis {
        public Valve.VR.EVRButtonId buttonID;
        public bool pressed;
        public bool held;
        public bool released;

        public InputAxis (Valve.VR.EVRButtonId id) {
            buttonID = id;
            pressed = false;
            held = false;
            released = false;
        }
    }

    [SerializeField]
    [Header("Set to left or right to gain static reference:")]
    private ControllerID _controllerID;
    public ControllerID controllerID {
        get {
            return _controllerID;
        }
    }

    public static ViveInputHandler leftController;
    public static ViveInputHandler rightController;

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device controller {
        get {
            if (trackedObj == null) {
                return null;
            }
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

    bool inputCheckedThisFrame = false;

    List<InputAxis> axes;

    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        SetAxes();
        if (controllerID == ControllerID.left) {
            if (leftController == null) {
                leftController = this;
            }
            else {
                Debug.LogError("ViveInputHandler: More than one controller bound to 'left'.");
            }
        }
        else if (controllerID == ControllerID.right) {
            if (rightController == null) {
                rightController = this;
            }
            else {
                Debug.LogError("ViveInputHandler: More than one controller bound to 'right'.");
            }
        }
        else if (controllerID == ControllerID.unbound) {
            Debug.LogWarningFormat("ViveInputHandler: Controller '{0}' is currently set to 'unbound' and will not be tracked.", gameObject.name);
        }
    }

    void LateUpdate () {
        if (inputCheckedThisFrame) {
            ResetInput();
        }
    }

    void SetAxes () {
        axes = new List<InputAxis>();
        for (int i = 0; i <= 7; i++) {
            axes.Add(new InputAxis((Valve.VR.EVRButtonId)i));
        }
        for (int i = 32; i <= 36; i++) {
            axes.Add(new InputAxis((Valve.VR.EVRButtonId)i));
        }
    }

    InputAxis GetAxis (AxisID id) {
        return axes[(int)id];
    }

    void CheckInput () {
        if (controller == null) {
            Debug.LogWarning("ViveInputHandler: Controller not initialized!");
            return;
        }
        foreach (InputAxis axis in axes) {
            axis.pressed = controller.GetPressDown(axis.buttonID);
            axis.held = controller.GetPress(axis.buttonID);
            axis.released = controller.GetPressUp(axis.buttonID);
        }
        inputCheckedThisFrame = true;
    }

    void ResetInput () {
        foreach (InputAxis axis in axes) {
            axis.pressed = false;
            axis.held = false;
            axis.released = false;
        }
        inputCheckedThisFrame = false;
    }

    public static bool GetButtonPressed (AxisID button, ControllerID hand = ControllerID.unbound) {
        if (hand == ControllerID.unbound) {
            if (leftController == null) {
                hand = ControllerID.right;
            }
            else if (rightController == null) {
                hand = ControllerID.left;
            }
        }
        switch (hand) {
            case ControllerID.left:
                if (leftController == null) {
                    return false;
                }
                return leftController.GetPressed(button);
            case ControllerID.right:
                if (rightController == null) {
                    return false;
                }
                return rightController.GetPressed(button);
            default:
                return (rightController.GetPressed(button) || leftController.GetPressed(button));
        }
    }

    bool GetPressed (AxisID button) {
        if (!inputCheckedThisFrame) {
            CheckInput();
        }
        return GetAxis(button).pressed;
    }

    public static bool GetButtonHeld (AxisID button, ControllerID hand = ControllerID.unbound) {
        if (hand == ControllerID.unbound) {
            if (leftController == null) {
                hand = ControllerID.right;
            }
            else if (rightController == null) {
                hand = ControllerID.left;
            }
        }
        switch (hand) {
            case ControllerID.left:
                if (leftController == null) {
                    return false;
                }
                return leftController.GetHeld(button);
            case ControllerID.right:
                if (rightController == null) {
                    return false;
                }
                return rightController.GetHeld(button);
            default:
                return (rightController.GetHeld(button) || leftController.GetHeld(button));
        }
    }

    bool GetHeld (AxisID button) {
        if (!inputCheckedThisFrame) {
            CheckInput();
        }
        return GetAxis(button).held;
    }

    public static bool GetButtonReleased (AxisID button, ControllerID hand = ControllerID.unbound) {
        if (hand == ControllerID.unbound) {
            if (leftController == null) {
                hand = ControllerID.right;
            }
            else if (rightController == null) {
                hand = ControllerID.left;
            }
        }
        switch (hand) {
            case ControllerID.left:
                if (leftController == null) {
                    return false;
                }
                return leftController.GetReleased(button);
            case ControllerID.right:
                if (rightController == null) {
                    return false;
                }
                return rightController.GetReleased(button);
            default:
                return (rightController.GetReleased(button) || leftController.GetReleased(button));
        }
    }

    bool GetReleased (AxisID button) {
        if (!inputCheckedThisFrame) {
            CheckInput();
        }
        return GetAxis(button).released;
    }

}
#endif