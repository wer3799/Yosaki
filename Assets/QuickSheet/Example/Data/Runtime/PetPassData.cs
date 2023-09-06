using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class PetPassData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  double unlockamount;
  public double Unlockamount { get {return unlockamount; } set { this.unlockamount = value;} }
  
  [SerializeField]
  int reward1;
  public int Reward1 { get {return reward1; } set { this.reward1 = value;} }
  
  [SerializeField]
  float reward1_value;
  public float Reward1_Value { get {return reward1_value; } set { this.reward1_value = value;} }
  
  [SerializeField]
  int reward2;
  public int Reward2 { get {return reward2; } set { this.reward2 = value;} }
  
  [SerializeField]
  float reward2_value;
  public float Reward2_Value { get {return reward2_value; } set { this.reward2_value = value;} }
  
  [SerializeField]
  string shopid;
  public string Shopid { get {return shopid; } set { this.shopid = value;} }
  
  [SerializeField]
  string reward1_key;
  public string Reward1_Key { get {return reward1_key; } set { this.reward1_key = value;} }
  
  [SerializeField]
  string reward2_key;
  public string Reward2_Key { get {return reward2_key; } set { this.reward2_key = value;} }
  
  [SerializeField]
  RewardItemType rewarditemtype;
  public RewardItemType REWARDITEMTYPE { get {return rewarditemtype; } set { this.rewarditemtype = value;} }
  
}