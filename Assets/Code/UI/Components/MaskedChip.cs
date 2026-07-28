using UnityEngine;

namespace KesselSabacc.UI.Components
{
	public class MaskedChip : MonoBehaviour
	{
		[SerializeField]
		private GameObject _overlayMask;

		public void ShowOverlay()
		{
			_overlayMask.SetActive(true);
		}

		public void HideOverlay()
		{
			_overlayMask.SetActive(false);
		}
	}
}
