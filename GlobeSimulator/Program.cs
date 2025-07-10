namespace GlobeSimulator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.

			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += (s, e) =>
				Console.WriteLine("UI Thread Ex: " + e.Exception);
			AppDomain.CurrentDomain.UnhandledException += (s, e) =>
				Console.WriteLine("Non-UI Thread Ex: " + e.ExceptionObject);

			ApplicationConfiguration.Initialize();
           // try {
                Application.Run(new Simulator());
            //} catch (Exception ex) {
            //    Console.WriteLine(ex.ToString());
            //}
        }

	}
}