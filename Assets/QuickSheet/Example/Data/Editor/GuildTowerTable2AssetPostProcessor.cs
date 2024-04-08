using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class GuildTowerTable2AssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/GuildTowerTable2.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/GuildTowerTable2.asset";
    private static readonly string sheetName = "GuildTowerTable2";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            GuildTowerTable2 data = (GuildTowerTable2)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(GuildTowerTable2));
            if (data == null) {
                data = ScriptableObject.CreateInstance<GuildTowerTable2> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<GuildTowerTable2Data>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<GuildTowerTable2Data>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
