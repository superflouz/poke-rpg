using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected CharacterMovement controlled;

    protected void Start()
    {
        controlled = transform.parent.GetComponent<CharacterMovement>();
    }

    virtual protected void Update()
    {

    }
}
