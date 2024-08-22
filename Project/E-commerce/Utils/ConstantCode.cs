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
    }
}
