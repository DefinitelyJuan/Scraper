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
    class imgScraper
    {
        static void Main(string[] args)
        {
            majestic_millionTableAdapter majestic_Million = new majestic_millionTableAdapter();
            DataTable dt = majestic_Million.GetData();
            foreach (DataRow row in dt.Rows)
            {
                string html = row[0].ToString();
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

                if (htmlDoc.DocumentNode.Descendants("img") == null)
                {
                    continue;
                }
                List<HtmlNode> imgs = htmlDoc.DocumentNode.Descendants("img").ToList();

                imgIndexTableAdapter index = new imgIndexTableAdapter();
                foreach (HtmlNode img in imgs)
                {
                    string imgSrc = "", imgAlt ="";
                    if (img.Attributes.Count > 0)
                    {
                        if (img.Attributes["src"] != null)
                        {
                            if (img.Attributes["src"].Value != "")
                                imgSrc = img.Attributes["src"].Value;
                            else
                                continue;
                        }
                        else
                        {
                            continue;
                        }
                        if (img.Attributes["alt"] != null)
                        {
                            if (img.Attributes["alt"].Value != "")
                                imgAlt = img.Attributes["alt"].Value;
                            else
                                continue;
                        }
                        else
                        {
                            continue;
                        }
                        if (imgSrc.First() == '/')
                            imgSrc = html + imgSrc;
                        else if (imgSrc.StartsWith("data"))
                            continue;

                        index.Insert(imgSrc, imgAlt);
                        Console.WriteLine($"src:{imgSrc}\nalt:{imgAlt}\n");
                    }
                    
                }


            }


        }
    }
}
