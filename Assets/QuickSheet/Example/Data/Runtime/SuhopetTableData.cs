using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SuhopetTableData
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
  double[] rewardcut = new double[0];
  public double[] Rewardcut { get {return rewardcut; } set { this.rewardcut = value;} }
  
  [SerializeField]
  string[] cutstring = new string[0];
  public string[] Cutstring { get {return cutstring; } set { this.cutstring = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
  [SerializeField]
  TimeType timetype;
  public TimeType TIMETYPE { get {return timetype; } set { this.timetype = value;} }
  
  [SerializeField]
  int sweeeptype;
  public int Sweeeptype { get {return sweeeptype; } set { this.sweeeptype = value;} }
  
  [SerializeField]
  float sweepvalue;
  public float Sweepvalue { get {return sweepvalue; } set { this.sweepvalue = value;} }
  
  [SerializeField]
  int abiltype;
  public int Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float[] abilvalue = new float[0];
  public float[] Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
  [SerializeField]
  int maxlevel;
  public int Maxlevel { get {return maxlevel; } set { this.maxlevel = value;} }
  
  [SerializeField]
  int requireitemtype;
  public int Requireitemtype { get {return requireitemtype; } set { this.requireitemtype = value;} }
  
  [SerializeField]
  int[] requirevalue = new int[0];
  public int[] Requirevalue { get {return requirevalue; } set { this.requirevalue = value;} }
  
  [SerializeField]
  int awakeskillid;
  public int Awakeskillid { get {return awakeskillid; } set { this.awakeskillid = value;} }
  
  [SerializeField]
  SuhoPetType suhopettype;
  public SuhoPetType SUHOPETTYPE { get {return suhopettype; } set { this.suhopettype = value;} }
  
  [SerializeField]
  string acquiredescription;
  public string Acquiredescription { get {return acquiredescription; } set { this.acquiredescription = value;} }
  
  [SerializeField]
  string gradedescription;
  public string Gradedescription { get {return gradedescription; } set { this.gradedescription = value;} }
  
  [SerializeField]
  int grade;
  public int Grade { get {return grade; } set { this.grade = value;} }
  
}