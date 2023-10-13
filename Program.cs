using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using StereoKit;

namespace sk_cs_test;

class Program
{
	static void Main(string[] args)
	{
		Backend.OpenXR.ExcludeExt("XR_MSFT_unbounded_reference_space");
		// Initialize StereoKit
		SKSettings settings = new()
		{
			appName = "sk_cs_test",
			assetsFolder = "Assets",
			blendPreference = DisplayBlend.Blend,
			// overlayApp = true,
			// overlayPriority = 1,
			depthMode = DepthMode.D32,
		};
		if (!SK.Initialize(settings))
			return;


		// Get the PackageManager instance
		// PackageManager packageManager = Application.Context.PackageManager;

		// Get a list of all installed applications
		// IList<PackageInfo> installedPackages = packageManager.GetInstalledPackages(0);

		// Create assets used by the app
		Pose appListPose = new(0, 0, -0.5f, Quat.FromAngles(0f, 180.0f, 0f));
		Model cube = Model.FromMesh(
			Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
			Material.UI);

		World.OcclusionEnabled = true;

		// Core application loop
		SK.Run(() =>
		{
			UI.WindowBegin("Information", ref appListPose);

			UI.Label("Left Hand:" + (Input.Hand(Handed.Left).IsTracked ? "Tracked" : "Untracked"));
			UI.Label("Right Hand:" + (Input.Hand(Handed.Right).IsTracked ? "Tracked" : "Untracked"));

			// foreach (PackageInfo info in installedPackages)
			// {
			// 	UI.Label(info.PackageName ?? "Unknown");
			// }

			UI.WindowEnd();
		});
	}
}