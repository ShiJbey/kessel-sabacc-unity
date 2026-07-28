using KesselSabacc.Model;
using TMPro;
using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class ScoreRow : UIComponent
	{
		[SerializeField]
		private TMP_Text _rankLabel;
		[SerializeField]
		private TMP_Text _playerNameLabel;
		[SerializeField]
		private TMP_Text _sandCardValueLabel;
		[SerializeField]
		private TMP_Text _bloodCardValueLabel;
		[SerializeField]
		private RemainingChipCounter _chipsCounter;
		[SerializeField]
		private InvestedChipCounter _chipsInvestedCounter;
		[SerializeField]
		private GameObject _overlay;
		[SerializeField]
		private GameObject _rollingDiceOverlay;

		private PlayerRoundResult _result;
		private Player _player;

		public RectTransform rectTransform { get; private set; }
		public PlayerRoundResult Result => _result;

		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (_player != null)
			{
				_player.OnChipsChanged -= OnPlayerChipsChanged;
				_player.OnChipsInvestedChanged -= OnPlayerChipsInvestedChanged;
				_player.OnDisqualified -= OnPlayerDisqualified;
				_player = null;
			}
		}

		public void Initialize(PlayerRoundResult result)
		{
			_result = result;
			_player = result.Player;
			HideRank();
			HideRollingDiceOverlay();
			_overlay.SetActive( false );
			SetName( result.Player.Name );
			SetSandCardValue( result.SandCard.Value );
			SetBloodCardValue( result.BloodCard.Value );
			_chipsCounter.SetMaxChipCount(result.Player.StartingChips);
			_chipsCounter.SetCurrentChipCount(result.Player.Chips);
			_chipsInvestedCounter.SetChipCount(result.Player.ChipsInvested);
			_player.OnChipsChanged += OnPlayerChipsChanged;
			_player.OnChipsInvestedChanged += OnPlayerChipsInvestedChanged;
			_player.OnDisqualified += OnPlayerDisqualified;
		}

		public void ShowRollingDiceOverlay()
		{
			_rollingDiceOverlay.gameObject.SetActive( true );
		}

		public void HideRollingDiceOverlay()
		{
			_rollingDiceOverlay.gameObject.SetActive( false );
		}

		public void ShowRank()
		{
			_rankLabel.gameObject.SetActive( true );
		}

		public void HideRank()
		{
			_rankLabel.gameObject.SetActive( false );
		}

		public void SetRank(int value)
		{
			_rankLabel.text = value.ToString();
		}

		public void SetName(string value)
		{
			_playerNameLabel.text = value;
		}

		public void SetSandCardValue(int value)
		{
			_sandCardValueLabel.text = value.ToString();
		}

		public void SetBloodCardValue(int value)
		{
			_bloodCardValueLabel.text = value.ToString();
		}

		private void OnPlayerDisqualified()
		{
			_overlay.SetActive( true );
		}

		private void OnPlayerChipsChanged(int chips)
		{
			_chipsCounter.SetCurrentChipCount(chips);
		}

		private void OnPlayerChipsInvestedChanged(int chips)
		{
			_chipsInvestedCounter.SetChipCount(chips);
		}
	}
}
