using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class PetAwakeTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int pettype;
  public int Pettype { get {return pettype; } set { this.pettype = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  int abiltype;
  public int Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float[] abilvalue = new float[0];
  public float[] Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
}