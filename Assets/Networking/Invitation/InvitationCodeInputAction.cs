// GENERATED AUTOMATICALLY FROM 'Assets/Networking/Invitation/InvitationCodeInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InvitationCodeInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InvitationCodeInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InvitationCodeInputAction"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f221172a-f867-4cff-94dc-c1cf0b1b2419"",
            ""actions"": [
                {
                    ""name"": ""AddX"",
                    ""type"": ""Button"",
                    ""id"": ""9e9ffb80-b169-4660-a526-7c5c571ee578"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddA"",
                    ""type"": ""Button"",
                    ""id"": ""e5adbe2e-7e18-4db3-9d05-52c97a2296a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddB"",
                    ""type"": ""Button"",
                    ""id"": ""1ef8fd83-e22b-47f1-9c26-fc64192785e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddY"",
                    ""type"": ""Button"",
                    ""id"": ""2eb52b38-bc32-4368-bc9d-9bf6391a6b2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7e0ebc23-7fb9-4790-92c0-af56d93bd407"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21fb2994-9f1f-4c81-99bc-ddc9c23de405"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8955f43-d09c-4fc8-b657-2cdfb0482fd5"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26d53c46-9775-4fb6-aa56-18c725678e43"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_AddX = m_Player.FindAction("AddX", throwIfNotFound: true);
        m_Player_AddA = m_Player.FindAction("AddA", throwIfNotFound: true);
        m_Player_AddB = m_Player.FindAction("AddB", throwIfNotFound: true);
        m_Player_AddY = m_Player.FindAction("AddY", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_AddX;
    private readonly InputAction m_Player_AddA;
    private readonly InputAction m_Player_AddB;
    private readonly InputAction m_Player_AddY;
    public struct PlayerActions
    {
        private @InvitationCodeInputAction m_Wrapper;
        public PlayerActions(@InvitationCodeInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddX => m_Wrapper.m_Player_AddX;
        public InputAction @AddA => m_Wrapper.m_Player_AddA;
        public InputAction @AddB => m_Wrapper.m_Player_AddB;
        public InputAction @AddY => m_Wrapper.m_Player_AddY;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @AddX.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddX;
                @AddX.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddX;
                @AddX.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddX;
                @AddA.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddA;
                @AddA.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddA;
                @AddA.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddA;
                @AddB.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddB;
                @AddB.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddB;
                @AddB.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddB;
                @AddY.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddY;
                @AddY.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddY;
                @AddY.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAddY;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddX.started += instance.OnAddX;
                @AddX.performed += instance.OnAddX;
                @AddX.canceled += instance.OnAddX;
                @AddA.started += instance.OnAddA;
                @AddA.performed += instance.OnAddA;
                @AddA.canceled += instance.OnAddA;
                @AddB.started += instance.OnAddB;
                @AddB.performed += instance.OnAddB;
                @AddB.canceled += instance.OnAddB;
                @AddY.started += instance.OnAddY;
                @AddY.performed += instance.OnAddY;
                @AddY.canceled += instance.OnAddY;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnAddX(InputAction.CallbackContext context);
        void OnAddA(InputAction.CallbackContext context);
        void OnAddB(InputAction.CallbackContext context);
        void OnAddY(InputAction.CallbackContext context);
    }
}
