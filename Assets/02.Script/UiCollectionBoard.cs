using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiCollectionBoard : MonoBehaviour
{
    [SerializeField]
    private UiCollectionCell cellPrefab;

    [SerializeField]
    private Transform cellParent;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
     //   UiManagerDescription.Instance.SetManagerDescription(ManagerDescriptionType.monsterCollection);
    }

    private void Initialize()
    {
        using var e = TableManager.Instance.EnemyData.GetEnumerator();
        //메터리얼인덱스
        int enemyIdx = 0;
        while (e.MoveNext())
        {
            if (e.Current.Value.Usecollection == false) continue;

            var cell = Instantiate<UiCollectionCell>(cellPrefab, cellParent);
            cell.Initialize(e.Current.Value, enemyIdx);
            enemyIdx++;
        }
    }
}
