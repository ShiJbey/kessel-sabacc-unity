using System;
using System.Collections;
using System.Collections.Generic;
using KesselSabacc.Gameplay;
using KesselSabacc.Model;
using KesselSabacc.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	public class RoundEndUI : UIComponent
	{
		[Header( "References" )]
		[SerializeField]
		private Button _nextButton;
		[SerializeField]
		private GameObject _scoreRowPrefab;
		[SerializeField]
		private RectTransform _scoreRowContainer;

		[Header( "Animation Settings" )]
		private float _tweenDuration = 0.35f;
		[SerializeField] private AnimationCurve _tweenCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );

		private KesselSabaccGameController _gameController;

		private List<ScoreRow> _scoreRows = new();

		protected override void Awake()
		{
			base.Awake();
			_scoreRowPrefab.SetActive( false );
		}

		public void Initialize(KesselSabaccGameController gameController)
		{
			_gameController = gameController;
			gameController.Model.RoundResults.OnResultAdded += OnRoundResultAdded;
			gameController.Model.RoundResults.OnResultsCleared += OnRoundResultsCleared;
		}

		protected override void SubscribeToEvents()
		{
			_nextButton.onClick.AddListener( HandleNextButtonClicked );
		}

		protected override void UnsubscribeFromEvents()
		{
			_nextButton.onClick.AddListener( HandleNextButtonClicked );
		}

		private void OnRoundResultAdded(PlayerRoundResult result)
		{
			AddScore( result );
		}

		private void OnRoundResultsCleared()
		{
			ClearScores();
		}

		public void AddScore(PlayerRoundResult result)
		{
			// Create new row
			ScoreRow newRow = Instantiate( _scoreRowPrefab, _scoreRowContainer )
				.GetComponent<ScoreRow>();

			newRow.gameObject.SetActive( true );

			newRow.Initialize( result );

			_scoreRows.Add( newRow );
		}

		public async Awaitable SortRows()
		{
			_scoreRows.Sort( (a, b) => b.Result.CompareTo( a.Result ) );
			await ReorderRows();
		}

		private async Awaitable ReorderRows()
		{
			// Capture starting positions before reordering
			Dictionary<ScoreRow, Vector2> startPositions = new Dictionary<ScoreRow, Vector2>();
			foreach ( var row in _scoreRows )
			{
				startPositions[row] = row.rectTransform.anchoredPosition;
			}

			// Reorder in hierarchy to match sorted order
			for ( int i = 0; i < _scoreRows.Count; i++ )
			{
				_scoreRows[i].SetRank( i + 1 );
				_scoreRows[i].gameObject.transform.SetSiblingIndex( i );
			}

			// Force layout rebuild to get target positions
			LayoutRebuilder.ForceRebuildLayoutImmediate( _scoreRowContainer.GetComponent<RectTransform>() );
			await Awaitable.NextFrameAsync(); // Wait one frame for layout

			// Capture target positions
			Dictionary<ScoreRow, Vector2> targetPositions = new Dictionary<ScoreRow, Vector2>();
			foreach ( var row in _scoreRows )
			{
				targetPositions[row] = row.rectTransform.anchoredPosition;
				// Reset to start position for animation
				row.rectTransform.anchoredPosition = startPositions[row];
			}

			// Animate to target positions
			float elapsed = 0f;
			while ( elapsed < _tweenDuration )
			{
				elapsed += Time.deltaTime;
				float t = _tweenCurve.Evaluate( elapsed / _tweenDuration );

				foreach ( var row in _scoreRows )
				{
					Vector2 start = startPositions[row];
					Vector2 target = targetPositions[row];
					row.rectTransform.anchoredPosition = Vector2.Lerp( start, target, t );
				}

				await Awaitable.NextFrameAsync();
			}

			// Ensure final positions are exact
			foreach ( var row in _scoreRows )
			{
				row.rectTransform.anchoredPosition = targetPositions[row];
			}
		}

		public void ClearScores()
		{
			foreach ( var row in _scoreRows )
			{
				Destroy( row.gameObject );
			}
			_scoreRows.Clear();
		}

		public void ShowContinueButton()
		{
			_nextButton.gameObject.SetActive( true );
		}

		public void HideContinueButton()
		{
			_nextButton.gameObject.SetActive( false );
		}

		private void HandleNextButtonClicked()
		{
			UIFeedbackManager.Instance.PlayButtonClickSound();
			_gameController.AdvanceRound();
		}
	}
}
