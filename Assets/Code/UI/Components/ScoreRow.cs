using KesselSabacc.Gameplay;
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

		private PlayerController _playerController;

		public RectTransform rectTransform { get; private set; }
		public int score { get; set; }

		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
		}

		public void Initialize(PlayerController playerController)
		{
			_playerController = playerController;
			HideRank();
			HideRollingDiceOverlay();
			_overlay.gameObject.SetActive( false );
			SetName( playerController.Model.Name );
			SetSandCardValue( playerController.Model.GetFirstCardOfSuit( Model.CardSuit.SAND ).Value );
			SetBloodCardValue( playerController.Model.GetFirstCardOfSuit( Model.CardSuit.BLOOD ).Value );
			// _chipsCounter.Initialize();
			// _chipsInvestedCounter.Initialize();
			playerController.Model.OnDisqualified += OnPlayerDisqualified;
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
			if ( _playerController != null )
			{
				_playerController.Model.OnDisqualified += OnPlayerDisqualified;
			}
		}
	}
}
