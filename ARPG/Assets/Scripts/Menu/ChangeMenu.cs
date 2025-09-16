using RainbowArt.CleanFlatUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChangeMenu : BaseMenu<ChangeMenu>
{
    public AsyncOperation ao;
    public Image image;
    public ProgressBarSpecial pbs;
    public bool ok;
    private float sum;
    public override void Init()
    {
        sum = 10f / 9f;
        Hide();
    }
    public override void Show()
    {
        base.Show();
        image.enabled = true;
    }
    public void Show(int id,bool ok)
    {
        Show();
        if (!ok)
        {
            image.enabled = false;
        }
        StartCoroutine(Change(id));
    }
    private IEnumerator Change(int id)
    {
        if(ok) ChangeInstance(null);
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.Hide();
            PlayerStateShow.Instance.Hide();
        }
        ao = SceneManager.LoadSceneAsync(id);
        ao.allowSceneActivation = false;
        while (ao.progress < 0.9f)
        {
            pbs.CurrentValue = ao.progress * sum * 10;
            yield return null;
        }
        pbs.CurrentValue = 100;
        ao.completed += (op) =>
        {
            Hide();
            if (TaskManager.Instance != null)
            {
                TaskManager.Instance.Show();
                PlayerStateShow.Instance.Show();
            }
        };
        yield return new WaitForSeconds(1f);
        ao.allowSceneActivation = true;
    }
    
}
