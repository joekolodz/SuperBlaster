using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    [Range(0, 10)]
    public int roundsPerBurst = 3;
    [Range(0, 1)]
    public float delayBetweenRounds = 0.07f;
    [Range(0, 10)]
    public float delayBetweenShots = 4.0f;
    [Range(0, 1)]
    public float accuracy = 0.25f;

    private System.DateTime fireTime;
    public bool isFiring = false;

    void Start()
    {
        fireTime = System.DateTime.Now.AddSeconds(delayBetweenShots + Random.Range(0.0f, 1.0f));
    }

    private void Update()
    {

        if (StateManager.isWaitingForNextLevelToStart) return;

        if (isFiring) return;

        if (GameObject.Find("MenuControl").GetComponent<MenuControl>().isPaused) return;

        if (spawnPoint == null) return;

        if (System.DateTime.Now > fireTime)
        {
            isFiring = true;

            gameObject.GetComponent<BadGuyMovement>().RotateToFaceRadar(accuracy);

            Fire();
        }
    }

    public void Fire()
    {
        StartCoroutine(FirePlasmaBurst());
    }

    private IEnumerator FirePlasmaBurst()
    {

        for (var n = 1; n <= roundsPerBurst; n++)
        {
            if (spawnPoint != null)
            {
                if (StateManager.isWaitingForNextLevelToStart)
                {
                    continue;
                }

                var p = ObjectPooler.Instance.GetPlasma();
                if (p)
                {
                    p.transform.position = spawnPoint.position;
                    p.transform.rotation = spawnPoint.rotation;
                    p.SetActive(true);
                    yield return new WaitForSeconds(delayBetweenRounds);
                }
            }
        }

        fireTime = System.DateTime.Now.AddSeconds(Random.Range(delayBetweenShots - 0.1f, delayBetweenShots + 0.1f));

        isFiring = false;

    }
}
