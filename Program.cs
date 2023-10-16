using StereoKit;

namespace stereokit_lynx_template;

class Program
{
	static void Main(string[] args)
	{
		// Initialize StereoKit
		SKSettings settings = new()
		{
			appName = "StereoKit Lynx Template",
			assetsFolder = "Assets",
			blendPreference = DisplayBlend.Blend,
			// overlayApp = true,
			// overlayPriority = 1,
			depthMode = DepthMode.D32,
		};
		if (!SK.Initialize(settings))
			return;

		// Create assets used by the app
		Pose windowPose = new(0, 0, -0.5f, Quat.FromAngles(0f, 180.0f, 0f));

		World.OcclusionEnabled = true;

		// Core application loop
		SK.Run(() =>
		{
			UI.WindowBegin("Application", ref windowPose);

			if (UI.Button("AR"))
			{
				Device.DisplayBlend = DisplayBlend.Blend;
			}
			UI.SameLine();
			if (UI.Button("VR"))
			{
				Device.DisplayBlend = DisplayBlend.Opaque;
			}

			UI.WindowEnd();
		});
	}
}