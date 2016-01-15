using UnityEngine;
using System.Collections;

public struct TableID 
{

    public static string ITEMS = "item";
    
    public static string ACTIVITY = "activity";

    //add other table id here....





    public static string[] ids=new string[]
    {
        ITEMS,
        ACTIVITY,
        //dd other table id here....
    };




    private string V;
    
    public TableID(string aa) { V = aa; }
    
    
    public static implicit operator string(TableID id)
    {
        return (id.ToString());
    }
    
    public static implicit operator TableID(string id)
    {
        return (new TableID(id));
    }
    
    public override string ToString()
    {
        return (V);
    }
}
