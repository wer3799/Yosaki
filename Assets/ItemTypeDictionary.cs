#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class ItemTypeDictionary : EditorWindow
{
  [SerializeField] private int m_SelectedIndex = -1;
  private VisualElement secondPane;
  [MenuItem("Tools/ItemTypeDictionary")]
  public static void ShowMyEditor()
  {
    // This method is called when the user selects the menu item in the Editor
    EditorWindow wnd = GetWindow(typeof(ItemTypeDictionary)) as ItemTypeDictionary;
    wnd.titleContent = new GUIContent("ItemTypeDictionary");

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
    var labelElement = new Label("ItemType : 한글명 : 변수명 : 뒤끝서버변수명\n테이블 참고하는 경우 미등록으로 표기(에디터 실행 후 플레이어가 등장후 실행시키면 뜰수도있음.{테이블 로드 완료시})");
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

    Item_Type[] values = GetValues<Item_Type>();
    firstPane.makeItem = () => new Label();
    firstPane.bindItem = (item, index) => { 
      int value = (int)values[index];
      string name = values[index].ToString();
      string name2 = CommonString.GetItemName((Item_Type)value);
      string name3 = ServerData.goodsTable.ItemTypeToServerString((Item_Type)value);
      (item as Label).text =
        value > 0 ? value.ToString() + " : " + name2 + " : " + name + " : " + name3 : name + " : " + name3;
    };
    firstPane.itemsSource = values;
    firstPane.onSelectionChange += indices =>
    {
      var selectedIndices = indices.Cast<int>().ToList(); // IEnumerable<object>를 List<int>로 변환
      if (selectedIndices.Count > 0)
      {
          // 인덱스를 사용하여 선택된 ItemType을 가져옴
          var selectedItemType = values[selectedIndices[0]];
          
          // 선택된 ItemType에 대한 이미지를 가져와서 오른쪽 패널에 추가
      
          var itemName = CommonString.GetItemName(selectedItemType);
          if (itemName != null)
          {

          }
        }
     };
    // var labelElement = new Label("ItemType : 한글명 : 변수이름");
    //
    // secondPane.Clear();
    // secondPane.Add(labelElement);
    
  }

}
#endif
