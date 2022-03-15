using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webserver.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class bitcoin : ControllerBase
    {
        public class CryptoPrice
        {
            public string BTC;
            public string ETC;
            public string DOGE;
            public string MATIC;
            public string GLM;
            public string WEMIX;
            public string KRW;

            public CryptoPrice()
            {
                BTC = "";
                ETC = "";
                DOGE = "";
                MATIC = "";
                GLM = "";
                WEMIX = "";
                KRW = "";
            }
        }

        // GET 
        [HttpGet()]
        public string Get()
        {
            var list = new List<CryptoPrice>();

            CryptoPrice price = new CryptoPrice();
            // loop on columns

            price.BTC = "";
            price.ETC = "";
            price.DOGE = "";
            price.MATIC = "";
            price.GLM = "";
            price.WEMIX = "";
            price.KRW = "";

            list.Add(price);

            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }
    }
}
