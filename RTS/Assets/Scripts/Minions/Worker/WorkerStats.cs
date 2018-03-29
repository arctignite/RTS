using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerStats : MonoBehaviour {

    [SerializeField] private float wood;
    [SerializeField] private float stone;
    [SerializeField] private string name;
    private int minionType;

    [SerializeField] private float movementSpeed = 2;
    [SerializeField] private float productivity = 1;

    private WorkerAnimationManager anim;

    private void Start()
    {
        anim = gameObject.GetComponent<WorkerAnimationManager>();
    }

    public void AddItemsToInventory(ResourceList _resources)
    {
        SetWood(_resources.GetWood());
        SetStone(_resources.GetStone());
    }

    public void ClearInventory()
    {
        SetWood(0);
        SetStone(0);
    }

    public void SetWood(float i)
    {
        wood = i;

        if (wood > 0)
        {
            anim.EquipWood();
        }
        else if (wood <= 0 && stone <= 0)      
        {
            anim.NoLoad();
        }
    }

    public float GetWood()
    {
        return wood;
    }

    public void SetStone(float i)
    {
        stone = i;

        if (stone > 0)
        {
            anim.EquipBag();
        }
        else if (wood <= 0 && stone <= 0)
        {
            anim.NoLoad();
        }
    }

    public float GetStone()
    {
        return stone;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public void SetMovementSpeed(float _speed)
    {
        movementSpeed = _speed;
    }

    public float GetProductivity()
    {
        return productivity;
    }

    public void SetProductivity(float _productivity)
    {
        productivity = _productivity;
    }

    public string GetName()
    {
        return name;
    }

    public void SetMinionType(int _minionType)
    {
        minionType = _minionType;
    }
}
