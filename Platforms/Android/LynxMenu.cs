using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using StereoKit;
using StereoKit.Framework;
using Color = StereoKit.Color;

namespace stereokit_lynx_template;

public class LynxMenu : IStepper
{

    public bool open = false;
    public bool Enabled { get { return open; } }

    private string? label;
    private Sprite? icon;

    private Pose pose = new(0, 0, -0.5f, Quat.FromAngles(0f, 180.0f, 0f));

    public LynxMenu() { }

    public bool Initialize()
    {
        this.label = GetApplicationName();
        this.icon = GetApplicationIcon();

        return true;
    }

    public void Shutdown() { }

    public void Step()
    {
        if (!open)
        {
            return;
        }
        UI.WindowBegin(this.label ?? "Unknown App Name", ref pose, new Vec2(0.3f, 0.1f + UI.Settings.margin + UI.Settings.margin));

        UI.LayoutPushCut(UICut.Left, 0.1f, false);
        UI.Image(this.icon ?? Sprite.ToggleOn, new Vec2(0.1f, 0.1f));
        UI.LayoutPop();

        UI.LayoutPushCut(UICut.Bottom, UI.LineHeight);
        if (UI.Button("Exit"))
        {
            SK.Quit();
        }
        UI.SameLine();
        if (UI.Button("Resume"))
        {
            this.open = false;
        }
        UI.LayoutPop();

        UI.WindowEnd();
    }

    public static string? GetApplicationName()
    {
        ApplicationInfo? applicationInfo = Application.Context.ApplicationInfo;
        int? stringId = applicationInfo?.LabelRes;
        return stringId != null ? Application.Context.GetString((int)stringId) : applicationInfo?.NonLocalizedLabel?.ToString();
    }
    public static Sprite? GetApplicationIcon()
    {
        ApplicationInfo? applicationInfo = Application.Context.ApplicationInfo;
        PackageManager? pm = Application.Context.PackageManager;
        int? icon = applicationInfo?.Icon;
        Bitmap? appIconBitmap = icon.HasValue ? BitmapFactory.DecodeResource(Application.Context.Resources, icon.Value) : null;
        if (appIconBitmap == null)
        {
            return null;
        }
        Tex tex = Tex.GenColor(Color.Black, appIconBitmap.Width, appIconBitmap.Height, TexType.Image, TexFormat.Rgba32);
        Sprite sprite = Sprite.FromTex(tex, SpriteType.Single);
        return sprite;
    }
}