using System.Collections;
using UnityEngine;

public class TimerObject : MonoBehaviour, IActivable
{
    [Header("Parameters")]
    [SerializeField] private TimerMode _timerMode = TimerMode.Duration;
    [SerializeField] private float timerSeconds = 5f;
    
    [Header("Objects to activate")]
    [SerializeField] private GameObject[] _objectsToActivate;
    
    private IActivable[] _activables;
    private Coroutine _currentCoroutine;

    private enum TimerMode
    {
        Delay,
        Duration
    }
    private void Start()
    {
        _activables = new IActivable[_objectsToActivate.Length];
        for (int i = 0; i < _objectsToActivate.Length; i++)
        {
            _activables[i] = _objectsToActivate[i].GetComponent<IActivable>();
        }
    }
    public void Activate()
    { 
        if (_currentCoroutine != null)
            StopCoroutine(nameof(TimerLogic));
        _currentCoroutine = StartCoroutine(nameof(TimerLogic));
    }

    public void DeActivate()
    {
        throw new System.NotImplementedException();
    }

    public void SwapState()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator TimerLogic()
    {
        switch (_timerMode)
        {
            case TimerMode.Delay:
                yield return new WaitForSeconds(timerSeconds);
                foreach (IActivable activable in _activables)
                {
                    activable.Activate();
                }
                break;
            case TimerMode.Duration:
                foreach (IActivable activable in _activables)
                {
                    activable.Activate();
                }
        
                yield return new WaitForSeconds(timerSeconds);
        
                foreach (IActivable activable in _activables)
                {
                    activable.DeActivate();
                }

                break;
        }
        
        _currentCoroutine = null;
    }
}
