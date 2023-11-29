using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static List<Resource> ResourcesOnEarth {  get; private set; } = new List<Resource>();
    public bool Busy {  get; private set; }
    
    private void Awake()
    {
        ResourcesOnEarth.Add(this);
    }

    public void TrueBusy()
    {
        Busy = true;
    }

    public void RaiseFromGround()
    {
        ResourcesOnEarth.Remove(this);
    }
}
