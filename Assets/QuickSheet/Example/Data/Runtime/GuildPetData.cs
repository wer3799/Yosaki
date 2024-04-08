using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class GuildPetData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int requirelevel;
  public int Requirelevel { get {return requirelevel; } set { this.requirelevel = value;} }
  
  [SerializeField]
  int rewardtype;
  public int Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float rewardvalue;
  public float Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
}