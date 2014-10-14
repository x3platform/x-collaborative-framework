namespace X3Platform.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Web;
    using System.Web.Caching;

    public class NavigationSerialization
    {
        private static string path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["navigation.config"]);
        
        public static string Path
        {
            get { return path; }
            set { path = HttpContext.Current.Server.MapPath(value); }
        }

        public static NavigationMap Load()
        {
            NavigationMap map;

            if (HttpContext.Current.Cache.Get("NavigationMap") == null)
            {
                CacheDependency dependency = new CacheDependency(path);

                XmlSerializer ser = new XmlSerializer(typeof(NavigationMap));

                FileStream stream = new FileStream(path, FileMode.Open);

                map = (NavigationMap)ser.Deserialize(stream);

                stream.Close();

                HttpContext.Current.Cache.Insert("NavigationMap", map, dependency);
            }
            else
            {
                map = (NavigationMap)HttpContext.Current.Cache.Get("NavigationMap");

            }
            

            return map;
        }

        public static void Save(NavigationMap map)
        {
            StreamWriter stream = new StreamWriter(path);

            XmlSerializer ser = new XmlSerializer(typeof(NavigationMap));

            ser.Serialize(stream, map);

            stream.Close();
        }
    }
}
