﻿using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;
[SupportedOSPlatform("windows")]
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FrmPrincipal());
    }
}
