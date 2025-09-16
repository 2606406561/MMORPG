using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Renderer))]
public class CubeTextureAnimator : MonoBehaviour
{
    // 拖入所有序列帧纹理（确保是Texture2D类型）
    public Texture2D[] frames = new Texture2D[24];
    // 每帧播放时间（秒）
    public float frameDuration = 0.1f;
    // 材质实例（避免影响其他物体）
    private Material materialInstance;

    private int currentFrameIndex = 0;
    private float timer = 0;

    public void OnTriggerEnter(Collider other)
    {
        CameraController.Instance.Change();
        List<object> list = new List<object>();
        list.Add(TCPClick.Instance.user_ID);
        list.Add(BackPackManager.Instance.playerData.scene_id);
        list.Add(2);
        PlayerController.Instance.ChangeScene();
        TCPClick.Instance.SendMessage(14, list);
        BackPackManager.Instance.playerData.scene_id = 2;
        TaskManager.Instance.Hide();
        PlayerStateShow.Instance.Hide();
        ChangeMenu.Instance.Show(2,false);
    }
    void Start()
    {
        for (int i = 0; i < 24; i++) frames[i] = Resources.Load<Texture2D>("gif/" + (i + 1).ToString());
        // 获取Cube的渲染组件
        Renderer renderer = GetComponent<Renderer>();
        // 创建材质实例，防止修改共享材质
        materialInstance = new Material(renderer.material);
        renderer.material = materialInstance;

        // 初始显示第一帧
        if (frames.Length > 0)
        {
            materialInstance.mainTexture = frames[0];
        }
    }

    void Update()
    {
        if (frames.Length == 0) return;

        // 计时切换帧
        timer += Time.deltaTime;
        if (timer >= frameDuration)
        {
            // 循环切换到下一帧
            currentFrameIndex = (currentFrameIndex + 1) % frames.Length;
            materialInstance.mainTexture = frames[currentFrameIndex];
            timer = 0;
        }
    }

    // 清理材质实例，避免内存泄漏
    void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}
