using TMPro;
using UnityEngine;

namespace KesselSabacc.UI
{
    public class VersionText : MonoBehaviour
    {
        private TMP_Text m_Text;

        private void Awake()
        {
            m_Text = GetComponent<TMP_Text>();
        }

        void Start()
        {
            m_Text.text = Application.version;
        }
    }
}
