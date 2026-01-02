using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KesselSabacc.UI
{
    public class LoadingScreenController : MonoBehaviour
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
