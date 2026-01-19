using System;
using System.Collections;
using KesselSabacc.Gameplay;
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
		private DieImage[] _dice;
		[SerializeField]
		private Button _rollButton;

		/// <summary>
		/// Event invoked when a die value is selected;
		/// </summary>
		public event Action<int> OnDieResult;

		protected override void SubscribeToEvents()
		{
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.OnClick += SelectDieValue;
			}
			_rollButton.onClick.AddListener( HandleRollButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.OnClick -= SelectDieValue;
			}
			_rollButton.onClick.RemoveListener( HandleRollButtonClicked );
		}

		public override void Show()
		{
			base.Show();
			Reset();
		}

		public void Reset()
		{
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.Reset();
			}
			_rollButton.gameObject.SetActive( true );
		}

		public void SelectDieValue(int value)
		{
			OnDieResult?.Invoke( value );
		}

		private void HandleRollButtonClicked()
		{
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.RollDie();
			}
			_rollButton.gameObject.SetActive( false );
		}
	}
}
