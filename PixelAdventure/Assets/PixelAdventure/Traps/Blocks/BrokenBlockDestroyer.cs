using PixelAdventure.API;
using System.Collections;
using UnityEngine;

namespace PixelAdventure.Traps.Blocks
{
    public class BrokenBlockDestroyer : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other) => StartCoroutine(DestroyCoroutine());

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(0.6f);

            GetComponent<Animator>().SetTrigger(AnimatorHashes.HIT);

            yield return new WaitForSeconds(0.3f);

            GetComponentInParent<ChildCounter>().Decrease();

            Destroy(this.gameObject);

            yield return 0;
        }
    }
}
