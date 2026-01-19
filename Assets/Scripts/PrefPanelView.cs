using System;
using UnityEngine;

// ボタンが押されたことを通知する
public class PrefPanelView : MonoBehaviour
{
    [SerializeField]
    private UIButton _prefButton;
    
    private PrefInfo _prefInfo;
    
    // 押したことを通知するためのイベント
    public event Action<PrefInfo> OnClicked;

    public void SetUpPrefButton(PrefInfo prefInfo)
    {
        _prefInfo = prefInfo;
        _prefButton.SetText(prefInfo.Name);
        _prefButton.OnClickAddListener(() => OnClicked?.Invoke(_prefInfo));
    }
}