using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
#if UNITY_4_1_2
using System.Xml;
#else
using System.Xml.Linq;
#endif
using UnityEngine;
#if !UNITY_WINRT
using XElement = System.Xml.XmlNode;
using XDocument = System.Xml.XmlDocument;
using XmlNodeList = System.Xml.XmlNodeList;
#else
using System.Linq;
using XmlNodeList = System.Collections.Generic.IEnumerable<System.Xml.Linq.XElement>;
#endif
namespace Extentions
{
    public static class PlatformExtentions
    {
        public static Type GetInterfaceEx(this Type type, string className)
        {
#if UNITY_WINRT
			return type.GetInterface(className, false);
#else
            return type.GetInterface(className);
#endif
        }

        public static XElement SelectSingleNodeEx(this XElement node, string nodeName)
        {
#if UNITY_WP8
			return node.Element(nodeName);
#else
            return node.SelectSingleNode(nodeName);
#endif
        }

        public static XElement SelectSingleNodeEx(this XDocument doc, string nodeName)
        {
#if UNITY_WP8
			return doc.Element(nodeName);
#else
            return doc.SelectSingleNode(nodeName);
#endif
        }

        public static XmlNodeList SelectNodesEx(this XElement node, string nodeName)
        {
#if UNITY_WP8
			return node.Elements(nodeName);
#else
            return node.SelectNodes(nodeName);
#endif
        }

        public static XElement ElementAtEx(this XmlNodeList nodeList, int i)
        {
#if UNITY_WP8
			return nodeList.ElementAt(i);
#else
            return nodeList[i];
#endif
        }

        public static string InnerText(this XElement element)
        {
#if UNITY_WP8
			return element.Value;
#else
            return element.InnerText;
#endif
        }
    }

    public static class Application
    {
        public static void OpenURL(string url)
        {
#if UNITY_WP8
			WP8Statics.FireOpenUrl(url);
#else
            UnityEngine.Application.OpenURL(url);
#endif
        }
    }

    public static class WP8Statics
    {
        public static event EventHandler OpenUrlHandle;

        public static void FireOpenUrl(string url)
        {
            if (OpenUrlHandle != null)
            {
                OpenUrlHandle(url, null);
            }
        }
    }
}

