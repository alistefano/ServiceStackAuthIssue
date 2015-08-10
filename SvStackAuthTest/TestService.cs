using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace SvStackAuthTest
{
    [Route("/test/{Msg}")]
    public class TestRequest :IReturn<TestResponse>
    {
        public string Msg { get; set; }
    }

    public class TestResponse
    {
        public string Msg { get; set; }
    }
   
    public class TestService : Service
    {
        [Authenticate]
        public object Any(TestRequest req)
        {
            return new TestResponse() {Msg = req.Msg};
        }
    }
}
