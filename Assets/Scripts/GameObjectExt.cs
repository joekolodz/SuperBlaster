using UnityEngine;

public static class GameObjectEx
{
    public static bool IsDebug = false;

    public static GameObject Instantiate(this Object thisGameObject, GameObject original, Transform parent, Vector3 position, Quaternion rotation, bool worldPositionStays)
    {
        var prefabInstance = GameObject.Instantiate(original);
        prefabInstance.transform.SetParent(parent, worldPositionStays);
        prefabInstance.transform.localPosition = position;
        prefabInstance.transform.rotation = rotation;
        return prefabInstance;
    }

    public static void DrawLine(this GameObject container, Vector3 point, Color color)
    {
        if (!IsDebug)
        {
            var lineToRemove = container.GetComponent<LineRenderer>();
            if (lineToRemove != null)
            {
                lineToRemove.enabled = false;
            }
            return;
        }

        var line = container.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = container.AddComponent<LineRenderer>();
        }

        if (line.enabled == false) line.enabled = true;

        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = true;
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        line.startColor = color;
        line.endColor = color;

        line.SetPosition(0, container.transform.position);
        line.SetPosition(1, point);
    }

    public static void DrawCircle(this GameObject container)
    {
        DrawCircle(container, new Color(1,0,0));
    }

    public static void DrawCircle(this GameObject container, Color color)
    {
        if (!IsDebug)
        {
            var lineToRemove = container.GetComponent<LineRenderer>();
            if (lineToRemove != null)
            {
                lineToRemove.enabled = false;
            }
            return;
        }

        float ThetaScale = 0.02f;
        float radius = 2f;
        int Size;
        float theta = 0f;


        var line = container.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = container.AddComponent<LineRenderer>();
        }

        if (line.enabled == false) line.enabled = true;

        Size = (int)((1f / ThetaScale) + 1f);

        line.positionCount = Size;
        line.startWidth = 0.2f;
        line.endWidth = 0.2f;
        line.useWorldSpace = false;
        line.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        line.startColor = color;
        line.endColor = color;

        for (int i = 0; i < Size; i++)
        {
            theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            line.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}