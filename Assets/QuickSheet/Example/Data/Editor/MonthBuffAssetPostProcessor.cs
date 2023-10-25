using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class MonthBuffAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/MonthBuff.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/MonthBuff.asset";
    private static readonly string sheetName = "MonthBuff";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            MonthBuff data = (MonthBuff)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(MonthBuff));
            if (data == null) {
                data = ScriptableObject.CreateInstance<MonthBuff> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<MonthBuffData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<MonthBuffData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
