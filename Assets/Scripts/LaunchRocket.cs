using UnityEngine;

public class LaunchRocket : MonoBehaviour
{
    public static readonly LaunchRocket Instance = (new GameObject("LaunchRocketSingletonContainer")).AddComponent<LaunchRocket>();

    private const float DEFAULT_ROCKET_FORCE = 3000.0f;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static LaunchRocket()
    {
    }

    private LaunchRocket()
    {
    }

    public GameObject Launch(RocketSpawn rocketSpawn)
    {
        var r = Launch(rocketSpawn, rocketSpawn.spawnPoint.position, 1.0f);
        return r;
    }

    public GameObject Launch(RocketSpawn rocketSpawn, float forceMultiplier)
    {
        var r = Launch(rocketSpawn, rocketSpawn.spawnPoint.position, forceMultiplier);
        return r;
    }

    public GameObject Launch(RocketSpawn rocketSpawn, Vector3 newPosition)
    {
        var r = Launch(rocketSpawn, newPosition, 1.0f);
        return r;
    }

    public GameObject Launch(RocketSpawn rocketSpawn, Vector3 newPosition, float forceMultiplier)
    {
        var r = ObjectPooler.Instance.GetRocket();
        if (r)
        {
            r.transform.position = newPosition;
            r.transform.rotation = rocketSpawn.spawnPoint.rotation;
            r.SetActive(true);

            var force = DEFAULT_ROCKET_FORCE * forceMultiplier;
            r.GetComponent<RocketFire>().FireRocket(force);
            
            //r.GetComponent<Rigidbody2D>().AddForce(r.transform.right * force);
        }

        return r;
    }
}