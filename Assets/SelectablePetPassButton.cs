using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class SelectablePetPassButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] public Image _Image;
    [SerializeField] public List<Sprite> _Sprites;

    private UiPetPassPopup _passPopup;

    private int passGrade=0;
    
    //왼쪽탭
    public void Initialize(int absolutedPassId,UiPetPassPopup parentPopup)
    {
        _passPopup = parentPopup;
        
        var data = TableManager.Instance.InAppPurchase.dataArray[absolutedPassId];
        
        passGrade = int.Parse(data.Productid.Replace("petpass", ""));

        var spriteIdx = passGrade %= _Sprites.Count;
        
        _Image.sprite = _Sprites[spriteIdx];
        _textMeshProUGUI.SetText($"{data.Title}");
    }

    public void OnClickButton()
    {
        _passPopup.OnSelectPetPassButton(passGrade);
    }
}
