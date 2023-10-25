using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class TaegeukSimbeopAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/06.Table/TaegeukSimbeop.xlsx";
    private static readonly string assetFilePath = "Assets/06.Table/TaegeukSimbeop.asset";
    private static readonly string sheetName = "TaegeukSimbeop";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            TaegeukSimbeop data = (TaegeukSimbeop)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(TaegeukSimbeop));
            if (data == null) {
                data = ScriptableObject.CreateInstance<TaegeukSimbeop> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<TaegeukSimbeopData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<TaegeukSimbeopData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
