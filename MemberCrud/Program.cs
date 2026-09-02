using MemberCrud.Services;

namespace MemberCrud
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Create the application-scoped service instances here (composition root)
            IMemberService memberService = new MemberService();

            Application.Run(new ChurchManagement(memberService));
        }
    }
}
