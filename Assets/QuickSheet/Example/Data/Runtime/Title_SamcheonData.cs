using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class Title_SamcheonData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string stringid;
  public string Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  int tabtype;
  public int Tabtype { get {return tabtype; } set { this.tabtype = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  int abiltype;
  public int Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float abilvalue;
  public float Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
  [SerializeField]
  string lockmaskdescription;
  public string Lockmaskdescription { get {return lockmaskdescription; } set { this.lockmaskdescription = value;} }
  
  [SerializeField]
  int[] bossid = new int[0];
  public int[] Bossid { get {return bossid; } set { this.bossid = value;} }
  
}