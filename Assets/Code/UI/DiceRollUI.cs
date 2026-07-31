using System;
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
		private GameObject _rollButtonOverlay;
		[SerializeField]
		private Button _rollButton;

		private Action<int> _resultCallback;

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

		public void SetResultCallback(Action<int> cb)
		{
			_resultCallback = cb;
		}

		public void Reset()
		{
			_rollButtonOverlay.SetActive(true);
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.Reset();
			}
		}

		public void SelectDieValue(int value)
		{
			_resultCallback?.Invoke(value);
			_resultCallback = null;
			Hide();
		}

		private void HandleRollButtonClicked()
		{
			_rollButtonOverlay.SetActive(false);
			foreach ( DieImage dieImage in _dice )
			{
				dieImage.RollDie();
			}
		}
	}
}
