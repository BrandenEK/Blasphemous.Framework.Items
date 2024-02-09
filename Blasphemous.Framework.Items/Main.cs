using BepInEx;

namespace Blasphemous.Framework.Items;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.0.2")]
public class Main : BaseUnityPlugin
{
    public static ItemFramework ItemFramework { get; private set; }

    private void Start()
    {
        ItemFramework = new ItemFramework();
    }
}
