using Microsoft.AspNetCore.Mvc;


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
