using HtmlAgilityPack;
using scraper.dsScraperTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scraper
{
    class Program
    {
        static void scrape(string[] args)
        {
            majestic_millionTableAdapter majestic_Million = new majestic_millionTableAdapter();
            DataTable dt = majestic_Million.GetData();
            //List<string> RestrictedLinks = new List<string> { "adobe.com" , "docs.google.com", "wordpress.com", "apps.apple.com" };
            foreach (DataRow row in dt.Rows)
            {
                string html = row[0].ToString();
                //if (RestrictedLinks.Contains(html))
                //{
                //    continue;
                //}
                html = "http://" + html;
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = null;
                try
                {
                    htmlDoc = web.Load(html);

                }
                catch
                {
                    continue;
                }

                if (htmlDoc.DocumentNode.SelectSingleNode("//head/title") == null)
                {
                    continue;
                }
                var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title").InnerText;
                var desc = htmlDoc.DocumentNode.SelectNodes("//head/meta");
                string description = null;
                string keywords = null;
                if(desc != null)
                {
                    foreach (HtmlNode meta in desc)
                    {
                        if (meta.Attributes.Count > 0)
                        {
                            if (meta.Attributes[0].Value == "description" && meta.Attributes.Count > 1)
                            {
                                description = meta.Attributes[1].Value;
                            }
                            else if (meta.Attributes[0].Value == "keywords" && meta.Attributes.Count > 1)
                            {
                                keywords = meta.Attributes[1].Value;
                            }
                        }

                    }
                }
                
                string title = node;
                wwwIndexTableAdapter index = new wwwIndexTableAdapter();
                index.Insert(title, description, keywords,html);

                //var html = "http://" + row[0].ToString(); ;

                //HtmlWeb web = new HtmlWeb();

                //HtmlDocument htmlDoc = web.Load(html);
                //var node = htmlDoc.DocumentNode;
                //var title = htmlDoc.DocumentNode.SelectNodes("/html/head")[0];

                //Console.WriteLine(title);
            }


        }
    }
}
