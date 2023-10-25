using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class MonthRewardAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/MonthReward.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/MonthReward.asset";
    private static readonly string sheetName = "MonthReward";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            MonthReward data = (MonthReward)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(MonthReward));
            if (data == null) {
                data = ScriptableObject.CreateInstance<MonthReward> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<MonthRewardData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<MonthRewardData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
