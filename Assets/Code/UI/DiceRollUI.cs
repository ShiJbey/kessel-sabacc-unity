using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	/// <summary>
	/// UI interface shown to players when they must roll a die to
	/// assign a value to an Imposter card.
	/// </summary>
	public class DiceRollUI : UIComponent
	{
		[SerializeField]
		private DieSprite _dieSprites;
		[SerializeField]
		private Button _rollButton;

		/// <summary>
		/// Event invoked when a die value is selected;
		/// </summary>
		public event Action<int> OnDieResult;

		[System.Serializable]
		public struct DieSprite
		{
			public int value;
			public Sprite sprite;
		}

		protected override void SubscribeToEvents()
		{
			_rollButton.onClick.AddListener( HandleRollButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_rollButton.onClick.RemoveListener( HandleRollButtonClicked );
		}

		public void SelectDieValue(int value)
		{
			OnDieResult?.Invoke( value );
		}

		private void HandleRollButtonClicked()
		{
			StartCoroutine( DiceRollCoroutine() );
		}

		private IEnumerator DiceRollCoroutine()
		{
			yield return new WaitForSeconds( 1f );
			SelectDieValue( UnityEngine.Random.Range( 1, 6 ) );
		}
	}
}
