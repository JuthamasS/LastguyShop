using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace LastguyShop.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistrationMember()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            return View();
        }

        public IActionResult FileManagement()
        {
            return View();
        }

        public IActionResult SaveToFile(string text)
        {
            string _dirname = Directory.GetCurrentDirectory();
            string _filepath = Path.Combine(_dirname, "Storage\\FileText\\");
            var _guid = Guid.NewGuid().ToString("N");

            if (!Directory.Exists(_filepath))
            {
                Directory.CreateDirectory(_filepath);
            }

            StreamWriter sw = new StreamWriter(_filepath + _guid + ".txt");
            sw.WriteLine(text);
            sw.Close();

            return RedirectToAction("ListOfTextFile");
        }
        public IActionResult ListOfTextFile()
        {
            List<string> list = new List<string>();
            return RedirectToAction("FileManagement",list);
        }

        public IActionResult BarCode()
        {
            return View();
        }

        public IActionResult ReadBarCode()
        {
            //Barcode barCode = new Barcode();
            //var img = (Image)barCode.Encode(BarcodeStandard.Type.Code39,"256987",250,100);
            //var data = ConvertImageToBytes(img);
            return View();
        }

        //public byte[] ConvertImageToBytes()
        //{
        //    //using (MemoryStream ms = new MemoryStream())
        //    //{
        //    //    img.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
        //    //    return ms.ToArray();
        //    //}
        //}

        public IActionResult WriteBarCode(string code)
        {
            

            return View();
        }
    }
}
