using UnityEngine;

namespace Sabacc
{
    [CreateAssetMenu(fileName = "ShiftTokenData", menuName = "Scriptable Objects/ShiftTokenData")]
    public class ShiftTokenData : ScriptableObject
    {
        [field: SerializeField] public string Name;
        [TextArea(3, 5)]
        [field: SerializeField] public string Description;
        [field: SerializeField] public Sprite Sprite;
    }

}
