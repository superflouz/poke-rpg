using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    // Update is called once per frame
    override protected void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Round(horizontal) != 0 && Mathf.Round(vertical) == 0)
        {
            controlled.Direction = new Vector2(horizontal, 0).normalized;
        }
        else if (Mathf.Round(horizontal) == 0 && Mathf.Round(vertical) != 0)
        {
            controlled.Direction = new Vector2(0, vertical).normalized;
        }
        else if (Mathf.Round(horizontal) == 0 && Mathf.Round(vertical) == 0)
        {
            controlled.Direction = Vector2.zero;
        }
    }
}
