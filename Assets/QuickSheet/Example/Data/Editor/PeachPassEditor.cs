using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(PeachPass))]
public class PeachPassEditor : BaseExcelEditor<PeachPass>
{	    
    public override bool Load()
    {
        PeachPass targetData = target as PeachPass;

        string path = targetData.SheetName;
        if (!File.Exists(path))
            return false;

        string sheet = targetData.WorksheetName;

        ExcelQuery query = new ExcelQuery(path, sheet);
        if (query != null && query.IsValid())
        {
            targetData.dataArray = query.Deserialize<PeachPassData>().ToArray();
            EditorUtility.SetDirty(targetData);
            AssetDatabase.SaveAssets();
            return true;
        }
        else
            return false;
    }
}
