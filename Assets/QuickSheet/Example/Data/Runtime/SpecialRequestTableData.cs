using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SpecialRequestTableData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string enddate;
  public string Enddate { get {return enddate; } set { this.enddate = value;} }
  
  [SerializeField]
  int[] specialrequestbossid = new int[0];
  public int[] Specialrequestbossid { get {return specialrequestbossid; } set { this.specialrequestbossid = value;} }
  
  [SerializeField]
  double[] specialrequestbosshp = new double[0];
  public double[] Specialrequestbosshp { get {return specialrequestbosshp; } set { this.specialrequestbosshp = value;} }
  
  [SerializeField]
  string[] stringid = new string[0];
  public string[] Stringid { get {return stringid; } set { this.stringid = value;} }
  
  [SerializeField]
  int[] rewardtype = new int[0];
  public int[] Rewardtype { get {return rewardtype; } set { this.rewardtype = value;} }
  
  [SerializeField]
  float[] rewardvalue = new float[0];
  public float[] Rewardvalue { get {return rewardvalue; } set { this.rewardvalue = value;} }
  
}