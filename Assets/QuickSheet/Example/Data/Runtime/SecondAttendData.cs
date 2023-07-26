using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SecondAttendData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int unlockday;
  public int Unlockday { get {return unlockday; } set { this.unlockday = value;} }
  
  [SerializeField]
  int[] reward = new int[0];
  public int[] Reward { get {return reward; } set { this.reward = value;} }
  
  [SerializeField]
  float[] reward_value = new float[0];
  public float[] Reward_Value { get {return reward_value; } set { this.reward_value = value;} }
  
}