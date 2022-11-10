using System.Collections;
using System.Collections.Generic;
using NoodleLand.Inventory;
using UnityEngine;

public class Diluter : ContainerEntity
{
    public float kappa;


    protected override void Awake()
    {
        base.Awake();


        StartCoroutine(Doer());

    }

    IEnumerator Doer()
    {
        while (true)
        {
            InventorySlot s0 = container.Slots[0];
            if (!s0.IsEmpty() && s0.StackableItem.BaseItemData.ItemTag == "Log")
            {
                kappa += 1000;
                s0.AddToStack(-1);
            }
            yield return new WaitForSeconds(1f);

        }
    }
}
