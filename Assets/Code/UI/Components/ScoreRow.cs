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
		private ChipCounter _chipsCounter;
		[SerializeField]
		private ChipCounter _chipsInvestedCounter;
		[SerializeField]
		private GameObject _overlay;
		[SerializeField]
		private GameObject _rollingDiceOverlay;

		private PlayerRoundResult _result;

		public RectTransform rectTransform { get; private set; }
		public PlayerRoundResult Result => _result;

		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
		}

		public void Initialize(PlayerRoundResult result)
		{
			_result = result;
			HideRank();
			HideRollingDiceOverlay();
			_overlay.SetActive( false );
			SetName( result.Player.Name );
			SetSandCardValue( result.SandCard.Value );
			SetBloodCardValue( result.BloodCard.Value );
			_chipsCounter.Initialize( result.Player );
			_chipsInvestedCounter.Initialize( result.Player );
			result.Player.OnDisqualified += OnPlayerDisqualified;
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

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if ( _result != null && _result.Player != null )
			{
				_result.Player.OnDisqualified -= OnPlayerDisqualified;
			}
		}
	}
}
