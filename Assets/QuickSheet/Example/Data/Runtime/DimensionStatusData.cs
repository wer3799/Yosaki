using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class DimensionStatusData
{
  [SerializeField]
  string key;
  public string Key { get {return key; } set { this.key = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  StatusWhere statuswhere;
  public StatusWhere STATUSWHERE { get {return statuswhere; } set { this.statuswhere = value;} }
  
  [SerializeField]
  bool ispercent;
  public bool Ispercent { get {return ispercent; } set { this.ispercent = value;} }
  
  [SerializeField]
  int maxlv;
  public int Maxlv { get {return maxlv; } set { this.maxlv = value;} }
  
  [SerializeField]
  float addvalue;
  public float Addvalue { get {return addvalue; } set { this.addvalue = value;} }
  
  [SerializeField]
  int statustype;
  public int Statustype { get {return statustype; } set { this.statustype = value;} }
  
  [SerializeField]
  string needstatuskey;
  public string Needstatuskey { get {return needstatuskey; } set { this.needstatuskey = value;} }
  
  [SerializeField]
  int unlocklevel;
  public int Unlocklevel { get {return unlocklevel; } set { this.unlocklevel = value;} }
  
  [SerializeField]
  int upgradeprice;
  public int Upgradeprice { get {return upgradeprice; } set { this.upgradeprice = value;} }
  
}