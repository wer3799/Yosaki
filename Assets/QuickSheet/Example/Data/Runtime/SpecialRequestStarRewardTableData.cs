using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SpecialRequestStarRewardTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int starcondition;
  public int Starcondition { get {return starcondition; } set { this.starcondition = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  int startseasonid;
  public int Startseasonid { get {return startseasonid; } set { this.startseasonid = value;} }
  
  [SerializeField]
  int endseasonid;
  public int Endseasonid { get {return endseasonid; } set { this.endseasonid = value;} }
  
}