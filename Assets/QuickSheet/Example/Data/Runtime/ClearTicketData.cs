using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class ClearTicketData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  int itemtype;
  public int Itemtype { get {return itemtype; } set { this.itemtype = value;} }
  
  [SerializeField]
  int itemvalue;
  public int Itemvalue { get {return itemvalue; } set { this.itemvalue = value;} }
  
  [SerializeField]
  string description;
  public string Description { get {return description; } set { this.description = value;} }
  
  [SerializeField]
  int price;
  public int Price { get {return price; } set { this.price = value;} }
  
  [SerializeField]
  bool active;
  public bool Active { get {return active; } set { this.active = value;} }
  
}