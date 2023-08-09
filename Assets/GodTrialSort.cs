using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GodTrialSort : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;
    [SerializeField] private List<GameObject> _graduateText;

    [SerializeField] private SeletableTab _seletableTab;
    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
    }

    void Subscribe()
    {
        ServerData.userInfoTable_2.GetTableData(UserInfoTable_2.GodTrialGraduateIdx).AsObservable().Subscribe(
            e =>
            {
                if (e > 0)
                {
                    for (int i = 0; i < e; i++)
                    {
                        _gameObjects[(int)i].transform.SetAsLastSibling();
                        _graduateText[i].SetActive(true);
                    }
                }

                if (e >= GameBalance.darkGodGraduate)
                {
                    _seletableTab.OnSelect(0);
                }
                else
                {
                    _seletableTab.OnSelect((int)e);
                }
            }).AddTo(this);
    }
}
