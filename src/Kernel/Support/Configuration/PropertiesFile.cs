// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

namespace X3Platform.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>.ini �����ļ�</summary>
    public class PropertiesFile
    {
        private SortedList sections = new SortedList();

        private string m_FileName;

        public string FileName
        {
            get { return this.m_FileName; }
            set { this.m_FileName = value; }
        }

        public PropertiesFile(string fileName)
        {
            this.Load(fileName);
        }

        public void Load(string fileName)
        {
            this.m_FileName = fileName;

            if (File.Exists(fileName))
            {
                string s1, s2, key, value;

                int index;

                string sectionName = string.Empty;

                using (StreamReader reader = new StreamReader(fileName, ASCIIEncoding.Default))
                {
                    SortedList section = null;

                    sections.Clear();

                    while (reader.Peek() >= 0)
                    {
                        s1 = reader.ReadLine();
                        s2 = s1.Trim();

                        if (s2.Length < 2)
                            continue;

                        if (s2[0] == '[' && s2[s2.Length - 1] == ']')
                        {
                            //
                            //
                            //

                            //new section
                            s1 = s2.Substring(1, s2.Length - 2).Trim();
                            if (s1.Length == 0)
                                continue;

                            sectionName = s1;

                            section = sections[sectionName] as System.Collections.SortedList;

                            if (section == null)
                            {
                                section = new System.Collections.SortedList();
                                sections.Add(sectionName, section);
                            }

                            continue;
                        }
                        else
                        {
                            //
                            // ��ȡ key=value
                            //

                            //old section
                            if (sectionName == string.Empty)
                                continue;

                            index = s1.IndexOf("=");

                            if (index < 0)
                            {
                                //no key or value
                                continue;
                            }

                            key = s1.Substring(0, index).Trim();
                            value = s1.Substring(index + 1, s1.Length - index - 1);

                            if (key.Length == 0 || value.Length == 0)
                            {
                                continue;
                            }

                            if (section[key] == null)
                            {
                                section.Add(key, value);
                            }
                            else
                            {
                                section[key] = value;
                            }
                        }
                    }

                    reader.Close();
                }
            }
        }

        public void Save(string fileName)
        {
            SortedList section;

            string sectionName, key, v;
            string sectionFmt = "[{0}]\r\n";
            string keyFmt = "{0}={1}\r\n";

            System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            try
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, System.Text.ASCIIEncoding.Default);
                try
                {
                    for (int i = 0; i < sections.Count; i++)
                    {
                        sectionName = (string)sections.GetKey(i);
                        writer.Write(string.Format(sectionFmt, sectionName));

                        section = sections.GetByIndex(i) as System.Collections.SortedList;
                        for (int j = 0; j < section.Count; j++)
                        {
                            key = (string)section.GetKey(j);
                            v = (string)section.GetByIndex(j);
                            writer.Write(string.Format(keyFmt, key, v));
                        }
                    }
                }
                finally
                {
                    writer.Close();
                }
            }
            finally
            {
                stream.Close();
            }
        }

        public void Update()
        {
            this.Save(this.FileName);
        }

        public string Read(string sectionName, string key)
        {
            SortedList section = sections[sectionName] as SortedList;

            if (section == null)
            {
                return string.Empty;
            }
            else
            {
                object obj = section[key];

                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)obj;
                }
            }
        }

        public void Write(string sectionName, string key, string value)
        {
            sectionName = sectionName.Trim();

            key = key.Trim();

            if (sectionName == string.Empty || key == string.Empty)
            {
                return;
            }

            SortedList section = sections[sectionName] as SortedList;

            if (section == null)
            {
                section = new SortedList();
                sections.Add(sectionName, section);
            }

            object obj = section[key];

            if (obj == null)
            {
                section.Add(key, value);
            }
            else
            {
                section[key] = value;
            }
        }

        public void Remove(string sectionName, string key)
        {
            SortedList section = sections[sectionName] as SortedList;

            if (section == null)
                return;

            section.Remove(key);
        }

        //
        // ����
        //
        public void EraseSection(string sectionName)
        {
            sections.Remove(sectionName);
        }
    }
}
