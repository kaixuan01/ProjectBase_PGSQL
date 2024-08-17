using System.ComponentModel;

namespace Utils.Enums
{
    public enum Enum_UserRole 
    {
        [Description("Administrator")]
        Admin = 0,

        [Description("Tester")]
        Tester = 1,

        [Description("Developer")]
        Developer = 2,

        [Description("Normal User")]
        NormalUser = 3
    }
}