using Spine.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundDependency : Dependency
    {
        [SerializeField] private BEPP01BlendingSoundScriptableObject bEPP01BlendingSoundScriptableObject;
        [SerializeField] private RectTransform boxAnswer;
        [SerializeField] private RectTransform board;
        [SerializeField] private RectTransform boxUnderline;
        [SerializeField] private RectTransform studenGroup;
        [SerializeField] private RectTransform ellie;
        [SerializeField] private RectTransform BGBottom;
        [SerializeField] private RectTransform backGround;
        [SerializeField] private GameObject prefabUnderline;
        [SerializeField] private BEPP01Image fadeOutGame;
        [SerializeField] private Image lockButtonUnderline;
        [SerializeField] private Button buttonEllie;
        [SerializeField] private List<Button> listButtonAnswer; 
        [SerializeField] private List<GameObject> listLine; 

        [Header("Point")]
        [SerializeField] private RectTransform pointAnswerAppear;
        [SerializeField] private RectTransform pointBoxAnswerAppear;
        [SerializeField] private RectTransform pointAnswerInit;
        [SerializeField] private RectTransform pointBoardMoveIntro;
        [SerializeField] private RectTransform pointStartBoard;
        [SerializeField] private RectTransform pointStudentGroupDissapear;
        [Header("Animation")]
        [SerializeField, SpineAnimation] private string ellieNormal;
        [SerializeField, SpineAnimation] private string ellieIntro;
        [SerializeField, SpineAnimation] private string ellieGuiding;
        [SerializeField, SpineAnimation] private string ellieCorrect;
        [SerializeField, SpineAnimation] private string ellieCorrectToNormal;
        [SerializeField, SpineAnimation] private string ellieWrong;
        [SerializeField, SpineAnimation] private string ellieEndGame;
        [SerializeField, SpineAnimation] private string ellieEndGameToNormal;
        public override T GetStateData<T>()
        {
            T data;

            Type listType = typeof(T);

            if (listType == typeof(BEPP01BlendingSoundInitStateDataObjectDependency))
            {
                BEPP01BlendingSoundInitStateDataObjectDependency initStateData = new();
                initStateData.boxAnswer = boxAnswer;
                initStateData.board = board;
                initStateData.boxUnderline = boxUnderline;
                initStateData.listButtonAnswer = listButtonAnswer;
                initStateData.listLine = listLine;
                initStateData.pointAnswerInit = pointAnswerInit;
                initStateData.prefabUnderline = prefabUnderline;
                initStateData.StudentGroup = studenGroup.transform;
                data = ConvertToType<T>(initStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundIntroStateDataObjectDependency))
            {
                BEPP01BlendingSoundIntroStateDataObjectDependency introStateData = new();
                introStateData.introDataConfig = bEPP01BlendingSoundScriptableObject.introDataConfig;
                introStateData.board = board;
                introStateData.boxAnswer = boxAnswer;
                introStateData.pointBoxAnswerAppear = pointBoxAnswerAppear;
                introStateData.ListButtonAnswer = listButtonAnswer;
                introStateData.ellie = ellie;
                introStateData.ButtonEllie = buttonEllie;
                introStateData.ellieSkeleton = ellie.GetComponent<SkeletonGraphic>();
                introStateData.boxUnderline = boxUnderline;
                introStateData.studenGroup = studenGroup;
                introStateData.listLine = listLine;
                introStateData.BGBottom = BGBottom;
                introStateData.backGround = backGround;
                introStateData.pointAnswerAppear = pointAnswerAppear;
                introStateData.pointBoardMoveIntro = pointBoardMoveIntro;
                introStateData.pointStartBoard = pointStartBoard;
                introStateData.pointStudentGroupDissapear = pointStudentGroupDissapear;
                introStateData.ellieIntro = ellieIntro;
           
                data = ConvertToType<T>(introStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundGuidingStateDataObjectDependency))
            {
                BEPP01BlendingSoundGuidingStateDataObjectDependency guidingStateData = new();
                guidingStateData.ellieSkeleton = ellie.GetComponent<SkeletonGraphic>();
                guidingStateData.guidingDataConfig = bEPP01BlendingSoundScriptableObject.guidingDataConfig;
                guidingStateData.ListButtonAnswer = listButtonAnswer;
                guidingStateData.boxAnswer = boxAnswer;
                guidingStateData.pointBoxAnswerAppear = pointBoxAnswerAppear;
                guidingStateData.ellieGuiding = ellieGuiding;

                data = ConvertToType<T>(guidingStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundClickEllieStateDataObjectDependency))
            {
                BEPP01BlendingSoundClickEllieStateDataObjectDependency clickEllieStateData = new();
                clickEllieStateData.ellieSkeleton = ellie.GetComponent<SkeletonGraphic>();
                clickEllieStateData.ButtonEllie = buttonEllie;
                clickEllieStateData.ellieGuiding = ellieGuiding;
                clickEllieStateData.audioTopic = bEPP01BlendingSoundScriptableObject.guidingDataConfig.audioGuiding;
                data = ConvertToType<T>(clickEllieStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundClickUnderlineStateDataObjectDependency))
            {
                BEPP01BlendingSoundClickUnderlineStateDataObjectDependency clickUnderlineStateData = new();
                clickUnderlineStateData.boxUnderline = boxUnderline;
                clickUnderlineStateData.LockButtonUnderline = lockButtonUnderline;
                data = ConvertToType<T>(clickUnderlineStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundClickAnswerStateDataObjectDependency))
            {
                BEPP01BlendingSoundClickAnswerStateDataObjectDependency clickAnswerStateData = new();
                clickAnswerStateData.ClickAnswerDataConfig = bEPP01BlendingSoundScriptableObject.clickAnswerDataConfig;
                clickAnswerStateData.BoxUnderline = boxUnderline;
                clickAnswerStateData.BoxAnswer = boxAnswer;
                clickAnswerStateData.ListButtonAnswer = listButtonAnswer;
                clickAnswerStateData.listLine = listLine;
                clickAnswerStateData.ellie = ellie;
                clickAnswerStateData.EllieSkeleton = ellie.GetComponent<SkeletonGraphic>();
                clickAnswerStateData.ButtonEllie = buttonEllie;
                clickAnswerStateData.pointAnswerInit = pointAnswerInit;
                clickAnswerStateData.ellieNormal = ellieNormal;
                clickAnswerStateData.ellieCorrect = ellieCorrect;
                clickAnswerStateData.ellieCorrectToNormal = ellieCorrectToNormal;
                clickAnswerStateData.ellieWrong = ellieWrong;
                data = ConvertToType<T>(clickAnswerStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency))
            {
                BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency prepareNextTurnStateData = new();
                prepareNextTurnStateData.BoxAnswer = boxAnswer;
                prepareNextTurnStateData.boxUnderline = boxUnderline;
                prepareNextTurnStateData.ButtonEllie = buttonEllie;
                prepareNextTurnStateData.ListButtonAnswer = listButtonAnswer;
                prepareNextTurnStateData.listLine = listLine;
                data = ConvertToType<T>(prepareNextTurnStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundEndGameStateDataObjectDependency))
            {
                BEPP01BlendingSoundEndGameStateDataObjectDependency endGameStateData = new();
                endGameStateData.BGBottom = BGBottom;
                endGameStateData.backGround = backGround;
                endGameStateData.ellie = ellie;
                endGameStateData.ListButtonAnswer = listButtonAnswer;
                endGameStateData.listLine = listLine;
                endGameStateData.FadeOutGame = fadeOutGame;
                endGameStateData.ellieSkeleton = ellie.GetComponent<SkeletonGraphic>();
                endGameStateData.board = board;
                endGameStateData.ellieNormal = ellieNormal;
                endGameStateData.ellieEndGame = ellieEndGame;
                endGameStateData.ellieEndGameToNormal = ellieEndGameToNormal;
                data = ConvertToType<T>(endGameStateData);
            }
            else if (listType == typeof(BEPP01BlendingSoundNextStateDependency))
            {
                BEPP01BlendingSoundNextStateDependency nextStateDependency = new();
                nextStateDependency.BoxAnswer = boxAnswer;
                nextStateDependency.BoxUnderline = boxUnderline;
                data = ConvertToType<T>(nextStateDependency);
            }
            else
            {
                data = ConvertToType<T>(null);
            }

            return data;
        }
    }
}