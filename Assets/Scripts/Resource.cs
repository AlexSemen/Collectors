using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public static List<Resource> ResourcesOnEarth {  get; private set; }
    public bool Busy {  get; private set; }

    static Resource()
    {
        ResourcesOnEarth = new List<Resource>();
    }

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
