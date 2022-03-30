using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDictionary : MonoBehaviour
{
    public static SelectedDictionary Instance;
    void Awake() => Instance = this;


    public Dictionary<int, Worker> SelectedTable = new Dictionary<int, Worker>();

    public void AddSelected(Worker worker)
    {
        int id = worker.GetInstanceID();

        if (!(SelectedTable.ContainsKey(id)))
        {
            SelectedTable.Add(id, worker);
            worker.gameObject.AddComponent<SelectionComponent>();
        }
    }

    public void Deselect(int id)
    {
        Destroy(SelectedTable[id].GetComponent<SelectionComponent>());
        SelectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach(KeyValuePair<int,Worker> pair in SelectedTable)
        {
            if(pair.Value != null)
            {
                Destroy(SelectedTable[pair.Key].GetComponent<SelectionComponent>());
            }
        }
        SelectedTable.Clear();
    }
}
