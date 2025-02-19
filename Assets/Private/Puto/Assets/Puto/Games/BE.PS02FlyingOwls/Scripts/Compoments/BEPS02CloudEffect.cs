using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02CloudEffect : MonoBehaviour
    {
        [SerializeField] RectTransform pointCloud;
        [SerializeField] RectTransform resetCloud;
        private List<Image> cloudImages;

        private void Start()
        {
            cloudImages = GetComponentsInChildren<Image>().ToList();
            OnCloudEffect();
        }

        private void OnCloudEffect()
        {
            foreach (var item in cloudImages)
            {
                item.transform.DOMoveX(pointCloud.position.x, 100f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    item.transform.position = resetCloud.position;
                    OnCloudEffect();
                });
            }
        }
    }
}
