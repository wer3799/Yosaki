using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class AbilAwakeTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  AbilAwakeType abilawaketype;
  public AbilAwakeType ABILAWAKETYPE { get {return abilawaketype; } set { this.abilawaketype = value;} }
  
  [SerializeField]
  int unlockclosedtraining;
  public int Unlockclosedtraining { get {return unlockclosedtraining; } set { this.unlockclosedtraining = value;} }
  
  [SerializeField]
  int unlockcontents;
  public int Unlockcontents { get {return unlockcontents; } set { this.unlockcontents = value;} }
  
  [SerializeField]
  int[] abiltype = new int[0];
  public int[] Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float[] abilvalue = new float[0];
  public float[] Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
  [SerializeField]
  int grade;
  public int Grade { get {return grade; } set { this.grade = value;} }
  
}