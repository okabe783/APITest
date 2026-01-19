using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

// スクロール作成クラス
public class PrefScrollView : MonoBehaviour
{
    [SerializeField] 
    private GameObject _prefScrollViewContent;
    [SerializeField] 
    private UIButton _prefPanelButton;
    [SerializeField]
    private UIButton _cityPanelButton;

    [Header("Page Settings")]
    [SerializeField]
    private RectTransform _contentPrefab;
    [SerializeField]
    private ScrollRect  _scrollRect;

    [SerializeField] 
    private UIButton _backButton;

    private List<PrefInfo> _prefList = new();
    private Stack<RectTransform> _contents = new();

    public event Action<CityInfo> OnCityClicked;

    private void Start()
    {
        _backButton.OnClickAddListener(GoBack);
    }

    // PrefListを受け取る
    public void SetUpPrefPanel(List<PrefInfo> prefs)
    {
        _prefList = prefs;
        CreatePrefPanel();
    }

    // 47都道府県分のPanelを生成
    private void CreatePrefPanel()
    {
        GameObject obj = Instantiate(_contentPrefab.gameObject, _prefScrollViewContent.transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        _scrollRect.content = rect;
        _contents.Push(rect);
        foreach (PrefInfo prefInfo in _prefList)
        {
            GameObject prefPanel = Instantiate(_prefPanelButton.gameObject, obj.transform);
            PrefPanelView prefPanelView = prefPanel.GetComponent<PrefPanelView>();
            prefPanelView.SetUpPrefButton(prefInfo);
            prefPanelView.OnClicked += OnPrefSelected;
        }
    }
    
    private void OnPrefSelected(PrefInfo prefInfo)
    {
        SlideToPage();
        CreateCityPanel(prefInfo);
    }

    private void OnCitySelected(CityInfo cityInfo)
    {
        OnCityClicked?.Invoke(cityInfo);
    }

    // Pageをスライドする
    private void SlideToPage()
    {
        float pageWidth = 1080f;
        Vector2 targetPos = new (-pageWidth,0);
        _scrollRect.content.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutCubic);
    }

    private void CreateCityPanel(PrefInfo prefInfo)
    {
        GameObject obj = Instantiate(_contentPrefab.gameObject, _prefScrollViewContent.transform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        _scrollRect.content = rect;
        _contents.Push(rect);
        _backButton.gameObject.SetActive(true);
        foreach (CityInfo cityInfo in prefInfo.Cities) 
        { 
            GameObject prefPanel = Instantiate(_cityPanelButton.gameObject, obj.transform); 
            CityPanelView prefPanelView = prefPanel.GetComponent<CityPanelView>(); 
            prefPanelView.SetUpCityButton(cityInfo);
            prefPanelView.OnClicked += OnCitySelected;
        }
    }

    // 戻るボタンの処理
    private void GoBack()
    {
        if (_contents.Count <= 1) return;
        
        RectTransform current = _contents.Pop();
        RectTransform previous = _contents.Peek();

        float pageWidth = 1080f;

        // 戻るページを左から出す
        previous.gameObject.SetActive(true);
        previous.anchoredPosition = new Vector2(-pageWidth, 0);

        _scrollRect.enabled = false;

        Sequence seq = DOTween.Sequence();
        seq.Join(current.DOAnchorPos(new Vector2(pageWidth, 0), 0.5f));
        seq.Join(previous.DOAnchorPos(Vector2.zero, 0.5f));
            seq.OnComplete(() =>
            {
                Destroy(current.gameObject);
                _scrollRect.content = previous;
                _scrollRect.enabled = true;
                _backButton.gameObject.SetActive(_contents.Count > 1);
            });
    }
}