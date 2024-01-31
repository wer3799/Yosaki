using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class BingoEventData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  BingoEventRewardType bingoeventrewardtype;
  public BingoEventRewardType BINGOEVENTREWARDTYPE { get {return bingoeventrewardtype; } set { this.bingoeventrewardtype = value;} }
  
  [SerializeField]
  int itemtype;
  public int Itemtype { get {return itemtype; } set { this.itemtype = value;} }
  
  [SerializeField]
  float itemvalue;
  public float Itemvalue { get {return itemvalue; } set { this.itemvalue = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  int grade;
  public int Grade { get {return grade; } set { this.grade = value;} }
  
}