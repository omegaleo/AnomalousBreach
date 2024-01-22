using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class NewsPanel : InstancedBehavior<NewsPanel>
{
    [SerializeField] private List<TMP_Text> _texts = new List<TMP_Text>();
    [SerializeField] private float _secondsToShow = 28f;
    
    private Vector3 _shownPosition;
    private Vector3 _hidePosition;
    private Animator _animator;
    public bool showing;
    
    // Start is called before the first frame update
    void Start()
    {
        var rect = GetComponent<RectTransform>();
        
        _shownPosition = transform.localPosition;
        _hidePosition = transform.localPosition - new Vector3(0f, rect.rect.height, 0f);

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showing)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _shownPosition, 0.125f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _hidePosition, 0.125f);
        }
    }

    public void Show(string text)
    {
        _texts.ForEach(x => x.text = text); // Change the text inside our text elements that loop around
        showing = true;
        _animator.enabled = true;

        StartCoroutine(WaitToHide());
    }

    private IEnumerator WaitToHide()
    {
        yield return new WaitForSeconds(_secondsToShow);
        showing = false;
        _animator.enabled = false;
    }
}
