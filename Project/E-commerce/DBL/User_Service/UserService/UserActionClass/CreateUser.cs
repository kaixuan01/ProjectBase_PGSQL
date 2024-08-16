﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.User_Service.UserService.UserActionClass
{
    public class CreateUser_REQ
    {
        public string? name { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? email { get; set; }
        public int? userRoleId { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }

    }

    public class CreateUser_RESP
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}