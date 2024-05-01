using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class TransJewelTowerData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string dayofweek;
  public string Dayofweek { get {return dayofweek; } set { this.dayofweek = value;} }
  
  [SerializeField]
  double[] rewardcut = new double[0];
  public double[] Rewardcut { get {return rewardcut; } set { this.rewardcut = value;} }
  
  [SerializeField]
  int[] unlocktranscount = new int[0];
  public int[] Unlocktranscount { get {return unlocktranscount; } set { this.unlocktranscount = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
}