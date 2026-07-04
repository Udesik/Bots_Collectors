using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceBar : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private List<Slider> _sliders;

    [SerializeField] private float _speed = 2f;
    
    // ИСПРАВЛЕНО: Массив корутин под каждый слайдер, чтобы они не сбрасывали друг друга в цикле
    private Coroutine[] _updateCoroutines;

    private void Awake()
    {
        // Создаем массив корутин по количеству слайдеров
        _updateCoroutines = new Coroutine[_sliders.Count];
    }

    private void OnEnable()
    {
        _base.ResourceReceived += OnResourceReceived;
    }

    private void OnDisable()
    {
        _base.ResourceReceived -= OnResourceReceived;
    }

    private void OnResourceReceived(int[] resources, int max)
    {
        for (int i = 0; i < _sliders.Count; i++)
        {
            // Проверка, чтобы не выйти за пределы пришедшего массива ресурсов
            if (i >= resources.Length) break;
            if (_sliders[i] == null) continue;

            // ИСПРАВЛЕНО: Приведение к (float). Теперь деление целых чисел выдает дробное значение (например, 0.5f вместо 0)
            float target = (float)resources[i] / max;
        
            // ИСПРАВЛЕНО: Останавливаем корутину именно для текущего слайдера по индексу i
            if (_updateCoroutines[i] != null) 
                StopCoroutine(_updateCoroutines[i]);
        
            // Запускаем корутину в её личную ячейку массива
            _updateCoroutines[i] = StartCoroutine(SmoothUpdate(target, i));
        }
    }

    private IEnumerator SmoothUpdate(float targetValue, int index)
    {
        while (!Mathf.Approximately(_sliders[index].value, targetValue))
        {
            _sliders[index].value = Mathf.MoveTowards(_sliders[index].value, targetValue, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
