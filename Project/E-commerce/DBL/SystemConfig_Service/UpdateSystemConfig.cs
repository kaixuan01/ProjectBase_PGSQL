using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.SystemConfig_Service
{
    public class UpdateSystemConfig_REQ
    {
        public string? userId { get; set; }
        public List<SysConfig> sysConfigList { get; set; } = new List<SysConfig>();
    }

    public class UpdateSystemConfig_RESP
    {
        public string Code { get; set; }
        public string? Message { get; set; }
    }

    public class SysConfig
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
