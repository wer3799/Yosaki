using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUltiSkillEffect : SingletonMono<UiUltiSkillEffect>
{
    [SerializeField]
    private List<GameObject> ultSkillEffect;
    
    public void ShowUltSkillEffect(int idx)
    {
        if (SettingData.showVisionSkill.Value == 0 ) return;
        
        if (PlayerMoveController.Instance.MoveDirection == MoveDirection.Left)
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
   
        
        if (idx == 46)
        {
            ultSkillEffect[0].SetActive(true);
        }

        if (idx == 47)
        {
            ultSkillEffect[1].SetActive(true);
        }

        if (idx == 48)
        {
            ultSkillEffect[2].SetActive(true);
        }

        if (idx == 49)
        {
            ultSkillEffect[3].SetActive(true);
        }
        
        if (idx == 55)
        {
            ultSkillEffect[4].SetActive(true);
        }  
        
        if (idx == 95)
        {
            ultSkillEffect[5].SetActive(true);
        }
        
        if (idx == 141)
        {
            ultSkillEffect[6].SetActive(true);
        }
        
                
        if (idx == 142)
        {
            ultSkillEffect[7].SetActive(true);
        }
        
        if (idx == 157)
        {
            ultSkillEffect[8].SetActive(true);
        }
        
        if (idx == 158)
        {
            ultSkillEffect[9].SetActive(true);
        }
        
        if (idx == 159)
        {
            ultSkillEffect[10].SetActive(true);
        }
        
        if (idx == 160)
        {
            ultSkillEffect[11].SetActive(true);
        }
    }

    public void OffAllUltSkillEffect()
    {
        for (var i = 0; i < ultSkillEffect.Count; i++)
        {
            ultSkillEffect[i].gameObject.SetActive(false);   
        }
    }
    

}
