using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList {

    private float _wood;
    private float _stone;

	public ResourceList(float wood = 0, float stone = 0)
    {
        _wood = wood;
        _stone = stone;
    }

    public float GetWood()
    {
        return _wood;
    }

    public float GetStone()
    {
        return _stone;
    }
}
