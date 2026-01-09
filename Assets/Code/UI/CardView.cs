using System.Threading.Tasks;
using DG.Tweening;
using KesselSabacc.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
	/// <summary>
	/// Manages the presentation of a single card on the screen.
	/// </summary>
	public class CardView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		private Image _cardImage;

		private RectTransform _rectTransform;
		private bool _isFlipping = false;

		/// <summary>
		/// The card this view visualizes.
		/// </summary>
		public Card Card { get; private set; }

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		public void Start()
		{
			ShowBack();
		}


		/// <summary>
		/// Initialize the card appearance using a Card Object
		/// </summary>
		/// <param name="card"></param>
		public void Initialize(Card card)
		{
			Card = card;
			if ( card.IsFaceUp )
			{
				ShowFront();
			}
			else
			{
				ShowBack();
			}
		}

		/// <summary>
		/// Show the front of the card using a flip animation.
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public Task ShowFrontAsync(float duration = 0.15f)
		{
			if ( _isFlipping ) return Task.CompletedTask;

			_isFlipping = true;
			var sequence = DOTween.Sequence();
			var scaleDownTween = _rectTransform.DOScale( new Vector3( 0f, 1.1f, 1f ), duration );
			scaleDownTween.onComplete += () =>
			{
				_cardImage.sprite = Card.FrontSprite;
			};
			sequence.Append( scaleDownTween );
			var scaleUpTween = _rectTransform.DOScale( 1f, duration );
			sequence.Append( scaleUpTween );
			sequence.onComplete += () =>
			{
				_isFlipping = false;
				Card.IsFaceUp = true;
			};

			return sequence.AsyncWaitForCompletion();
		}

		/// <summary>
		/// Show the back of the card using a flip animation.
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public Task ShowBackAsync(float duration = 0.15f)
		{
			if ( _isFlipping ) return Task.CompletedTask;

			_isFlipping = true;
			var sequence = DOTween.Sequence();
			var scaleDownTween = _rectTransform.DOScale( new Vector3( 0f, 1.1f, 1f ), duration );
			scaleDownTween.onComplete += () =>
			{
				_cardImage.sprite = Card.BackSprite;
			};
			sequence.Append( scaleDownTween );
			var scaleUpTween = _rectTransform.DOScale( 1f, duration );
			sequence.Append( scaleUpTween );
			sequence.onComplete += () =>
			{
				_isFlipping = false;
				Card.IsFaceUp = false;
			};
			return sequence.AsyncWaitForCompletion();
		}

		/// <summary>
		/// Show the front of the card without any flip animation.
		/// </summary>
		public void ShowFront()
		{
			_cardImage.sprite = Card.FrontSprite;
			Card.IsFaceUp = true;
		}

		/// <summary>
		/// Show the back of the card without any flip animation.
		/// </summary>
		public void ShowBack()
		{
			_cardImage.sprite = Card.BackSprite;
			Card.IsFaceUp = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if ( Card.IsFaceUp )
			{
				ShowBackAsync();
			}
			else
			{
				ShowFrontAsync();
			}
		}
	}

}
