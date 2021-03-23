using System;

foreach(string f in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
{
    var proc = System.Diagnostics.Process.Start("WpfApp1.exe", "\"" + f + '\"');
    //"WpfApp1.exe \"" +f+'\"';

}