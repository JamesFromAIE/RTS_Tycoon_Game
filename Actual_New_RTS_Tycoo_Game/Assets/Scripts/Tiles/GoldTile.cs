using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldTile : MinableTile
{
    void Update()
    {
        if (WorkerList.Count < 1) return;
        else
        {
            MineTimer -= Time.deltaTime * WorkerList.Count;

            if (MineTimer <= 0)
            {
                MineTimer = MineMaxTimer;
                UIManager.Instance.GainGold(ResourceWorth);
            }
        }
    }
}
