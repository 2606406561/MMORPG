using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BatchReplaceUIMaterials : MonoBehaviour
{
    public Material targetMaterial;
    void Awake()
    {
        BatchReplace();
    }

    [ContextMenu("批量替换所有UI材质")]
    public void BatchReplace()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("请先指定目标材质");
            return;
        }

        // 获取场景中所有UI元素
        var allUIElements = FindObjectsOfType<Graphic>().ToList();
        int replacedCount = 0;
        foreach (var ui in allUIElements)
        {
            // 跳过已经使用目标材质的元素
            if (ui.material != null && ui.material.name == targetMaterial.name)
                continue;

            // 强制替换
            Material newMat = new Material(targetMaterial);
            ui.material = newMat;
            replacedCount++;
        }

        Debug.Log($"批量替换完成，共处理 {replacedCount} 个UI元素");
    }
}
