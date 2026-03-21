using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public List<PippiController> units = new List<PippiController>();

    public void MoveTo(Vector3 target)
    {
        foreach (var unit in units)
        {
            if (unit != null)
                unit.MoveTo(target);
        }
    }
}