using BepInEx;
using BepInEx.NET.Common;
using BepInExResoniteShim;
using HarmonyLib;
using FrooxEngine;
using FrooxEngine.UIX;

namespace MoreReferenceProxies
{
    [ResonitePlugin(PluginMetadata.GUID, PluginMetadata.NAME, PluginMetadata.VERSION, PluginMetadata.AUTHORS, PluginMetadata.REPOSITORY_URL)]
    [BepInDependency(BepInExResoniteShim.PluginMetadata.GUID)]
    public class MoreReferenceProxies : BasePlugin
	{
		public override void Load() => HarmonyInstance.PatchAll();

		[HarmonyPatch]
		class MoreReferenceProxiesPatch
		{
			[HarmonyPostfix]
			[HarmonyPatch(typeof(SyncMemberEditorBuilder), "BuildBag")]
			public static void BuildBagPostFix(ISyncBag bag, UIBuilder ui)
			{
				BuildProxy(bag, ui);
			}
			
			[HarmonyPostfix]
			[HarmonyPatch(typeof(SyncMemberEditorBuilder), "BuildList")]
			public static void BuildListPostFix(ISyncList list, UIBuilder ui)
			{
				BuildProxy(list, ui);
			}

			[HarmonyPostfix]
			[HarmonyPatch(typeof(UserInspectorItem), "RebuildUser")]
			public static void RebuildUserPostFix(UserInspectorItem __instance, SyncRef<User> ____user)
			{
				__instance.Slot[0].AttachComponent<ReferenceProxySource>().Reference.Target = ____user.Target;
				__instance.Slot[1][1][0].AttachComponent<ReferenceProxySource>().Reference.Target = (WorkerBag<UserComponent>)AccessTools.Field(typeof(User), "componentBag").GetValue(____user.Target);
				for (int i = 0; i < ____user.Target.StreamGroupManager.Groups.Count; i++)
				{
					__instance.Slot[1][1][i + 1].AttachComponent<ReferenceProxySource>().Reference.Target = (StreamBag)AccessTools.Field(typeof(User), "streamBag").GetValue(____user.Target);
				}
			}
		}
		private static void BuildProxy(IWorldElement target, UIBuilder ui)
		{
			Slot textSlot = ui.Current[0];
			textSlot.AttachComponent<ReferenceProxySource>().Reference.Target = target;
			InteractionElement.ColorDriver colorDriver = textSlot.AttachComponent<Button>().ColorDrivers.Add();
			Text text = textSlot.GetComponent<Text>();
			colorDriver.ColorDrive.Target = text.Color;
			colorDriver.NormalColor.Value = RadiantUI_Constants.TEXT_COLOR;
            colorDriver.HighlightColor.Value = RadiantUI_Constants.LABEL_COLOR;
            colorDriver.PressColor.Value = RadiantUI_Constants.HEADING_COLOR;
        }
	}
}