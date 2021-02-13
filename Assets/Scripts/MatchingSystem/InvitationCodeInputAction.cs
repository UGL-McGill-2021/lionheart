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
                },
                {
                    ""name"": ""Talk"",
                    ""type"": ""Button"",
                    ""id"": ""fca7d825-2984-46d6-a5bb-c3df2463a75a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mute"",
                    ""type"": ""Button"",
                    ""id"": ""b63ef39d-89f3-433c-93af-7db6dba150b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""8fbd340e-2caf-414e-a662-cd452bb0bf54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""fc6f6f53-821b-47e9-95ba-3d1b66497218"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7e0ebc23-7fb9-4790-92c0-af56d93bd407"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""894a94d1-6a02-4283-bd17-57bd131696cf"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21fb2994-9f1f-4c81-99bc-ddc9c23de405"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c6c1909-dc73-4350-98d5-b6ac32c901f0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddA"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8955f43-d09c-4fc8-b657-2cdfb0482fd5"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb76a7ba-cc02-47ec-a807-416b740bba03"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26d53c46-9775-4fb6-aa56-18c725678e43"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""AddY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""275564aa-7824-494c-9e95-5ab1fae8fd02"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc0a7287-9b41-4667-95c1-60be55481655"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Talk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb21a549-7013-4b8e-aef7-a3a4620c7105"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Talk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""402eadf2-b682-49f9-9ac5-87e1959d23ec"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a51a4f7-b443-4329-9c0c-1a2c64ad66d8"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26490d57-8a11-445c-8326-f5d5d9308c2b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37744fd8-6401-450c-8f7d-a63dddbf39e8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
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
        m_Player_Talk = m_Player.FindAction("Talk", throwIfNotFound: true);
        m_Player_Mute = m_Player.FindAction("Mute", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Fire = m_Player.FindAction("Fire", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Talk;
    private readonly InputAction m_Player_Mute;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Fire;
    public struct PlayerActions
    {
        private @InvitationCodeInputAction m_Wrapper;
        public PlayerActions(@InvitationCodeInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddX => m_Wrapper.m_Player_AddX;
        public InputAction @AddA => m_Wrapper.m_Player_AddA;
        public InputAction @AddB => m_Wrapper.m_Player_AddB;
        public InputAction @AddY => m_Wrapper.m_Player_AddY;
        public InputAction @Talk => m_Wrapper.m_Player_Talk;
        public InputAction @Mute => m_Wrapper.m_Player_Mute;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Fire => m_Wrapper.m_Player_Fire;
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
                @Talk.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
                @Talk.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
                @Talk.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTalk;
                @Mute.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMute;
                @Mute.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMute;
                @Mute.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMute;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Fire.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFire;
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
                @Talk.started += instance.OnTalk;
                @Talk.performed += instance.OnTalk;
                @Talk.canceled += instance.OnTalk;
                @Mute.started += instance.OnMute;
                @Mute.performed += instance.OnMute;
                @Mute.canceled += instance.OnMute;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
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
        void OnTalk(InputAction.CallbackContext context);
        void OnMute(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
