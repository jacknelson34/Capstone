using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;

[RunInstaller(true)] // This tells the installer to execute this class
public class InstallerActions : Installer
{
    public override void Install(IDictionary stateSaver)
    {
        base.Install(stateSaver);

        // Install ODBC Driver
        InstallODBCDriver();
        ConfigureODBCDriver();
    }

    public override void Commit(IDictionary savedState)
    {
        base.Commit(savedState);

        // Perform any final setup or logging
        LogEvent("Installation committed successfully.");
    }

    public override void Rollback(IDictionary savedState)
    {
        base.Rollback(savedState);

        // Remove installed driver if rollback is triggered
        LogEvent("Installation rolled back.");
    }

    public override void Uninstall(IDictionary savedState)
    {
        base.Uninstall(savedState);

        // Remove configuration if necessary
        LogEvent("Application uninstalled successfully.");
    }

    private void InstallODBCDriver()
    {
        // Locate the installer in the same directory as the application
        string installerPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "msodbcsql.msi");

        if (!File.Exists(installerPath))
        {
            throw new InstallException("ODBC Driver installer not found at: " + installerPath);
        }

        Process process = new Process();
        process.StartInfo.FileName = installerPath;
        process.StartInfo.Arguments = "/quiet /norestart";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        try
        {
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InstallException($"ODBC Driver installation failed with exit code {process.ExitCode}.");
            }
        }
        catch (Exception ex)
        {
            throw new InstallException("ODBC Driver installation failed: " + ex.Message);
        }
    }

    private void ConfigureODBCDriver()
    {
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI", true))
        {
            if (key == null)
                throw new InstallException("ODBC registry path not found.");

            using (RegistryKey driverKey = key.CreateSubKey("ODBC Driver 18 for SQL Server"))
            {
                driverKey.SetValue("Driver", @"C:\Program Files\Microsoft ODBC Driver 18 for SQL Server\msodbcsql18.dll");
                driverKey.SetValue("Setup", @"C:\Program Files\Microsoft ODBC Driver 18 for SQL Server\msodbcsql18.dll");
            }
        }
    }


    private void LogEvent(string message)
    {
        string logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "install.log");
        File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
    }
}
