using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KesselSabacc.UI.Behaviors
{
	public class HandItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private float animationDuration = 0.1f;

		private Vector3 _originalPosition;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			_originalPosition = _rectTransform.anchoredPosition;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			var _hoverPosition = _originalPosition + _rectTransform.up.normalized * 24f;
			_rectTransform.DOKill();
			_rectTransform.DOAnchorPos( _hoverPosition, animationDuration );
			_rectTransform.DOScale( 1.01f, animationDuration );
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_rectTransform.DOKill();
			_rectTransform.DOAnchorPos( _originalPosition, animationDuration );
			_rectTransform.DOScale( 1f, animationDuration );
		}
	}
}
