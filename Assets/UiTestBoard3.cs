using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UiTestBoard3 : MonoBehaviour
{
    [SerializeField] private UiGoodsIndicatorV2 prefab;

    [SerializeField] private List<Transform> transforms;

    [Header("Idle")]
    [SerializeField] private List<string> IdleItem =new List<string>();
    [Header("Normal")]
    [SerializeField] private List<string> NormalItem =new List<string>();
    [Header("Clear")]
    [SerializeField] private List<string> ClearItem =new List<string>();
    
    // Start is called before the first frame update
    void Start()
    {
        MakeCells();
    }

    private void MakeCells()
    {
        using var e = IdleItem.GetEnumerator();

        while (e.MoveNext())
        {
            var cell = Instantiate(prefab, transforms[0]);
            cell.Initialize(e.Current);
        }
        using var e2 = NormalItem.GetEnumerator();

        while (e2.MoveNext())
        {
            var cell = Instantiate(prefab, transforms[1]);
            cell.Initialize(e.Current);
        }
        using var e3 = ClearItem.GetEnumerator();

        while (e3.MoveNext())
        {
            var cell = Instantiate(prefab, transforms[2]);
            cell.Initialize(e.Current);
        }
    }
}
