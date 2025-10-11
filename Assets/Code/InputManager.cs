using UnityEngine;
using UnityEngine.InputSystem;

namespace Sabacc
{
	public class InputManager : MonoBehaviour
	{
		public static InputManager Instance { get; private set; }

		public bool MenuOpenCloseInput { get; private set; }

		private PlayerInput m_PlayerInput;

		private InputAction m_OpenCloseMenuAction;

		private InputAction m_NavigateAction;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy( gameObject );
				return;
			}

			Instance = this;
			m_PlayerInput = GetComponent<PlayerInput>();
			m_PlayerInput.SwitchCurrentActionMap( "UI" );
			m_NavigateAction = m_PlayerInput.actions["Navigate"];
			// m_OpenCloseMenuAction = m_PlayerInput.actions["MenuOpenClose"];
		}

		private void Start()
		{
			//Debug.Log( Gamepad.current.description.ToJson() );
		}

		void Update()
		{
			// MenuOpenCloseInput = m_OpenCloseMenuAction.WasPressedThisFrame();
			if ( m_NavigateAction.WasPressedThisFrame() )
			{
				//Debug.Log( Gamepad.current.displayName );
				//Vector2 direction = m_NavigateAction.ReadValue<Vector2>();
				//Debug.Log( "Navigation pressed: " + direction.ToString() );
			}
		}

		private void OnEnable()
		{
			//InputSystem.onDeviceChange += HandleDeviceChange;
		}


		private void OnDisable()
		{
			//InputSystem.onDeviceChange -= HandleDeviceChange;
		}

		private void HandleDeviceChange(InputDevice device, InputDeviceChange change)
		{
			Debug.Log( device.description.ToJson() );
			Debug.Log( device.description.ToJson() );
		}
	}
}


