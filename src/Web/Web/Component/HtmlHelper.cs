namespace X3Platform.Web.Component
{
    using System.Text;

    /// <summary>Html´úÂëÉú³ÉÆ÷</summary>
    public static class HtmlHelper
    {
        public static string TextBox(string id, string value)
        {
            StringBuilder outString = new StringBuilder();

            outString.Append("<input ");
            outString.AppendFormat("id=\"{0}\" name=\"{0}\" ", id);
            outString.AppendFormat("value=\"{0}\" ", value);
            outString.Append("type=\"text\" ");
            outString.Append("/>");

            return outString.ToString();
        }

        public static string TextBox(string id, string value, string attributes)
        {
            return string.Empty;
        }

        // TextAreaFor()
        // DropDownListFor()
        // CheckboxFor()
        // RadioButtonFor()
        // ListBoxFor()
        // PasswordFor()
        // HiddenFor()
        // LabelFor()
    }
}