using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public struct TableID 
{

    public static string ITEMS = "item";
    
    public static string ACTIVITY = "activity";

   // public static string BASE="base";

    //add other table id here....

    public static string[] ids;

    public static void Init()
    {
        List<string> list=new List<string>();
        foreach (FieldInfo field in typeof(TableID).GetFields())
        {
            try
            {
                if(field.Name!="ids")
                {
                   // Debug.Log("field: "+(field.GetValue(null) as string));
                    list.Add(field.GetValue(null) as string);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
        ids=list.ToArray();
    }

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
