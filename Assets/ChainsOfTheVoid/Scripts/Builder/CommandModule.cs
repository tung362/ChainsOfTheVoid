using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShipBlueprint
{
    //0 = null, 1 = square, 2 = half square, 3 = triangle, 4 = right triangle
    public int BlockID;
    public Vector3 Position;
    public Quaternion Rotation;
    public int ConnectedWeaponID;
}

public class CommandModule : MonoBehaviour
{
    //Should be in the same order
    public List<GameObject> Hulls;
    public ShipBlueprint Schematic;

    void Start()
    {

    }

    void Update()
    {

    }
}
