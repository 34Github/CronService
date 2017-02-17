using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GMS.Core.Config
{
    [Serializable]
    public class UrlConfig : ConfigFileBase
    {
        public UrlConfig()
        { }

        public UrlInfo[] Urls { get; set; }

    }

    [Serializable]
    public class UrlInfo
    {   
        [XmlAttribute("name")]
        public string Name { get;set;}

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("schedule")]
        public string Schedule { get; set; }


        [XmlAttribute("ready")]
        public bool Ready { get; set; }

    }
}