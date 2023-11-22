using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class FoxFireData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int level;
  public int Level { get {return level; } set { this.level = value;} }
  
  [SerializeField]
  int type;
  public int Type { get {return type; } set { this.type = value;} }
  
  [SerializeField]
  int condition_type;
  public int Condition_Type { get {return condition_type; } set { this.condition_type = value;} }
  
  [SerializeField]
  float conditoin_value;
  public float Conditoin_Value { get {return conditoin_value; } set { this.conditoin_value = value;} }
  
  [SerializeField]
  int abil_type;
  public int Abil_Type { get {return abil_type; } set { this.abil_type = value;} }
  
  [SerializeField]
  float abil_value;
  public float Abil_Value { get {return abil_value; } set { this.abil_value = value;} }
  
  [SerializeField]
  int reward_type;
  public int Reward_Type { get {return reward_type; } set { this.reward_type = value;} }
  
  [SerializeField]
  float reward_value;
  public float Reward_Value { get {return reward_value; } set { this.reward_value = value;} }
  
  [SerializeField]
  float retroactive_value;
  public float Retroactive_Value { get {return retroactive_value; } set { this.retroactive_value = value;} }
  
}