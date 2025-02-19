using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSUserInputListener : MonoBehaviour, EventListener<BEPS01RFSInputChanner>
    {
        public void OnMMEvent(BEPS01RFSInputChanner eventType)
        {
            switch (eventType.UserInput)
            {
                case BEPS01RFSUserInput.Click:
                    GameObject dataEventClick = (GameObject)eventType.Data;
                    BEPS01RFSClickStateEventData clickStateEventData = new BEPS01RFSClickStateEventData();
                    clickStateEventData.ObjectEvent = dataEventClick;
                    clickStateEventData.UserInput = BEPS01RFSUserInput.Click;
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.ClickObject, clickStateEventData);
                    break;
                case BEPS01RFSUserInput.ClickCat:
                    BEPS01RFSClickStateEventData clickCatStateEventData = new BEPS01RFSClickStateEventData();
                    clickCatStateEventData.ObjectEvent = null;
                    clickCatStateEventData.UserInput = BEPS01RFSUserInput.ClickCat;
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.ClickObject, clickCatStateEventData);
                    break;
                case BEPS01RFSUserInput.SkipGuiding:
                    BEPS01RFSClickStateEventData clickStateGuidingEventData = new BEPS01RFSClickStateEventData();
                    clickStateGuidingEventData.UserInput = BEPS01RFSUserInput.SkipGuiding;
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.ClickObject, clickStateGuidingEventData);
                    break;
           
                case BEPS01RFSUserInput.SkipIntro:

                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.IntroGame, true);
                    break;
                case BEPS01RFSUserInput.UnClick:

                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.PlayGame, null);
                    break;
                case BEPS01RFSUserInput.Dragging:
                    GameObject dataEventDragging = (GameObject)eventType.Data;
                    BEPS01RFSDraggingStateEventData draggingStateEventData = new BEPS01RFSDraggingStateEventData();
                    draggingStateEventData.ObjectEvent = dataEventDragging;
                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DraggingObject, draggingStateEventData);
                    break;

                /*case BEPS01RFSUserInput.DragMatching:
                    (GameObject tube, GameObject dashBox, Transform resetPosition) dataEventDrag = ((GameObject tube, GameObject dashBox, Transform resetPosition))eventType.Data;
                  
                    BEPS01RFSDragResultStateEventData dragResultStateEventData = new BEPS01RFSDragResultStateEventData();
                    dragResultStateEventData.TubeObject = dataEventDrag.tube;
                    dragResultStateEventData.DashBoxObject = dataEventDrag.dashBox;
                    dragResultStateEventData.ResetPosition = dataEventDrag.resetPosition;
                    dragResultStateEventData.UserInput = BEPS01RFSUserInput.DragMatching;

                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragResult, dragResultStateEventData);
                    break;
                case BEPS01RFSUserInput.DragUnMatching:
                    (GameObject tube, Transform resetPosition) dataEventUnDrag = ((GameObject tube, Transform resetPosition))eventType.Data;

                    BEPS01RFSDragResultStateEventData dragUnMachingStateEventData = new BEPS01RFSDragResultStateEventData();
                    dragUnMachingStateEventData.TubeObject = dataEventUnDrag.tube;
                    dragUnMachingStateEventData.ResetPosition = dataEventUnDrag.resetPosition;
                    dragUnMachingStateEventData.UserInput = BEPS01RFSUserInput.DragUnMatching;

                    BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.DragResult, dragUnMachingStateEventData);
                    break;*/
            }
        }
        private void OnEnable()
        {
            this.ObserverStartListening<BEPS01RFSInputChanner>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BEPS01RFSInputChanner>();
        }
    }
}