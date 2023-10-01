using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monthpass23Apply : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ServerData.iapServerTable.TableDatas["monthpass23"].buyCount.Value = 1;
    }


}
