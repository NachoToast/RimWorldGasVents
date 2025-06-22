using HarmonyLib;
using System.Reflection;

namespace GasVents;

public static class WidgetExtensions
{
    private static readonly FieldInfo ROW_CUR_X = AccessTools.Field(typeof(WidgetRow), "curX");

    private static readonly FieldInfo ROW_CUR_Y = AccessTools.Field(typeof(WidgetRow), "curY");

    public static WidgetRow MakeRow(this Listing listing)
    {
        WidgetRow row = new(0f, listing.CurHeight);

        listing.Gap(Text.LineHeight);

        return row;
    }

    public static void TextArea(this WidgetRow row, ref string text, float minWidth = 50f, float paddingY = 8f)
    {
        float width = Mathf.Max(minWidth, Text.CalcSize(text).x) + 2f * paddingY;

        text = Widgets.TextArea(row.MakeRect(width), text).Replace(",", "");
    }

    private static Rect MakeRect(this WidgetRow row, float width)
    {
        float curX = (float)ROW_CUR_X.GetValue(row);
        float curY = (float)ROW_CUR_Y.GetValue(row);

        ROW_CUR_X.SetValue(row, curX + width);

        return new Rect(curX, curY, width, Text.LineHeight);
    }
}
