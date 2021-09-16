using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using BaseX;
using FrooxEngine.UIX;

namespace MoreReferenceProxies
{
	public class MoreReferenceProxies : NeosMod
	{
		public override string Name => "MoreReferenceProxies";
		public override string Author => "eia485";
		public override string Version => "1.0.0";
		public override string Link => "https://github.com/EIA485/NeosMoreReferenceProxies/";
		public override void OnEngineInit()
		{
			Harmony harmony = new Harmony("net.eia485.MoreReferenceProxies");
			harmony.PatchAll();
		}

		[HarmonyPatch(typeof(SyncMemberEditorBuilder))]
		class MoreReferenceProxiesPatch
		{
			[HarmonyPostfix]
			[HarmonyPatch("BuildBag")]
			public static void BuildBagPostFix(ISyncBag bag, UIBuilder ui)
			{
				BuildProxy(bag, ui);
			}
			[HarmonyPostfix]
			[HarmonyPatch("BuildList")]
			public static void BuildListPostFix(ISyncList list, UIBuilder ui)
			{
				BuildProxy(list, ui);
			}
		}
		private static void BuildProxy(IWorldElement target, UIBuilder ui)
		{
			Slot textSlot = ui.Current[0];
			textSlot.AttachComponent<ReferenceProxySource>().Reference.Target = target;
			InteractionElement.ColorDriver colorDriver = textSlot.AttachComponent<Button>().ColorDrivers.Add();
			Text text = textSlot.GetComponent<Text>();
			colorDriver.ColorDrive.Target = text.Color;
			colorDriver.NormalColor.Value = color.Black;
			colorDriver.HighlightColor.Value = color.Blue;
			colorDriver.PressColor.Value = color.Blue;
		}
	}
}