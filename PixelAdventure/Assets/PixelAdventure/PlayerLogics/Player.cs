using UnityEngine;

namespace PixelAdventure.PlayerLogics
{
    public class Player : MonoBehaviour
    {
        public static Player Instance {get; private set;}
        public static readonly string TAG = "Player";

        private void Awake() => Instance = this;

        public void Die()
        {
            Debug.Log("OMG I'M DEAD!");
        }
    }
}