using KesselSabacc.Gameplay;
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

		private Player _player;

		public RectTransform rectTransform { get; private set; }
		public int score { get; set; }

		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
		}

		public void Initialize(Player player)
		{
			_player = player;
			HideRank();
			HideRollingDiceOverlay();
			_overlay.SetActive( false );
			SetName( player.Name );
			SetSandCardValue( player.GetFirstCardOfSuit( Model.CardSuit.SAND ).Value );
			SetBloodCardValue( player.GetFirstCardOfSuit( Model.CardSuit.BLOOD ).Value );
			_chipsCounter.Initialize( player );
			_chipsInvestedCounter.Initialize( player );
			player.OnDisqualified += OnPlayerDisqualified;
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
			if ( _player != null )
			{
				_player.OnDisqualified -= OnPlayerDisqualified;
			}
		}
	}
}
