using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out Worker worker)) return;
        else worker.WorkerSelected(true);

    }

    private void OnDestroy()
    {
        if (!TryGetComponent(out Worker worker)) return;
        else worker.WorkerSelected(false);
    }
}
