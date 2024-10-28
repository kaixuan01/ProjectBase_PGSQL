using System.ComponentModel;

namespace Utils.Enums
{
    public enum Enum_UserRole 
    {
        [Description("Admin")]
        Admin = 1,

        [Description("Merchant")]
        Merchant = 2,

        [Description("Normal User")]
        NormalUser = 3
    }
}