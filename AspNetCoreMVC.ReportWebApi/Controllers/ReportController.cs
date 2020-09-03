using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC.ReportWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private static readonly List<string> Report = new List<string>();
            
        // GET: api/Report
        [HttpGet]
        public string Get()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Report)
            {
                stringBuilder.AppendLine(item);
            }
            return stringBuilder.ToString();
        }

        // POST: api/Report
        // [Authorize]
        [HttpPost]
        public void Post([FromBody] string value)
        {
            Report.Add(value);
        }
    }
}
