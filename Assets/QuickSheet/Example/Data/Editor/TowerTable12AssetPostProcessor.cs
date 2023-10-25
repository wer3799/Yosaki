using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class TowerTable12AssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/TowerTable12.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/TowerTable12.asset";
    private static readonly string sheetName = "TowerTable12";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            TowerTable12 data = (TowerTable12)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(TowerTable12));
            if (data == null) {
                data = ScriptableObject.CreateInstance<TowerTable12> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<TowerTable12Data>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<TowerTable12Data>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
