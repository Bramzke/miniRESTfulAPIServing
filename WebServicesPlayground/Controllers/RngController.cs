using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebServicesPlayground.Controllers
{
    public class RngController : Controller
    {
        static Random rnd = new Random();
        public IActionResult StartPage()
        {
            return View();
        }

        public IActionResult GetRandomNumbers(string apikey, int cnt, int min, int max, string responseformat, int rowlength = 6, bool distinct = false)
        {

            if (max + 1 - min < rowlength) rowlength = max + 1 - min;

            if (apikey == "hallowelt")
            {
                List<int> rndNums = new List<int>();
                int n = cnt / rowlength;
                if (cnt % rowlength != 0)
                {
                    n++;
                }

                for (int i = 0; i < n; i++)
                {
                    List<int> partL = new List<int>();
                    while (partL.Count < rowlength)
                    {
                        int neu = rnd.Next(min, max + 1);
                        if (!partL.Contains(neu))
                        {
                            partL.Add(neu);
                        }
                    }
                    rndNums.AddRange(partL);
                }

                rndNums = rndNums.Take(cnt).ToList();

                if (responseformat.ToLower() == "json")
                {
                    return Json(rndNums);
                }
                else if (responseformat.ToLower() == "html")
                {
                    return View(rndNums);
                }
                else if (responseformat.ToLower() == "xml")
                {
                    string xmlString = SerializeResultAsXML(rndNums);

                    return Content(xmlString);
                }
                else
                {
                    return StatusCode(400, "not supported format");
                }

            }
            else
            {
                return StatusCode(400, "wrong api-key!");
            }
        }

        String SerializeResultAsXML(List<int> randomNumberList)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<int>));
            //var subReq = new MyObject();
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, randomNumberList);
                    xml = sww.ToString(); // Your XML
                    return xml;
                }
            }
        }



    }
}
