using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI.Screens
{
	public class LoadingScreen : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text loadingText;

		[SerializeField]
		private Image backgroundImage;

		public string text
		{
			get
			{
				return loadingText.text;
			}
			set
			{
				loadingText.text = value;
			}
		}

		public Sprite background
		{
			get => backgroundImage.sprite;
			set => backgroundImage.sprite = value;
		}

		public void Awake()
		{
			Hide();
		}

		public void Show()
		{
			gameObject.SetActive( true );
		}

		public void Hide()
		{
			gameObject.SetActive( false );
		}
	}
}
