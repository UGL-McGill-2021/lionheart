using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Layouts;
namespace AssemblyCSharp.Assets
{
    [InputControlLayout(stateType = typeof(DualShock4HIDInputReport))]
    #if UNITY_EDITOR
    [InitializeOnLoad] // Make sure static constructor is called during startup.
    #endif

    public class DualShock4GamepadHID : Gamepad
    {
        static DualShock4GamepadHID()
        {
            // This is one way to match the Device.
            /*
            InputSystem.RegisterLayout<DualShock4GamepadHID>("DualShock4GamepadHID",
                new InputDeviceMatcher()
                    .WithInterface("HID")
                    .WithManufacturer("Sony.+Entertainment")
                    .WithProduct("Wireless Controller"));
            */

            // Alternatively, you can also match by PID and VID, which is generally
            // more reliable for HIDs.
            // (PID and VID can be seen in the input debugger)
            //InputSystem.RegisterLayout<DualShock4GamepadHID>(
            //    matches: new InputDeviceMatcher()
            //        .WithInterface("HID")
            //        .WithCapability("vendorId", 0x54C) // Sony Entertainment.
            //        .WithCapability("productId", 0x5C4)); // Wireless controller.
        }

        // In the Player, to trigger the calling of the static constructor,
        // create an empty method annotated with RuntimeInitializeOnLoadMethod.
        [RuntimeInitializeOnLoadMethod]
        static void Init() { }
    }
}
