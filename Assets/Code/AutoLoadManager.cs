using System;
using System.Collections;
using UnityEngine;

namespace KesselSabacc
{
    /// <summary>
    /// Manages the loading of global resources at the start of a scene.
    /// </summary>
    public class AutoLoadManager : MonoBehaviour
    {
        public Action onReady;
        public bool isReady { get; private set; }

        [SerializeField]
        private GameObject[] m_ResourcePrefabs;

        public static AutoLoadManager Instance { get; private set; }

        private void Awake()
        {
            if ( Instance != null )
            {
                Destroy( gameObject );
                return;
            }

            Instance = this;
            DontDestroyOnLoad( gameObject );
        }

        private void Start()
        {
            StartCoroutine( InitializeResources() );
        }

        private IEnumerator InitializeResources()
        {
            foreach ( GameObject prefab in m_ResourcePrefabs )
            {
                Instantiate( prefab, this.transform );
            }
            isReady = true;
            onReady?.Invoke();
            yield return null;
        }
    }
}
