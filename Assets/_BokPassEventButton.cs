using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BokPassEventButton : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        
        this.gameObject.SetActive(ServerData.bokPassServerTable.AttendanceBokPassAllReceived()==false);    }
}
