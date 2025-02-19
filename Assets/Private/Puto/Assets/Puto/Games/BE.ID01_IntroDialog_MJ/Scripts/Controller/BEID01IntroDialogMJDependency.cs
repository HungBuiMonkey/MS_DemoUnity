using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Coffee.UIExtensions;
namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJDependency : Dependency
    {
        [Header("Scriptable Object")]
        [SerializeField] private BEID01IntroDialogMJConfigScriptableObject configScriptableObject;
        [Space(5)]
        [Header("Canvas")]
        [SerializeField] private Canvas backgroundCanvas;
        [SerializeField] private Canvas gameplayCanvas;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image charatersImage;
        [Space(5)]
        [Header("Transform")]
        [SerializeField] private Transform backgroundParent;
        [SerializeField] private Transform guidingParent;
        [Space(5)]
        [Header("Game Object")]
        [SerializeField] private Image blurPanel;
        [SerializeField] private Image layerFade;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject answerPrefab;
        [SerializeField] private GameObject questionPrefab;
        [SerializeField] private GameObject guidingPrefab;
        [Space(5)]
        [Header("References")]
        [SerializeField] private BEID01MJConversationContainer conversationContainer;
        [Space(5)]
        [Header("Partical System")]
        [SerializeField] private UIParticle uIParticle;
        [SerializeField] private List<ParticleSystem> fireworks;
        [Space(5)]
        [Header("Characters")]
        [SerializeField] private GameObject charactersParent;
        [SerializeField] private SkeletonGraphic charatersAnim;
        [SerializeField] private string animIdle;
        [SerializeField] private string animIdleBlink;
        [SerializeField] private string animTalk;

        public override T GetStateData<T>()
        {
            T data;

            Type listType = typeof(T);
            if (listType == typeof(BEID01IntroDialogMJInitStateDependency))
            {
                BEID01IntroDialogMJInitStateDependency initStateDependency = new BEID01IntroDialogMJInitStateDependency();
                initStateDependency.BackgroundParent = backgroundParent;
                initStateDependency.CharactersParent = charactersParent;
                initStateDependency.CharatersAnim = charatersAnim;
                initStateDependency.AnswerPrefab = answerPrefab;
                initStateDependency.QuestionPrefab = questionPrefab;
                initStateDependency.ConversationContainer = conversationContainer;
                initStateDependency.GameplayCanvas = gameplayCanvas;
                initStateDependency.NextButton = nextButton;
                initStateDependency.BackgroundImage = backgroundImage;
                initStateDependency.CharatersImage = charatersImage;
                data = ConvertToType<T>(initStateDependency);
            }
            else if (listType == typeof(BEID01IntroDialogMJIntroStateDependency))
            {
                BEID01IntroDialogMJIntroStateDependency introStateDependency = new BEID01IntroDialogMJIntroStateDependency();
                introStateDependency.CharactersParent = charactersParent;
                introStateDependency.BlurPanel = blurPanel;
                introStateDependency.ConversationContainer = conversationContainer;
                introStateDependency.IntroConfig = configScriptableObject.introConfig;
                introStateDependency.CharatersAnim = charatersAnim;
                introStateDependency.AnimIdle = animIdleBlink;
                introStateDependency.AnimTalk = animTalk;
                data = ConvertToType<T>(introStateDependency);
            }
            else if (listType == typeof(BEID01IntroDialogMJPlayStateDependency))
            {
                BEID01IntroDialogMJPlayStateDependency playStateDependency = new BEID01IntroDialogMJPlayStateDependency();
                playStateDependency.ConversationContainer = conversationContainer;
                playStateDependency.PlayConfig = configScriptableObject.playConfig;
                playStateDependency.NextButton = nextButton;
                playStateDependency.LayerFade = layerFade;
                playStateDependency.UIParticle = uIParticle;
                playStateDependency.Fireworks = fireworks;
                data = ConvertToType<T>(playStateDependency);
            }
            else if (listType == typeof(BEID01IntroDialogMJListenStateDependency))
            {
                BEID01IntroDialogMJListenStateDependency listenStateDependency = new BEID01IntroDialogMJListenStateDependency();
                listenStateDependency.ListenConfig = configScriptableObject.listenConfig;
                listenStateDependency.CharactersParent = charactersParent;
                listenStateDependency.CharatersAnim = charatersAnim;

                data = ConvertToType<T>(listenStateDependency);
            }
            else if (listType == typeof(BEID01IntroDialogMJListenAgainStateDependency))
            {
                BEID01IntroDialogMJListenAgainStateDependency listenAgainStateDependency = new BEID01IntroDialogMJListenAgainStateDependency();
                listenAgainStateDependency.CharactersParent = charactersParent;
                listenAgainStateDependency.CharatersAnim = charatersAnim;
                data = ConvertToType<T>(listenAgainStateDependency);
            }
            else if (listType == typeof(BEID01IntroDialogMJReviewStateDependency))
            {
                BEID01IntroDialogMJReviewStateDependency reviewStateDependency = new BEID01IntroDialogMJReviewStateDependency();
                reviewStateDependency.ConversationContainer = conversationContainer;
                data = ConvertToType<T>(reviewStateDependency);
            }
            else
            {
                data = ConvertToType<T>(null);
            }

            return data;
        }
    }
}