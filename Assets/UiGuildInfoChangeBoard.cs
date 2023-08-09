using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGuildInfoChangeBoard : MonoBehaviour
{
    [SerializeField]
    private UiGuildIconCell uiGuildIconCell;

    [SerializeField]
    private Transform cellParent;

    [SerializeField]
    private Transform cellParent_Special;

        
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        var guildIconList = CommonUiContainer.Instance.guildIcon;

        for (int i = 0; i < guildIconList.Count; i++)
        {
            Transform parent;

            if (i < 30)
            {
                parent = cellParent;
            }
            else
            {
                parent = cellParent_Special;
            }
            
            var cell = Instantiate<UiGuildIconCell>(uiGuildIconCell, parent);
            cell.Initialize(i);
        }
    }
}
