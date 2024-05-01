using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTopRankerView : SingletonMono<UiTopRankerView>
{
    [SerializeField]
    private List<UiTopRankerCell> rankerCellList;
    public List<UiTopRankerCell> RankerCellList => rankerCellList;

    public void DisableAllCell()
    {
        if (rankerCellList.Count > 0)
        {
            rankerCellList.ForEach(e => e.gameObject.SetActive(false));
        }
    }
}
