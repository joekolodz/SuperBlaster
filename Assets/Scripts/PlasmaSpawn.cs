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

    private bool _isDebugging = false;

    void Start()
    {
        _isDebugging = name.Contains("(Triangle");
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
        if (_isDebugging) Debug.Log($"{System.DateTime.Now} - Firing");
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
                    if (_isDebugging) Debug.Log($"Got a plasma round");
                    yield return new WaitForSeconds(delayBetweenRounds);
                    if (_isDebugging) Debug.Log($"Back after yield");
                }
            }
        }
        if (_isDebugging) Debug.Log($"Finished Firing");

        fireTime = System.DateTime.Now.AddSeconds(Random.Range(delayBetweenShots - 0.1f, delayBetweenShots + 0.1f));
        if (_isDebugging) Debug.Log($"Next fire time: {fireTime}");

        isFiring = false;

    }
}
