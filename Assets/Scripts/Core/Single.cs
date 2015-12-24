//==================================================================================
/// 单件类，非MonoBehaviour类型继承（线程安全）
/// @单件实例必须在代码中显式创建, GetInstance()不创建实例
/// @huailiang.peng
/// @2015.11.30
//==================================================================================

//----------------------------------------------------------------------------
/// 非MonoBehaviour类型的单件辅助基类，利用C#的语法性质简化单件类的定义和使用
/// @T : 单件子类型
//----------------------------------------------------------------------------
public class Single<T> where T : class, new()
{
    //单件子类实例
    private static T s_instance;

    public static T Instance
    {
        get { return GetInstance(); }
    }

    protected Single()
    {

    }

    //--------------------------------------
    /// 创建单件实例
    //--------------------------------------
    public static void CreateInstance()
    {
        if (s_instance == null)
        {
            s_instance = new T();

            (s_instance as Single<T>).Init();
        }
    }

    //--------------------------------------
    /// 删除单件实例
    //--------------------------------------
    public static void DestroyInstance()
    {
        if (s_instance != null)
        {
            (s_instance as Single<T>).UnInit();
            s_instance = null;
        }
    }

    //--------------------------------------
    /// 返回单件实例
    //-------------------------------------- 
    public static T GetInstance()
    {
        if (s_instance == null)
        {
            CreateInstance();
        }

        return s_instance;
    }

    //--------------------------------------
    /// 是否被实例化
    //-------------------------------------- 
    public static bool HasInstance()
    {
        return (s_instance != null);
    }

    //--------------------------------------
    /// 初始化
    /// @需要在派生类中实现
    //-------------------------------------- 
    public virtual void Init()
    {

    }

    //--------------------------------------
    /// 反初始化
    /// @需要在派生类中实现
    //-------------------------------------- 
    public virtual void UnInit()
    {

    }
};
