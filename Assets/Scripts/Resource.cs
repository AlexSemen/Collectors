using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool Busy {  get; private set; }

    public void TrueBusy()
    {
        Busy = true;
    }
}
