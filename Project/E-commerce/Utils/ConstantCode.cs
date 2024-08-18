using System.Collections.Generic;

namespace Utils
{
    public class ConstantCode
    {
        public class Module
        {
            public const string User = "User";
            public const string SystemConfig = "System Config";
        }
        public class Action
        {
            public const string Create = "Create";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
            public const string View = "View";
        }
        public class UserStatus
        {
            public const string Active = "Active";
            public const string Blocked = "Blocked";
        }
        public class SystemConfig_Key
        {
            public const string MaxLoginFailedAttempt = "MaxLoginFailedAttempt";
        }

        public class TableName
        {
            public const string SystemConfig = "T_SystemConfig";
            public const string User = "T_User";
            public const string UserLoginHistory = "T_UserLoginHistory";
            public const string UserRole = "E_UserRole";
            public const string AuditTrail = "T_AuditTrail";
            public const string AuditTrailDetails = "T_AuditTrailDetails";
        }
    }
}
