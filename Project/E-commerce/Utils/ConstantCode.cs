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
            public const string EnableSendEmail_Background = "EnableSendEmail_Background";
            public const string SendEmailIntervalSec_Background = "SendEmailIntervalSec_Background";
            public const string SendEmailTotalRetry_Background = "SendEmailTotalRetry_Background";
            
        }

        public class TableName
        {
            public const string SystemConfig = "T_SystemConfig";
            public const string User = "T_User";
            public const string UserLoginHistory = "T_UserLoginHistory";
            public const string UserRole = "E_UserRole";
            public const string AuditTrail = "T_AuditTrail";
            public const string AuditTrailDetails = "T_AuditTrailDetails";
            public const string Email = "T_Email";
        }

        public class Resource
        {
            public class EmailTemplateDesign
            {
                public const string ConfirmEmailTemplate = "DBL.Resource.EmailTemplateDesign.ConfirmEmailTemplate.html";
            }
        }

        public class EmailPlaceholder
        {
            public const string RecipientName = "RecipientName";
            public const string ConfirmEmailUrl = "ConfirmEmailUrl";
        }

        public class UrlPath
        {
            public const string ConfirmEmail = "/ConfirmEmail/{id}";
        }

        public class Status
        {
            public static readonly Dictionary<string, string> StatusDictionary = new Dictionary<string, string>
            {
                { Code_Pending, Pending },
                { Code_Completed, Completed },
                { Code_Failed, Failed }
            };

            public const string Code_Pending = "P";
            public const string Pending = "Pending";

            public const string Code_Completed = "C";
            public const string Completed = "Completed";

            public const string Code_Failed = "F";
            public const string Failed = "Failed";
        }
    }
}
