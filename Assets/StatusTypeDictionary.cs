#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class StatusTypeDictionary : EditorWindow
{
  [SerializeField] private int m_SelectedIndex = -1;
  private VisualElement secondPane;
  [MenuItem("Tools/StatusTypeDictionary")]
  public static void ShowMyEditor()
  {
    // This method is called when the user selects the menu item in the Editor
    EditorWindow wnd = GetWindow(typeof(StatusTypeDictionary)) as StatusTypeDictionary;
    wnd.titleContent = new GUIContent("StatusTypeDictionary");

    // Limit size of the window
    wnd.minSize = new Vector2(450, 200);
    wnd.maxSize = new Vector2(1920, 720);
  }
  public static T[] GetValues<T>()
  {
    return Enum.GetValues(typeof(T))
      .Cast<T>()
      .OrderBy(e => Convert.ToInt32(e))
      .ToArray();
  }
  public void CreateGUI()
  {
    var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Vertical);
    rootVisualElement.Add(splitView);

    var firstPane = new ListView();
    splitView.Add(firstPane);

    secondPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
    var labelElement = new Label("StatusType : 한글명 : 변수명");
    secondPane.Clear();
    secondPane.Add(labelElement);
    splitView.Add(secondPane);
    
    firstPane.selectionType = SelectionType.Single;
    firstPane.onSelectionChange += objects =>
    {
      if (objects != null && objects.Any())
      {
        m_SelectedIndex = (int)objects.First();
        Debug.Log($"Selected index: {m_SelectedIndex}");
      }
    };

    StatusType[] values = GetValues<StatusType>();
    firstPane.makeItem = () => new Label();
    firstPane.bindItem = (item, index) => { 
      int value = (int)values[index];
      string name = values[index].ToString();
      string name2 = CommonString.GetStatusName((StatusType)value);
      (item as Label).text =
        value > 0 ? value.ToString() + " : " + name2 + " : " + name + " : "  : name;
    };
    firstPane.itemsSource = values;
    firstPane.onSelectionChange += indices =>
    {
    };
    
  }

}
#endif 

