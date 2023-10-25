using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class SealSwordEvolutionPassAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/SealSwordEvolutionPass.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/SealSwordEvolutionPass.asset";
    private static readonly string sheetName = "SealSwordEvolutionPass";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            SealSwordEvolutionPass data = (SealSwordEvolutionPass)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(SealSwordEvolutionPass));
            if (data == null) {
                data = ScriptableObject.CreateInstance<SealSwordEvolutionPass> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<SealSwordEvolutionPassData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<SealSwordEvolutionPassData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
