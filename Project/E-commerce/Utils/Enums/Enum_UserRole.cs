using System.ComponentModel;

namespace Utils.Enums
{
    public enum Enum_UserRole 
    {
        [Description("Administrator")]
        Admin = 0,

        [Description("Normal User")]
        NormalUser = 1
    }
}