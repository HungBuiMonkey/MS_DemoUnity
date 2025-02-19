using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02StarEffect : MonoBehaviour
    {
        private List<Image> starImages;
        private bool isBlinking = false;

        private void Start()
        {
            starImages = GetComponentsInChildren<Image>().ToList();
            OnStarEffect();
        }


        private void OnStarEffect()
        {
            isBlinking = true;
            StartCoroutine(BlinkRandomStars());
        }
        private IEnumerator BlinkRandomStars()
        {
            while (isBlinking)
            {
                if (!isBlinking) break;
                int numOfStarsToBlink = Random.Range(2, 4);
                List<Image> selectedStars = new List<Image>();

                for (int i = 0; i < numOfStarsToBlink; i++)
                {
                    Image selectedStar = GetRandomStar(selectedStars);
                    selectedStars.Add(selectedStar);
                    selectedStar.DOFade(0f, 1.5f).SetLoops(-1, LoopType.Yoyo);
                }

                yield return new WaitForSeconds(1.5f);
            }
        }
        private Image GetRandomStar(List<Image> selectedStars)
        {
            List<Image> unselectedStars = new List<Image>(starImages);

            foreach (Image selectedStar in selectedStars)
            {
                unselectedStars.Remove(selectedStar);
            }

            return unselectedStars[Random.Range(0, unselectedStars.Count)];
        }

        private void OnDestroy()
        {
            isBlinking = false;
        }

    }

}