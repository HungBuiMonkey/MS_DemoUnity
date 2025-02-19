using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02UserInputListener : MonoBehaviour, EventListener<BEPS02FlyingOwlsInputChanner>
    {
        public void OnMMEvent(BEPS02FlyingOwlsInputChanner eventType)
        {
            switch (eventType.UserInput)
            {
                case BEPS02FlyingOwlsUserInput.Click:
                    GameObject dataEventClick = (GameObject)eventType.Data;
                    BEPS02FlyingOwlsClickStateEventData clickStateEventData = new BEPS02FlyingOwlsClickStateEventData();
                    clickStateEventData.ObjectClick = dataEventClick;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.ClickObject, clickStateEventData);
                    break;
              /*  case BEPS02FlyingOwlsUserInput.UnClick:
                    GameObject dataEventUnClick = (GameObject)eventType.Data;
                    BEPS02FlyingOwlsPlayStateEventData playUnClickStateEventData = new BEPS02FlyingOwlsPlayStateEventData();
                    playUnClickStateEventData.OwlObject = dataEventUnClick;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.PlayGame, playUnClickStateEventData);
                    break;*/
                case BEPS02FlyingOwlsUserInput.Dragging:
                    GameObject dataEventDragging = (GameObject)eventType.Data;
                    BEPS02FlyingOwlsDraggingStateEventData draggingStateEventData = new BEPS02FlyingOwlsDraggingStateEventData();
                    draggingStateEventData.ObjectEvent = dataEventDragging;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DraggingObject, draggingStateEventData);
                    break;
               /* case BEPS02FlyingOwlsUserInput.DragMatching:
                    (GameObject owl, GameObject shadow, Transform resetPosition) dataEventDrag = ((GameObject owl, GameObject shadow, Transform resetPosition))eventType.Data;
                    BEPS02FlyingOwlsDragResultStateEventData dragResultStateEventData = new BEPS02FlyingOwlsDragResultStateEventData();
                    dragResultStateEventData.OwlObject = dataEventDrag.owl;
                    dragResultStateEventData.ShadowOwlObject = dataEventDrag.shadow;
                    dragResultStateEventData.ResetPosition = dataEventDrag.resetPosition;
                    dragResultStateEventData.UserInput = BEPS02FlyingOwlsUserInput.DragMatching;
                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragResult, dragResultStateEventData);
                    break;
                case BEPS02FlyingOwlsUserInput.DragUnMatching:
                    (GameObject owl, Transform resetPosition) dataEventUnDrag = ((GameObject owl, Transform resetPosition))eventType.Data;
                    BEPS02FlyingOwlsDragResultStateEventData dragUnMachingStateEventData = new BEPS02FlyingOwlsDragResultStateEventData();
                    dragUnMachingStateEventData.OwlObject = dataEventUnDrag.owl;
                    dragUnMachingStateEventData.ResetPosition = dataEventUnDrag.resetPosition;
                    dragUnMachingStateEventData.UserInput = BEPS02FlyingOwlsUserInput.DragUnMatching;

                    BEPS02HandleData.TriggerFinishState(BEPS02FlyingOwlsState.DragResult, dragUnMachingStateEventData);
                    break;*/

            }
        }
        private void OnEnable()
        {
            this.ObserverStartListening<BEPS02FlyingOwlsInputChanner>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BEPS02FlyingOwlsInputChanner>();
        }
    }

}