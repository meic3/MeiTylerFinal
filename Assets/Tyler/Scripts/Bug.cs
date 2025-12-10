using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class Bug : MonoBehaviour
{
    BugMove bugMove;
    [HideInInspector] public BugLife bugLife;

    [SerializeField] bool debug = false;
    [SerializeField] GameObject healthDebug;
    private TextMeshProUGUI healthDebugText;

    private void Start()
    {
        bugMove = GetComponent<BugMove>();
        bugLife = GetComponent<BugLife>();

        if (debug && healthDebug != null)
        {
            GameObject healthDebugInstance = Instantiate(healthDebug, transform);
            healthDebugText = healthDebugInstance.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        UpdateSlowInstances();
        if (debug && healthDebugText != null)
        {
            healthDebugText.text = bugLife.currentHP.ToString("F2");
        }
    }

    #region slow


    public class SlowInstance
    {
        public float slow; // 0-no effect, 1-frozen
        public float duration;
        public SlowInstance(float slow, float duration)
        {
            this.slow = slow;
            this.duration = duration;
        }
    }

    List<SlowInstance> slowInstances = new List<SlowInstance>();

    // keep track of the slow instance with the highest slow
    public SlowInstance effectiveSlowInstance;

    public bool isSlowed = false;

    public void AddSlowInstance(float slow, float duration)
    {
        if (slow <= 0 || duration <= 0) return;

        isSlowed = true;

        SlowInstance instance = new SlowInstance(slow, duration);
        slowInstances.Add(instance);

        if (effectiveSlowInstance == null || slow > effectiveSlowInstance.slow)
        {
            effectiveSlowInstance = instance;
        }
    }

    // count down duration.
    private void UpdateSlowInstances()
    {
        for (int i = slowInstances.Count - 1; i >= 0; i--)
        {
            SlowInstance s = slowInstances[i];
            s.duration -= Time.deltaTime;

            if (s.duration <= 0f)
            {
                slowInstances.RemoveAt(i);
                if (s == effectiveSlowInstance)
                { FindNewEffectiveSlowInstance(); }
            }
        }
    }

    // find slow instance with highest slow
    void FindNewEffectiveSlowInstance()
    {
        effectiveSlowInstance = null;
        for (int i = 0; i < slowInstances.Count; i++)
        {
            SlowInstance s = slowInstances[i];

            if (effectiveSlowInstance == null || s.slow > effectiveSlowInstance.slow)
            {
                effectiveSlowInstance = s;
            }
        }

        if (effectiveSlowInstance == null) isSlowed = false;
    }

    #endregion
}