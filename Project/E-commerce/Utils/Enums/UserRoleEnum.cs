using System.ComponentModel;

namespace Utils.Enums
{
    public enum UserRoleEnum
    {
        [Description("Administrator")]
        Admin = 1,

        [Description("Tester")]
        Tester = 2,

        [Description("Developer")]
        Developer = 3,

        [Description("Normal User")]
        NormalUser = 4
    }
}
