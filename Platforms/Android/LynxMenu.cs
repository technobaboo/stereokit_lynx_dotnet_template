
using Android.App;
using Android.Content.PM;
using StereoKit;
using StereoKit.Framework;

namespace stereokit_lynx_template;

public class LynxMenu : IStepper
{

    public bool open = false;
    public bool Enabled { get { return open; } }

    private string label;

    private Pose pose = new(0, 0, -0.5f, Quat.FromAngles(0f, 180.0f, 0f));

    public LynxMenu() { }

    public bool Initialize()
    {
        this.label = GetApplicationName();
        return true;
    }

    public void Shutdown() { }

    public void Step()
    {
        if (!open)
        {
            return;
        }
        UI.WindowBegin(this.label, ref pose);

        if (UI.Button("Exit"))
        {
            SK.Quit();
        }

        UI.WindowEnd();
    }

    public static string GetApplicationName()
    {
        ApplicationInfo applicationInfo = Application.Context.ApplicationInfo;
        int stringId = applicationInfo.LabelRes;
        return stringId == 0 ? applicationInfo.NonLocalizedLabel.ToString() : Application.Context.GetString(stringId);
    }
}