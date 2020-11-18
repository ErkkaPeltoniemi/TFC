using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Linq.Expressions;
using System;

public class Planner : MonoBehaviour
{
    public float planLengthInSeconds = 60;

    private Dictionary<GameObject, Plan> plans;
    private Plan planRecording;
    private List<Plan> plansPlaying;

    public Planner()
    {
        plans = new Dictionary<GameObject, Plan>();
        plansPlaying = new List<Plan>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if(planRecording != null)
        {

     
            planRecording.RecordStep();

            if(planRecording.State == Plan.PlanState.recordingFinished)
            {
                StopRecordingPlan();
            }
        }
        bool playFinished = false;
        foreach(var plan in plansPlaying)
        {
      
            if(!plan.PlayNextStep())
            {
                playFinished = true;
            }
        }

        if (playFinished)
        {
            
            foreach(var go in plansPlaying.Select(p => p.gameObject))
            {
                go.SendMessage("OnFinishedPlayingWhileRecordingAnotherCharacter");
            }
            plansPlaying.RemoveAll(p => p.State == Plan.PlanState.playingFinished);
            SendMessage("OnPlayingFinished");
        }
    }

    public void StartPlayingPlans(bool whileRecording = false)
    {

        plansPlaying = plans.Values.Where(p => p.recordingComplete).ToList();
        foreach (var plan in plansPlaying)
        {
            if(whileRecording)
            {
                plan.gameObject.SendMessage("OnPlayingWhileRecordingAnotherCharacter");
            }
            plan.StartPlaying();
        }
    }

    private void StopRecordingPlan()
    {
    
        SendMessage("OnRecordingFinished");
        planRecording = null;    
    }

    //Messages
    public void StartRecordingPlan(GameObject gameObject)
    {
        //discard currently recording plan
        if(planRecording != null)
        {
            planRecording = null;
        }

        Plan existingPlan;
        if(plans.TryGetValue(gameObject,out existingPlan))
        {
            plans.Remove(gameObject);
        }

        planRecording = new Plan(gameObject);
        plans.Add(gameObject, planRecording);
        planRecording.StartRecording(planLengthInSeconds);
        StartPlayingPlans(true);
    }

    private class Plan
    {
        public GameObject gameObject { get; private set; }
        public bool recordingComplete { get; private set; }

        List<PlanStep> steps;
        private int activeIndex = 0;
        public int recordedStepsCount
        {
            get
            {
                return steps != null ? steps.Count : 0;
            }
        }
        public float playingLeft { get; private set; }
        public float recordingLeft { get; private set; }

        private float timeSinceRecordingStarted;
        private float timeSincePlayingStarted;

        public PlanState State
        {
            private set;get;
        }
        private float planLength;

        public Plan(GameObject gameObject)
        {
            steps = new List<PlanStep>();
            this.gameObject = gameObject;
        }

        internal void RecordStep()
        {
            var timeLeft = recordingLeft - Time.deltaTime;
           
            
            if (timeLeft > 0)
            {
                timeSinceRecordingStarted += Time.deltaTime;
                recordingLeft = timeLeft;
                
                steps.Add(new PlanStep()
                {  
                    position = gameObject.transform.position,
                    rotation = gameObject.transform.rotation,
                    deltaTime = Time.deltaTime,
                    timeStamp = timeSinceRecordingStarted
                });
            }else
            {
                State = PlanState.recordingFinished;
                recordingComplete = true;
                timeSinceRecordingStarted = 0f;
            }
        }


        internal void StartRecording(float planLength)
        {
            State = PlanState.recording;
            this.planLength = planLength;
            this.recordingLeft = planLength;
            this.timeSinceRecordingStarted = 0f;
        }

        public void StartPlaying()
        {
            State = PlanState.playing;
            gameObject.GetComponent<CommandCharacter>().enabled = false;
            playingLeft = planLength;
            timeSincePlayingStarted = 0f;
            activeIndex = 0;
        }
        public bool PlayNextStep()
        {
            var lastIndex = steps.Count - 1;
            if(activeIndex <= lastIndex)
            {
                var currentStep = steps[activeIndex];
                int i = activeIndex;
                while (i <= lastIndex && timeSincePlayingStarted > steps[i].timeStamp)
                {
                    gameObject.transform.position = currentStep.position;
                    gameObject.transform.rotation = currentStep.rotation;                  
                    i++;               
                }
                activeIndex = i;

                playingLeft -= Time.deltaTime;
                timeSincePlayingStarted += Time.deltaTime;
                return true;
            }
            else
            {
                State = PlanState.playingFinished;
                timeSincePlayingStarted = 0f;
                activeIndex = 0;
                return false;
            }
        }


        public enum PlanState { notActive, recording, recordingFinished, playing, playingFinished }
    }

    private class PlanStep
    {
        public float deltaTime;
        public Vector3 position;
        public Quaternion rotation;
        internal float timeStamp;
    }
}


