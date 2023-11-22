using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class BattleContestTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  int maxrank;
  public int Maxrank { get {return maxrank; } set { this.maxrank = value;} }
  
  [SerializeField]
  int minrank;
  public int Minrank { get {return minrank; } set { this.minrank = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] winvalue = new float[0];
  public float[] Winvalue { get {return winvalue; } set { this.winvalue = value;} }
  
  [SerializeField]
  float[] losevalue = new float[0];
  public float[] Losevalue { get {return losevalue; } set { this.losevalue = value;} }
  
}