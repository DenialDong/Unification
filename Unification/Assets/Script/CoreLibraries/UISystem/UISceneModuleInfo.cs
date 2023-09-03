using System;
using System.Collections.Generic;

public class UISceneModuleInfo
{
    public Type targetClass;

    //模块名称
    public string name;

    //是否缓存
    public bool cacheEnabled;

    //需要提前加载的资源列表
    public List<string> preResList;

    public UISceneModuleInfo(string _name, bool _cacheEnabled, List<string> _preResList)
    {
        name = _name;
        targetClass = Type.GetType(_name);
        cacheEnabled = _cacheEnabled;
        preResList = _preResList;
    }
}