using System.Threading.Tasks;
using DG.Tweening;
using KesselSabacc.Model;
using UnityEngine;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Manages the presentation of a single card on the screen.
	/// </summary>
	public class CardView : MonoBehaviour
	{
		[SerializeField]
		private Sprite _frontSprite;
		[SerializeField]
		private Sprite _backSprite;
		[SerializeField]
		private float _flipAnimationTime = 0.3f;
		[SerializeField]
		private float _cardMovementSpeed = 0.3f;
		[SerializeField]
		private bool _isFaceUp = true;
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[Header( "Sounds" )]
		[SerializeField]
		private AudioClip _cardPlacedSound;

		private bool _isFlipping = false;
		private CardZone _currentZone;

		public Card Card { get; private set; }
		public Sprite Sprite => _spriteRenderer.sprite;

		public void Start()
		{
			if ( _isFaceUp )
			{
				_spriteRenderer.sprite = _frontSprite;
			}
			else
			{
				_spriteRenderer.sprite = _backSprite;
			}
		}


#if UNITY_EDITOR
		public void OnValidate()
		{
			if ( _isFaceUp )
			{
				_spriteRenderer.sprite = _frontSprite;
			}
			else
			{
				_spriteRenderer.sprite = _backSprite;
			}
		}
#endif


		/// <summary>
		/// Initialize the card appearance using a Card Object
		/// </summary>
		/// <param name="card"></param>
		public void Initialize(Card card, bool isFaceUp = false)
		{
			Card = card;
			DeckConfiguration deckConfig = NewGameManager.Instance.Data.deck;
			_frontSprite = deckConfig.GetFrontSprite(card.Suit, card.CardType);
			_backSprite = deckConfig.GetBackSprite(card.Suit);
			_isFaceUp = isFaceUp;
			_spriteRenderer.sprite = isFaceUp ? _frontSprite : _backSprite;
		}

		/// <summary>
		/// Show the front of the card using a flip animation.
		/// </summary>
		/// <returns></returns>
		public Task ShowFrontAsync()
		{
			if ( _isFaceUp ) return Task.CompletedTask;

			if ( _isFlipping ) return Task.CompletedTask;

			_isFlipping = true;
			var sequence = DOTween.Sequence();
			var scaleDownTween = transform.DOScale( new Vector3( 0f, 1.2f, 1f ), _flipAnimationTime / 2 );
			scaleDownTween.onComplete += () =>
			{
				_spriteRenderer.sprite = _frontSprite;
			};
			sequence.Append( scaleDownTween );
			var scaleUpTween = transform.DOScale( 1f, _flipAnimationTime / 2 );
			sequence.Append( scaleUpTween );
			sequence.onComplete += () =>
			{
				_isFlipping = false;
				_isFaceUp = true;
			};

			return sequence.AsyncWaitForCompletion();
		}

		/// <summary>
		/// Show the back of the card using a flip animation.
		/// </summary>
		/// <returns></returns>
		public Task ShowBackAsync()
		{
			if ( !_isFaceUp ) return Task.CompletedTask;

			if ( _isFlipping ) return Task.CompletedTask;

			_isFlipping = true;
			var sequence = DOTween.Sequence();
			var scaleDownTween = transform.DOScale( new Vector3( 0f, 1.2f, 1f ), _flipAnimationTime / 2 );
			scaleDownTween.onComplete += () =>
			{
				_spriteRenderer.sprite = _backSprite;
			};
			sequence.Append( scaleDownTween );
			var scaleUpTween = transform.DOScale( 1f, _flipAnimationTime / 2 );
			sequence.Append( scaleUpTween );
			sequence.onComplete += () =>
			{
				_isFlipping = false;
				_isFaceUp = false;
			};
			return sequence.AsyncWaitForCompletion();
		}

		public async Awaitable Flip()
		{
			if ( _isFaceUp )
			{
				await ShowBackAsync();
			}
			else
			{
				await ShowFrontAsync();
			}
		}

		public async Awaitable MoveCardToPosition(Vector3 position, Vector3 rotation)
		{
			var sequence = DOTween.Sequence();

			sequence.Append(
				transform.DOMove( position, _cardMovementSpeed ).SetEase( Ease.OutQuad )
			);

			sequence.Join(
				DOTween.Sequence()
				.Append( transform.DOScale( 1.3f, _cardMovementSpeed / 2 ) )
				.Append( transform.DOScale( 1f, _cardMovementSpeed / 2 ) )
			);

			sequence.Join(
				transform.DORotate( rotation, _cardMovementSpeed ).SetEase( Ease.OutQuad )
			);

			await sequence.AsyncWaitForCompletion();

			AudioManager.PlayOneSFX( _cardPlacedSound, Vector3.zero );
		}

		/// <summary>
		/// Show the front of the card without any flip animation.
		/// </summary>
		public void ShowFront()
		{
			_spriteRenderer.sprite = _frontSprite;
			_isFaceUp = true;
		}

		/// <summary>
		/// Show the back of the card without any flip animation.
		/// </summary>
		public void ShowBack()
		{
			_spriteRenderer.sprite = _backSprite;
			_isFaceUp = false;
		}

		/// <summary>
		/// Moves this card to a new zone
		/// </summary>
		public void MoveToZone(CardZone newZone)
		{
			_currentZone = newZone;
			CardSortingSystem.Instance.SetCardSorting( this, newZone );
		}

		/// <summary>
		/// Sets the sorting layer and order
		/// </summary>
		public void SetSortingLayer(string layerName, int orderInLayer)
		{
			_spriteRenderer.sortingLayerName = layerName;
			_spriteRenderer.sortingOrder = orderInLayer;
		}

		/// <summary>
		/// Sets only the sorting order (keeps current layer)
		/// </summary>
		public void SetSortingOrder(int orderInLayer)
		{
			_spriteRenderer.sortingOrder = orderInLayer;
		}


		public CardZone GetCurrentZone()
		{
			return _currentZone;
		}
	}

}
