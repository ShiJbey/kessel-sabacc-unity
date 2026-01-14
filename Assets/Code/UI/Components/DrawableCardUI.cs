using System;
using DG.Tweening;
using KesselSabacc.Model;
using KesselSabacc.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KesselSabacc.UI.Components
{
	public class DrawableCardUI : UIComponent, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		[SerializeField]
		private GameObject _selectionArrow;
		[SerializeField]
		private GameObject _selectionHighlight;
		[SerializeField]
		private Image _cardView;
		[SerializeField]
		private float _arrowAnimationSpeed;

		private Vector3 _selectionArrowBasePos;

		public event Action OnClick;

		private void Start()
		{
			_selectionArrowBasePos = _selectionArrow.GetComponent<RectTransform>().anchoredPosition;
			_selectionArrow.SetActive( false );
			_selectionHighlight.SetActive( false );
		}

		protected override void OnDestroy()
		{
			if ( _selectionArrow )
			{
				_selectionArrow.GetComponent<RectTransform>().DOKill();
			}
			base.OnDestroy();
		}

		public void Initialize(Sprite sprite)
		{
			_cardView.sprite = sprite;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_selectionArrow.SetActive( true );
			_selectionHighlight.SetActive( true );

			Sequence hoverSequence = DOTween.Sequence();

			// Add upward movement
			hoverSequence.Append(
				_selectionArrow.GetComponent<RectTransform>()
					.DOAnchorPosY( _selectionArrowBasePos.y + 12, _arrowAnimationSpeed / 2 )
					.SetEase( Ease.OutSine )
			);

			// Add downward movement
			hoverSequence.Append(
				_selectionArrow.GetComponent<RectTransform>()
					.DOAnchorPosY( _selectionArrowBasePos.y, _arrowAnimationSpeed / 2 )
					.SetEase( Ease.OutSine )
			);

			// Loop the sequence indefinitely
			hoverSequence.SetLoops( -1 );
			hoverSequence.SetLink( _selectionArrow );
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_selectionArrow.GetComponent<RectTransform>().DOKill();
			_selectionArrow.GetComponent<RectTransform>().anchoredPosition = _selectionArrowBasePos;
			_selectionArrow.SetActive( false );
			_selectionHighlight.SetActive( false );
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			_selectionArrow.GetComponent<RectTransform>().DOKill();
			OnClick?.Invoke();
		}
	}
}
