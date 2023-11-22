using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class MeditationData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int[] abiltype = new int[0];
  public int[] Abiltype { get {return abiltype; } set { this.abiltype = value;} }
  
  [SerializeField]
  float[] abilvalue = new float[0];
  public float[] Abilvalue { get {return abilvalue; } set { this.abilvalue = value;} }
  
  [SerializeField]
  int consume;
  public int Consume { get {return consume; } set { this.consume = value;} }
  
  [SerializeField]
  float retroactive_value;
  public float Retroactive_Value { get {return retroactive_value; } set { this.retroactive_value = value;} }
  
}