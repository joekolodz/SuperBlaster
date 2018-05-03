using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    public Transform[] goodGuys;

    public void Update()
    {
        //select a Guy
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);//use V2 because we don't want the Z axis

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //.zero means straight down under the mousepoint

            if (hit.collider != null)
            {
                var m = hit.collider.gameObject.GetComponent<GoodGuyManager>();

                if (m == null)
                    return;
                SelectGuy(hit.collider.gameObject);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //sets the Top Left good guy as the default selected
        goodGuys[0].GetComponent<GoodGuyManager>().Select();
    }

    public void UnselectAllGuys()
    {
        foreach (var guy in goodGuys)
        {
            guy.GetComponent<GoodGuyManager>().Unselect();
        }
    }

    public void SelectGuy(GameObject guy)
    {
        UnselectAllGuys();
        guy.GetComponent<GoodGuyManager>().Select();
    }
}
