using System;
using UnityEngine;

namespace KesselSabacc
{
    [CreateAssetMenu( fileName = "DeckConfiguration", menuName = "Sabacc/DeckConfiguration" )]
    public class DeckConfiguration : ScriptableObject
    {
        public Sprite bloodCardBack;
        public Sprite sandCardBack;
        public DeckCardConfig sylopCards;
        public DeckCardConfig oneCards;
        public DeckCardConfig twoCards;
        public DeckCardConfig threeCards;
        public DeckCardConfig fourCards;
        public DeckCardConfig fiveCards;
        public DeckCardConfig sixCards;
        public DeckCardConfig imposterCards;
    }

    [Serializable]
    public class DeckCardConfig
    {
        public Sprite bloodFront;
        public Sprite sandFront;
        public int count;
    }
}
