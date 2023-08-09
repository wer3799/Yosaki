using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class GuildLevelData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  int needamount;
  public int Needamount { get {return needamount; } set { this.needamount = value;} }
  
  [SerializeField]
  guildLevelType guildleveltype;
  public guildLevelType GUILDLEVELTYPE { get {return guildleveltype; } set { this.guildleveltype = value;} }
  
  [SerializeField]
  float value;
  public float Value { get {return value; } set { this.value = value;} }
  
  [SerializeField]
  string guildname;
  public string Guildname { get {return guildname; } set { this.guildname = value;} }
  
}