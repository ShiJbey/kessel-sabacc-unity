using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KesselSabacc.UI.Behaviors
{
	/// <summary>
	/// Scales buttons size when selected or hovered.
	/// </summary>
	public class HoverGrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private float _hoverScale = 1.05f;

		[SerializeField]
		private float _tweenDuration = 0.1f;

		private Vector3 _hoverScaleVector;
		private Vector3 _originalScaleVector;
		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
			_originalScaleVector = _rectTransform.localScale;
			_hoverScaleVector = _originalScaleVector * _hoverScale;
		}

		public void Select()
		{
			_rectTransform.DOKill();
			_rectTransform.DOScale( _hoverScaleVector, _tweenDuration );
		}

		public void Unselect()
		{
			_rectTransform.DOKill();
			_rectTransform.DOScale( _originalScaleVector, _tweenDuration );
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Select();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Unselect();
		}
	}
}
