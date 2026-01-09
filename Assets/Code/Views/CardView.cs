using System.Threading.Tasks;
using System.Xml.Serialization;
using DG.Tweening;
using KesselSabacc.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KesselSabacc.Views
{
	/// <summary>
	/// Manages the presentation of a single card on the screen.
	/// </summary>
	public class CardView : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField]
		private Sprite _frontSprite;
		[SerializeField]
		private Sprite _backSprite;
		[SerializeField]
		private float _flipAnimationTime = 0.3f;
		[SerializeField]
		private bool _isFaceUp = true;
		[SerializeField]
		private SpriteRenderer _spriteRenderer;
		private bool _isFlipping = false;

		public Card Card { get; private set; }

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
		public void Initialize(Card card)
		{
			Card = card;
			_frontSprite = card.FrontSprite;
			_backSprite = card.BackSprite;
			_isFaceUp = card.IsFaceUp;
			_spriteRenderer.sprite = card.VisibleSprite;
		}

		/// <summary>
		/// Show the front of the card using a flip animation.
		/// </summary>
		/// <returns></returns>
		public Task ShowFrontAsync()
		{
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

		private void OnMouseUp()
		{
			OnMouseUpAsButton();
		}

		private void OnMouseOver()
		{
			Debug.Log( "Mouse is hovering" );
		}

		private void OnMouseDown()
		{
			Debug.Log( "Mouse is down" );
		}

		private void OnMouseUpAsButton()
		{
			Debug.Log( "Card clicked" );
			if ( _isFaceUp )
			{
				ShowBackAsync();
			}
			else
			{
				ShowFrontAsync();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnMouseUpAsButton();
		}
	}

}
