using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class OpponentInfoPanel : UIComponent
	{
		[SerializeField]
		private RemainingChipCounter _chipsView;
		[SerializeField]
		private InvestedChipCounter _investedChipsView;
		[SerializeField]
		private Image _playerImage;
		[SerializeField]
		private TMP_Text _playerName;
		[SerializeField]
		private Animator _animator;
		[SerializeField]
		private Image _playerColorIndicator;


		private PlayerController _player;

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_player != null)
			{
				_player.Model.OnChipsChanged -= OnPlayerChipsChanged;
				_player.Model.OnChipsInvestedChanged -= OnPlayerChipsInvestedChanged;
				_player.OnTurnStarted -= OnPlayerTurnStarted;
				_player.OnTurnEnded -= OnPlayerTurnEnded;
				_player = null;
			}
		}

		public void Initialize(PlayerController player, Color color)
		{
			_player = player;
			_chipsView.SetMaxChipCount(player.Model.StartingChips);
			_chipsView.SetCurrentChipCount(player.Model.Chips);
			_investedChipsView.SetChipCount(0);
			_player.Model.OnChipsChanged += OnPlayerChipsChanged;
			_player.Model.OnChipsInvestedChanged += OnPlayerChipsInvestedChanged;
			_player.OnTurnStarted += OnPlayerTurnStarted;
			_player.OnTurnEnded += OnPlayerTurnEnded;
			SetPlayerName( player.Model.Name );
			SetPlayerColor(color);
		}

		public void SetPlayerColor(Color color)
		{
			_playerColorIndicator.color = color;
		}

		public void UpdateView(Player player)
		{
			SetPlayerName( player.Name );
		}

		public void SetPlayerName(string value)
		{
			_playerName.SetText( value );
		}

		public void SetPlayerSprite(Sprite sprite)
		{
			_playerImage.sprite = sprite;
		}

		private void OnPlayerChipsChanged(int chips)
		{
			_chipsView.SetCurrentChipCount(chips);
		}

		private void OnPlayerChipsInvestedChanged(int chips)
		{
			_investedChipsView.SetChipCount(chips);
		}

		private void OnPlayerTurnStarted()
		{
			_animator.SetBool("Focused", true);
		}

		private void OnPlayerTurnEnded()
		{
			_animator.SetBool("Focused", false);
		}
	}
}
