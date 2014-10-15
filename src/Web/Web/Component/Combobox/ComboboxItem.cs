

namespace X3Platform.Web.Component.Combobox
{
    using System;

    /// <summary>×éºÏ¿òÏî</summary>
    public class ComboboxItem
    {
        private string m_Text = string.Empty;

        public string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        private string m_Value = string.Empty;

        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        private bool m_Selected = false;

        public bool Selected
        {
            get { return m_Selected; }
            set { m_Selected = value; }
        }

        public ComboboxItem()
        {
        }

        public ComboboxItem(string value)
        {
            string[] item = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (item.Length < 2)
            {
                Text = value;
                Value = value;
            }
            else
            {
                Text = item[1];
                Value = item[0];
            }
        }

        public ComboboxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return "{\"text\":\"" + this.Text + "\",\"value\":\"" + this.Value + "\",\"selected\":\"" + this.Selected + "\"}";
        }
    }
}