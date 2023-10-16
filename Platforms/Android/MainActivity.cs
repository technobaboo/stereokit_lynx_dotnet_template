using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using StereoKit;
using System;
using System.Reflection;
using System.Threading;

namespace stereokit_lynx_template;

// <activity
// 	android:configChanges="screenSize|screenLayout|orientation|keyboardHidden|keyboard|navigation|uiMode|density"
// 	,
// 	android:label="@string/app_name" ,
// 	android:launchMode="singleTask" ,
// 	android:orientation="landscape" ,
// 	android:resizeableActivity="false"></activity>
[Activity(
	Label = "@string/app_name",
	MainLauncher = true,
	Exported = true,
	ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
	ResizeableActivity = false,
	LaunchMode = Android.Content.PM.LaunchMode.SingleTask,
	ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.ScreenLayout | Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Keyboard | Android.Content.PM.ConfigChanges.Navigation | Android.Content.PM.ConfigChanges.UiMode | Android.Content.PM.ConfigChanges.Density
)]
[IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { "org.khronos.openxr.intent.category.IMMERSIVE_HMD", "com.oculus.intent.category.VR", Intent.CategoryLauncher })]
public class MainActivity : Activity
{
	protected LynxMenu menu = new();

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);
		Run();
		SetContentView(Resource.Layout.activity_main);
	}

	protected override void OnDestroy()
	{
		SK.Quit();
		base.OnDestroy();
	}

	static bool running = false;
	void Run()
	{
		if (running) return;
		running = true;

		// Before anything else, give StereoKit the Activity. This should
		// be set before any other SK calls, otherwise native library
		// loading may fail.
		SK.AndroidActivity = this;

		// Temporary fix for the broken reference space (qualcomm runtime issue)
		Backend.OpenXR.ExcludeExt("XR_MSFT_unbounded_reference_space");
		// Task.Run will eat exceptions, but Thread.Start doesn't seem to.
		new Thread(InvokeStereoKit).Start();
	}

	void InvokeStereoKit()
	{
		this.menu = SK.AddStepper<LynxMenu>();

		Type entryClass = typeof(Program);
		MethodInfo? entryPoint = entryClass?.GetMethod("Main", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		// There are a number of potential method signatures for Main, so
		// we need to check each one, and give it the correct values.
		//
		// Converting MethodInfo into an Action instead of calling Invoke on
		// it allows for exceptions to properly bubble up to the IDE.
		ParameterInfo[]? entryParams = entryPoint?.GetParameters();
		if (entryParams == null || entryParams.Length == 0)
		{
			Action Program_Main = (Action)Delegate.CreateDelegate(typeof(Action), entryPoint!);
			Program_Main();
		}
		else if (entryParams?.Length == 1 && entryParams[0].ParameterType == typeof(string[]))
		{
			Action<string[]> Program_Main = (Action<string[]>)Delegate.CreateDelegate(typeof(Action<string[]>), entryPoint!);
			Program_Main(new string[] { });
		}
		else throw new Exception("Couldn't invoke Program.Main!");

		Process.KillProcess(Process.MyPid());
	}

	public override bool OnKeyUp(Keycode keyCode, KeyEvent? e)
	{
		if (e == null || e.KeyCode != Keycode.DpadCenter)
		{
			return false;
		}
		menu.open = true;
		return true;
	}

}