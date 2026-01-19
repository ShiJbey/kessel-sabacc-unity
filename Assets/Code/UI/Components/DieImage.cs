using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.Gameplay
{
	public class DieImage : MonoBehaviour
	{
		[SerializeField]
		private DieSprite[] _sprites;
		[SerializeField]
		private Image _image;
		[SerializeField]
		private Button _button;
		[SerializeField]
		private float _diceRollTime;
		[SerializeField]
		private float _spriteChangeDelay;

		private Coroutine _rollCoroutine;
		private int _dieValue;

		public bool IsRolling { get; private set; }

		private Dictionary<int, DieSprite> _spriteCache = new();

		public event Action<int> OnClick;

		private void Awake()
		{
			foreach ( DieSprite entry in _sprites )
			{
				_spriteCache.Add( entry.value, entry );
			}

			Reset();
		}

		private void OnEnable()
		{
			_button.onClick.AddListener( HandleDieClicked );
		}

		private void OnDisable()
		{
			_button.onClick.RemoveListener( HandleDieClicked );
		}

		public void Reset()
		{
			SetSprite( 1 );
		}

		public void SetInteractable(bool value)
		{
			_button.interactable = value;
		}

		public void SetSprite(int value)
		{
			if ( _spriteCache.TryGetValue( value, out var dieSprite ) )
			{
				_dieValue = value;
				_image.sprite = dieSprite.sprite;
			}
		}

		public void RollDie(Action<int> callback = null)
		{
			if ( _rollCoroutine != null )
			{
				StopCoroutine( _rollCoroutine );
			}
			_rollCoroutine = StartCoroutine( RollDieCoroutine( callback ) );
		}

		private IEnumerator RollDieCoroutine(Action<int> callback = null)
		{
			IsRolling = true;

			for ( int i = 0; i < 16; i++ )
			{
				SetSprite( UnityEngine.Random.Range( 1, 6 ) );
				yield return new WaitForSeconds( _spriteChangeDelay );
			}

			IsRolling = false;

			callback?.Invoke( _dieValue );
		}

		private void HandleDieClicked()
		{
			OnClick?.Invoke( _dieValue );
		}

		[System.Serializable]
		public struct DieSprite
		{
			public int value;
			public Sprite sprite;
		}
	}
}
