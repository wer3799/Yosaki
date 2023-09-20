using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class SeasonKillCountIndicator2 : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCountText;

    [SerializeField] private string key;
    
    private CompositeDisposable disposable = new CompositeDisposable();

    void Start()
    {
        Subscribe();
    }

    public void ChangeKey(string _key)
    {
        
        key = _key;
        Subscribe();
    }

    private void Subscribe()
    {
        
        disposable.Clear();
        ServerData.userInfoTable_2.GetTableData(key).AsObservable().Subscribe(e =>
        {
            killCountText.SetText($"처치 : {Utils.ConvertBigNum(e)}");
        }).AddTo(disposable);
    }
}
