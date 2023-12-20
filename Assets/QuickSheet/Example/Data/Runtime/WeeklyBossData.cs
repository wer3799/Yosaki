using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class WeeklyBossData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string materialtype;
  public string Materialtype { get {return materialtype; } set { this.materialtype = value;} }
  
  [SerializeField]
  double hp;
  public double Hp { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  float defense;
  public float Defense { get {return defense; } set { this.defense = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  int[] rewardgrade = new int[0];
  public int[] Rewardgrade { get {return rewardgrade; } set { this.rewardgrade = value;} }
  
}