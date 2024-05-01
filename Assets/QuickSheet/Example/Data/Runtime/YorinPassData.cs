using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class YorinPassData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { this.id = value;} }
  
  [SerializeField]
  string productid;
  public string Productid { get {return productid; } set { this.productid = value;} }
  
  [SerializeField]
  YorinPassType yorinpasstype;
  public YorinPassType YORINPASSTYPE { get {return yorinpasstype; } set { this.yorinpasstype = value;} }
  
  [SerializeField]
  int unlockcondition;
  public int Unlockcondition { get {return unlockcondition; } set { this.unlockcondition = value;} }
  
  [SerializeField]
  int adrewardcount;
  public int Adrewardcount { get {return adrewardcount; } set { this.adrewardcount = value;} }
  
  [SerializeField]
  int masknumber;
  public int Masknumber { get {return masknumber; } set { this.masknumber = value;} }
  
}