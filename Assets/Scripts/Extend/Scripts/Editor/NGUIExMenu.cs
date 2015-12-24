using UnityEngine;
using UnityEditor;
using System.Collections;

public class NGUIExMenu : MonoBehaviour
{
    [MenuItem("NGUIEx/UI Dummy/Hide Widgets")]
    public static void AttachDummy()
    {
        GameObject[] selectedObjs = Selection.gameObjects;
        for (int i = 0; i < selectedObjs.Length; i++)
        {
            GameObject curObj = selectedObjs[i];
            PrefabType prefabType = PrefabUtility.GetPrefabType(curObj);
            if (prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab)
            {
                Debug.LogWarning("Please select GameObject in Hierarchy view: " + curObj.name);
                continue;
            }
            int idx = 0;
            UILabel[] labels = curObj.GetComponentsInChildren<UILabel>(true);
            for (idx = 0; idx < labels.Length; idx++)
            {
                UIDummyLabel.AttachDummy(labels[idx], true);
            }
            UISprite[] sprites = curObj.GetComponentsInChildren<UISprite>(true);
            for (idx = 0; idx < sprites.Length; idx++)
            {
                UIDummySprite.AttachDummy(sprites[idx], true);
            }
        }
    }

    [MenuItem("NGUIEx/UI Dummy/Show Widgets")]
    public static void RemoveDummy()
    {
        GameObject[] selectedObjs = Selection.gameObjects;
        for (int i = 0; i < selectedObjs.Length; i++)
        {
            GameObject curObj = selectedObjs[i];
            PrefabType prefabType = PrefabUtility.GetPrefabType(curObj);
            if (prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab)
            {
                Debug.LogWarning("Please select GameObject in Hierarchy view: " + curObj.name);
                continue;
            }
            int idx = 0;
            UIDummyLabel[] dummyLabels = curObj.GetComponentsInChildren<UIDummyLabel>(true);
            for (idx = 0; idx < dummyLabels.Length; idx++)
            {
                dummyLabels[idx].RemoveDummy(true);
            }
            UIDummySprite[] dummySprites = curObj.GetComponentsInChildren<UIDummySprite>(true);
            for (idx = 0; idx < dummySprites.Length; idx++)
            {
                dummySprites[idx].RemoveDummy(true);
            }
        }
    }

    [MenuItem("NGUIEx/Attach Component Dict")]
    public static void AttachComponentDict()
    {
        GameObject[] selectedObjs = Selection.gameObjects;
        for (int i = 0; i < selectedObjs.Length; i++)
        {
            GameObject curObj = selectedObjs[i];
            PrefabType prefabType = PrefabUtility.GetPrefabType(curObj);
            if (prefabType == PrefabType.Prefab || prefabType == PrefabType.ModelPrefab)
            {
                Debug.LogWarning("Please select GameObject in Hierarchy view: " + curObj.name);
                continue;
            }
            UIComponentDict componentDict = curObj.GetComponent<UIComponentDict>();
            if (componentDict == null)
            {
                componentDict = curObj.AddComponent<UIComponentDict>();
            }
            componentDict.m_refreshNow = true;
            EditorUtility.SetDirty(curObj);
        }
    }
}
