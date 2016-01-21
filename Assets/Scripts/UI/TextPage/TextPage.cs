using UnityEngine;
using System.Collections;

public class StrText
{
    public string title;
    public string content;


    public StrText (int _title,int _cont)
    {
        title=Localization.Get( _title);
        content=Localization.Get( _cont);
    }

    public StrText (int _title,string _cont)
    {
        title=Localization.Get( _title);
        content= _cont;
    }
};

public class TextPage : View
{

    public UILabel m_lblTitle;

    public UILabel m_lblContent;


    public override void Refresh(object data)
    {
        base.Refresh(data);
        StrText Data=data as StrText;
        Refresh(Data.title,Data.content);
    }


    private void Refresh(int title,int content)
    {
        Refresh(Localization.Get(title),Localization.Get(content));
    }


    private void Refresh(string title,string content)
    {
        m_lblTitle.text=title;
        m_lblContent.text=content;
    }

}
