using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float Timeout = 1.0f;

        private void OnParticleSystemStopped()
        {
            ObjectPooler.Instance.ReturnExplosionToPool(gameObject);
        }

        private void OnEnable()
        {
            StartCoroutine(ReturnAfterTimeOut());
        }

        private IEnumerator ReturnAfterTimeOut()
        {
            var elapsed = 0.0f;
            while (elapsed < Timeout)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            ObjectPooler.Instance.ReturnExplosionToPool(gameObject);
        }
    }
}
