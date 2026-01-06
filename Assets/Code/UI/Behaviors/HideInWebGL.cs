using UnityEngine;

namespace KesselSabacc.UI.Behaviors
{
	/// <summary>
	/// Hides a WebGL component on start when in a WebGL build
	/// </summary>
	public class HideInWebGL : MonoBehaviour
	{
		private void Start()
		{
#if UNITY_WEBGL
			gameObject.SetActive( false );
#endif
		}
	}
}
