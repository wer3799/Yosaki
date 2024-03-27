using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class TwelveBossTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string stringid;
  public string Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  string title;
  public string Title { get {return title; } set { this.title = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  double hp;
  public double Hp { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  float rewardminhp;
  public float Rewardminhp { get {return rewardminhp; } set { this.rewardminhp = value;} }
  
  [SerializeField]
  double rewardmaxhp;
  public double Rewardmaxhp { get {return rewardmaxhp; } set { this.rewardmaxhp = value;} }
  
  [SerializeField]
  float attackpowermin;
  public float Attackpowermin { get {return attackpowermin; } set { this.attackpowermin = value;} }
  
  [SerializeField]
  float attackpowermax;
  public float Attackpowermax { get {return attackpowermax; } set { this.attackpowermax = value;} }
  
  [SerializeField]
  bool islock;
  public bool Islock { get {return islock; } set { this.islock = value;} }
  
  [SerializeField]
  float defense;
  public float Defense { get {return defense; } set { this.defense = value;} }
  
  [SerializeField]
  double[] rewardcut = new double[0];
  public double[] Rewardcut { get {return rewardcut; } set { this.rewardcut = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  string[] cutstring = new string[0];
  public string[] Cutstring { get {return cutstring; } set { this.cutstring = value;} }
  
  [SerializeField]
  TimeType timetype;
  public TimeType TIMETYPE { get {return timetype; } set { this.timetype = value;} }
  
  [SerializeField]
  NotImmuneType notimmunetype;
  public NotImmuneType NOTIMMUNETYPE { get {return notimmunetype; } set { this.notimmunetype = value;} }
  
  [SerializeField]
  int[] displayskeletondata = new int[0];
  public int[] Displayskeletondata { get {return displayskeletondata; } set { this.displayskeletondata = value;} }
  
  [SerializeField]
  BossType bosstype;
  public BossType BOSSTYPE { get {return bosstype; } set { this.bosstype = value;} }
  
  [SerializeField]
  RewardColor rewardcolor;
  public RewardColor REWARDCOLOR { get {return rewardcolor; } set { this.rewardcolor = value;} }
  
  [SerializeField]
  bool skipboss;
  public bool Skipboss { get {return skipboss; } set { this.skipboss = value;} }
  
}