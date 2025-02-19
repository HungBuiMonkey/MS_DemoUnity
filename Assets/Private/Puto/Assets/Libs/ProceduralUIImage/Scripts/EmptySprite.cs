using UnityEngine;
using System.Collections;

public static class EmptySprite
{
    static Sprite instance;

    ///<summary>
    /// Returns the instance of a (1 x 1) white Spprite
    /// </summary>	
    public static Sprite Get()
    {
        if (instance == null)
        {
#if ADDRESSABLE_ENABLED
            if (AddressableLoader.Instance.HasKey("sprites/procedural_ui_image_default_sprite"))
                instance = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>("sprites/procedural_ui_image_default_sprite").WaitForCompletion();
            else
                instance = Resources.Load<Sprite>("procedural_ui_image_default_sprite");
#else
             instance = Resources.Load<Sprite>("procedural_ui_image_default_sprite");
#endif

            instance = Resources.Load<Sprite>("procedural_ui_image_default_sprite");
        }
        return instance;
    }

    public static bool IsEmptySprite(Sprite s)
    {
        if (Get() == s)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
