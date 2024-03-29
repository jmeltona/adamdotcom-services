﻿using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AdamDotCom.Whois.Service.CountryNameLookup
{
    public class CountryNameLookup
    {
        private static XmlDocument countries;
        private const string countriesFilename = @"iso_3166-1_list_en.xml";

        public CountryNameLookup()
        {
            countries = new XmlDocument();

            var resourceFileWithNamespace = string.Format("{0}.{1}", GetType().Namespace, countriesFilename);
            var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFileWithNamespace);

            if (fileStream != null)
            {
                using (var reader = new StreamReader(fileStream))
                {
                    countries.LoadXml(reader.ReadToEnd());
                }
            }
        }

        public string GetCountryName(string countryCode2)
        {
            var countryNode = countries.SelectSingleNode(string.Format(@"/ISO_3166-1_List_en/ISO_3166-1_Entry/ISO_3166-1_Alpha-2_Code_element[.=""{0}""]", countryCode2)).ParentNode;
            var countryName = countryNode.SelectSingleNode("ISO_3166-1_Country_name").InnerText.ToLower();
            
            return char.ToUpper(countryName[0]) + countryName.Substring(1);
        }
    }
}