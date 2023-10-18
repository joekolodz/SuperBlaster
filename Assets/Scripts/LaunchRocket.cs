using UnityEngine;

public static class LaunchRocket
{
    private const float DEFAULT_ROCKET_FORCE = 3000.0f;

    public static GameObject Launch(RocketSpawn rocketSpawn)
    {
        return Launch(rocketSpawn, rocketSpawn.spawnPoint.position, 1.0f);
    }

    public static GameObject Launch(RocketSpawn rocketSpawn, float forceMultiplier)
    {
        return Launch(rocketSpawn, rocketSpawn.spawnPoint.position, forceMultiplier);
    }

    public static GameObject Launch(RocketSpawn rocketSpawn, Vector3 newPosition, float forceMultiplier)
    {
        var r = ObjectPooler.Instance.GetRocket();
        if (r)
        {
            r.transform.position = newPosition;
            r.transform.rotation = rocketSpawn.spawnPoint.rotation;
            r.SetActive(true);

            var force = DEFAULT_ROCKET_FORCE * forceMultiplier;
            r.GetComponent<RocketFire>().FireRocket(force);
        }

        return r;
    }
}