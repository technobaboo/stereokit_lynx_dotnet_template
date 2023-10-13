using StereoKit;

namespace sk_cs_test;

class Program
{
	static void Main(string[] args)
	{
		// Backend.OpenXR.ExcludeExt("XR_MSFT_unbounded_reference_space");
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



		// Create assets used by the app
		Pose cubePose = new(0, 0, -0.5f);
		Model cube = Model.FromMesh(
			Mesh.GenerateRoundedCube(Vec3.One * 0.1f, 0.02f),
			Material.UI);

		Matrix floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
		Material floorMaterial = new("floor.hlsl")
		{
			Transparency = Transparency.Blend
		};

		World.OcclusionEnabled = true;




		// Core application loop
		SK.Run(() =>
		{
			if (SK.System.displayType == Display.Opaque)
				Mesh.Cube.Draw(floorMaterial, floorTransform);

			UI.Handle("Cube", ref cubePose, cube.Bounds);
			cube.Draw(cubePose.ToMatrix());
		});
	}
}