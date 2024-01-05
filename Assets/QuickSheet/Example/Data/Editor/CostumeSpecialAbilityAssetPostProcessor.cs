using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CostumeSpecialAbilityAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/CostumeSpecialAbility.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/CostumeSpecialAbility.asset";
    private static readonly string sheetName = "CostumeSpecialAbility";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            CostumeSpecialAbility data = (CostumeSpecialAbility)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(CostumeSpecialAbility));
            if (data == null) {
                data = ScriptableObject.CreateInstance<CostumeSpecialAbility> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CostumeSpecialAbilityData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<CostumeSpecialAbilityData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
