using UnityEngine;

namespace Sabacc
{
    [CreateAssetMenu(fileName = "CardData", menuName = "Sabacc/Card Data")]
    public class CardData : ScriptableObject
    {
        [field: SerializeField] public string Name;
        [field: SerializeField] public CardValue Value;
        [field: SerializeField] public CardSuit Suit;
        [field: SerializeField] public Sprite FrontSprite;
		[field: SerializeField] public Sprite BackSprite;
    }
}
