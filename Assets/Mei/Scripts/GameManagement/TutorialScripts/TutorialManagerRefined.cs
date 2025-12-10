using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManagerRefined : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [Header("Tutorial Step Settings")]
        public GameObject interactObject;
        public Transform targetPosition;
        public float targetRadius = 1f;
        
        [Header("Visual Indicators")]
        public GameObject indicator; // The (!) sprite - existing GameObject in scene
        public Vector3 indicatorOffset = Vector3.zero;
        public GameObject targetFrame; // Dotted frame - existing GameObject in scene
        
        [Header("Step Behavior")]
        public TutorialStepType stepType;
        public int requiredClicks = 0; // For right-click tutorials
        public int requiredCount = 0; // For counting objects
        public string layerToCount = ""; // Layer name for counting
    }
    
    public enum TutorialStepType
    {
        DragToTarget,        // Tutorial 0 & 2 - Drag object to target
        WaitForBugKills,     // Tutorial 1 - Wait for bugs to be killed
        RightClickCount,     // Tutorial 3 - Count right clicks
        WaitForMoneyChange,  // Tutorial 4 - Wait for money to change
        CountObjectsInLayer, // Tutorial 5 - Count objects in layer
        Final                // Tutorial 6 - End tutorial
    }

    [Header("Tutorial Steps")]
    [SerializeField] private List<TutorialStep> tutorialSteps = new List<TutorialStep>();
    
    [Header("UI Elements")]
    [SerializeField] private GameObject textbox;
    [SerializeField] private SpriteRenderer blackSprite;
    [SerializeField] private GameObject panelThatCoversAllUI;
    [SerializeField] private GameObject panelThatCoversShopUI;
    [SerializeField] private GameObject coverSquare;
    
    [Header("Managers")]
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private BugGenerator bugGenerator;
    [SerializeField] private TutorialTextboxManager tutorialTextboxManager;
    
    // State tracking
    private int currentStepIndex = 0;
    private bool inTutorial = false;
    private Vector3 dragStartPosition;
    private int clickCount = 0;
    private float trackedMoney = 0f;

    void Awake()
    {
        if (textbox != null)
            textbox.SetActive(true);
        
        if (blackSprite != null)
            blackSprite.enabled = true;
        
        inTutorial = false;
        
        if (panelThatCoversShopUI != null)
            panelThatCoversShopUI.SetActive(true);
        
        // Disable all interact objects and hide all indicators initially
        foreach (var step in tutorialSteps)
        {
            if (step == null)
                continue;
                
            if (step.interactObject != null)
            {
                DisableInteractObject(step.interactObject);
            }
            
            if (step.indicator != null)
            {
                step.indicator.SetActive(false);
            }
            
            if (step.targetFrame != null)
            {
                step.targetFrame.SetActive(false);
            }
        }
    }

    void Start()
    {
        // Additional initialization if needed
    }

    void Update()
    {
        if (!inTutorial || currentStepIndex >= tutorialSteps.Count)
            return;

        TutorialStep currentStep = tutorialSteps[currentStepIndex];

        switch (currentStep.stepType)
        {
            case TutorialStepType.DragToTarget:
                HandleDragToTarget(currentStep);
                break;
                
            case TutorialStepType.WaitForBugKills:
                HandleWaitForBugKills(currentStep);
                break;
                
            case TutorialStepType.RightClickCount:
                HandleRightClickCount(currentStep);
                break;
                
            case TutorialStepType.WaitForMoneyChange:
                HandleWaitForMoneyChange(currentStep);
                break;
                
            case TutorialStepType.CountObjectsInLayer:
                HandleCountObjectsInLayer(currentStep);
                break;
                
            case TutorialStepType.Final:
                // Final step handled in NewTutorial()
                break;
        }
    }

    #region Step Handlers

    private void HandleDragToTarget(TutorialStep step)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (CheckCardPosition(step))
            {
                // Snap card to target position
                if (step.interactObject != null && step.targetPosition != null)
                {
                    GameObject parent = step.interactObject.transform.parent.gameObject;
                    parent.transform.position = step.targetPosition.position;
                }
                
                CompleteCurrentStep();
            }
            else
            {
                // Return to starting position if dropped in wrong place
                if (step.interactObject != null)
                {
                    GameObject parent = step.interactObject.transform.parent.gameObject;
                    parent.transform.position = dragStartPosition;
                }
            }
        }
    }

    private void HandleWaitForBugKills(TutorialStep step)
    {
        panelThatCoversAllUI.SetActive(false);
        panelThatCoversShopUI.SetActive(true);
        phaseManager.isPaused = false;
        bugGenerator.isPaused = false;

        if (bugGenerator.deadBug >= step.requiredCount)
        {
            // Re-enable the next interact object
            if (currentStepIndex + 1 < tutorialSteps.Count)
            {
                var nextStep = tutorialSteps[currentStepIndex + 1];
                if (nextStep.interactObject != null)
                {
                    EnableInteractObject(nextStep.interactObject);
                }
            }
            
            panelThatCoversAllUI.SetActive(true);
            panelThatCoversShopUI.SetActive(false);
            phaseManager.isPaused = true;
            bugGenerator.isPaused = true;
            
            CompleteCurrentStep();
        }
    }

    private void HandleRightClickCount(TutorialStep step)
    {
        if (Input.GetMouseButtonDown(1))
        {
            clickCount++;
            
            if (clickCount >= step.requiredClicks)
            {
                CompleteCurrentStep();
            }
        }
    }

    private void HandleWaitForMoneyChange(TutorialStep step)
    {
        if (PlayerMoney.money != trackedMoney)
        {
            CompleteCurrentStep();
        }
    }

    private void HandleCountObjectsInLayer(TutorialStep step)
    {
        if (Input.GetMouseButtonUp(0))
        {
            int count = FindObjectsInLayer(step.layerToCount);
            
            if (count >= step.requiredCount)
            {
                CompleteCurrentStep();
            }
        }
    }

    #endregion

    #region Tutorial Flow Control

    private void CompleteCurrentStep()
    {
        Debug.Log("Completing tutorial step: " + currentStepIndex);
        
        // Clean up current step indicators
        if (currentStepIndex < tutorialSteps.Count)
        {
            HideStepIndicators(tutorialSteps[currentStepIndex]);
            
            // Disable current interact object
            if (tutorialSteps[currentStepIndex].interactObject != null)
            {
                DisableInteractObject(tutorialSteps[currentStepIndex].interactObject);
            }
        }
        
        currentStepIndex++;
        Debug.Log("Advanced to step: " + currentStepIndex);
        
        if (textbox != null)
            textbox.SetActive(true);
        
        if (blackSprite != null)
            blackSprite.enabled = true;
        
        inTutorial = false;
        
        if (tutorialTextboxManager != null)
            tutorialTextboxManager.NextTextGroup();
        else
            Debug.LogError("TutorialTextboxManager is not assigned!");
    }

    public void NewTutorial()
    {
        Debug.Log("NewTutorial called for step: " + currentStepIndex);
        
        if (currentStepIndex >= tutorialSteps.Count)
        {
            Debug.Log("Tutorial complete - loading next scene");
            // Tutorial complete - load next scene
            SceneManager.LoadScene("Mei_TitleScreen");
            return;
        }

        textbox.SetActive(false);
        blackSprite.enabled = false;
        inTutorial = true;

        TutorialStep currentStep = tutorialSteps[currentStepIndex];
        Debug.Log("Starting tutorial step type: " + currentStep.stepType);

        // Setup step based on type
        switch (currentStep.stepType)
        {
            case TutorialStepType.DragToTarget:
                SetupDragToTargetStep(currentStep);
                break;
                
            case TutorialStepType.WaitForBugKills:
                SetupWaitForBugKillsStep(currentStep);
                break;
                
            case TutorialStepType.RightClickCount:
                clickCount = 0;
                ShowIndicator(currentStep);
                break;
                
            case TutorialStepType.WaitForMoneyChange:
                trackedMoney = PlayerMoney.money;
                ShowIndicator(currentStep);
                break;
                
            case TutorialStepType.CountObjectsInLayer:
                panelThatCoversAllUI.SetActive(false);
                panelThatCoversShopUI.SetActive(false);
                if (coverSquare != null)
                    coverSquare.SetActive(false);
                ShowIndicator(currentStep);
                break;
                
            case TutorialStepType.Final:
                SceneManager.LoadScene("Mei_TitleScreen");
                break;
        }
    }

    private void SetupDragToTargetStep(TutorialStep step)
    {
        if (step.interactObject != null)
        {
            EnableInteractObject(step.interactObject);
            GameObject parent = step.interactObject.transform.parent.gameObject;
            dragStartPosition = parent.transform.position;
        }
        
        ShowIndicator(step);
        ShowTargetFrame(step);
    }

    private void SetupWaitForBugKillsStep(TutorialStep step)
    {
        if (step.interactObject != null)
        {
            EnableInteractObject(step.interactObject);
        }
        
        ShowIndicator(step);
    }

    #endregion

    #region Visual Indicators

    private void ShowIndicator(TutorialStep step)
    {
        if (step.indicator != null && step.interactObject != null)
        {
            step.indicator.SetActive(true);
            Vector3 spawnPos = step.interactObject.transform.position + step.indicatorOffset;
            step.indicator.transform.position = spawnPos;
        }
    }

    private void ShowTargetFrame(TutorialStep step)
    {
        if (step.targetFrame != null)
        {
            step.targetFrame.SetActive(true);
            // If target frame is a child of targetPosition, it will already be at the right position
            // Otherwise position it at the target
            if (step.targetPosition != null && step.targetFrame.transform.parent != step.targetPosition)
            {
                step.targetFrame.transform.position = step.targetPosition.position;
            }
        }
    }

    private void HideStepIndicators(TutorialStep step)
    {
        if (step.indicator != null)
        {
            step.indicator.SetActive(false);
        }
        
        if (step.targetFrame != null)
        {
            step.targetFrame.SetActive(false);
        }
    }

    #endregion

    #region Helper Methods

    private void EnableInteractObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Trying to enable null interact object");
            return;
        }
        
        Transform parentTransform = obj.transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("Interact object has no parent: " + obj.name);
            return;
        }
        
        GameObject parent = parentTransform.gameObject;
        Vector3 pos = parent.transform.position;
        parent.transform.position = new Vector3(pos.x, pos.y, 0);
    }

    private void DisableInteractObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Trying to disable null interact object");
            return;
        }
        
        Transform parentTransform = obj.transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("Interact object has no parent: " + obj.name);
            return;
        }
        
        GameObject parent = parentTransform.gameObject;
        Vector3 pos = parent.transform.position;
        parent.transform.position = new Vector3(pos.x, pos.y, 10);
    }

    private bool CheckCardPosition(TutorialStep step)
    {
        if (step.interactObject == null || step.targetPosition == null)
            return false;

        GameObject parent = step.interactObject.transform.parent.gameObject;
        float dist = Vector3.Distance(parent.transform.position, step.targetPosition.position);
        
        return dist <= step.targetRadius;
    }

    private int FindObjectsInLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                count++;
            }
        }

        return count;
    }

    #endregion
}