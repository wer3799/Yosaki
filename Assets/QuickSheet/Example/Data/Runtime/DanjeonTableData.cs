using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class DanjeonTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  double score;
  public double Score { get {return score; } set { this.score = value;} }
  
  [SerializeField]
  string scoredescription;
  public string Scoredescription { get {return scoredescription; } set { this.scoredescription = value;} }
  
  [SerializeField]
  int[] abiltype = new int[0];
  public int[] Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float[] abilvalue = new float[0];
  public float[] Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
}