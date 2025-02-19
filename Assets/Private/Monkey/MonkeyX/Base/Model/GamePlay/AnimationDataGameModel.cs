

using Spine.Unity;

public class AnimationDataGameModel 
{
    public int ID { get; private set; }
    public SkeletonDataAsset SkeletonDataAsset { get; private set; }

    public AnimationDataGameModel(int iD, SkeletonDataAsset skeletonDataAsset)
    {
        ID = iD;
        SkeletonDataAsset = skeletonDataAsset;
    }
}
