using UnityEngine;

namespace PixelAdventure
{
    public class FruitAnimatorsManager : MonoBehaviour
    {
        public static FruitAnimatorsManager Instance {get; private set;}
        private void Awake() => Instance = this;

        [SerializeField] RuntimeAnimatorController[] m_Controllers = null;

        public RuntimeAnimatorController GetRandomController => m_Controllers[Random.Range(0, m_Controllers.Length - 1)]; 
    }
}
