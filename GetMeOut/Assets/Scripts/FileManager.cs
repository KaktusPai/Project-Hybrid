using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{

    public void UploadSaveValues(float MusicVolume, float MainVolume)
    {
        var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        Directory.CreateDirectory(appdata + "/GetMeOut_Game");
        string path = appdata + "/GetMeOut_Game/Options.txt";

        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine(Mathf.RoundToInt(MusicVolume * 100));
        writer.WriteLine(Mathf.RoundToInt(MainVolume * 100));
        writer.Close();
    }
    public returnValues ReadSaveValues()
    {
        try
        {
            var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string path = appdata + "/GetMeOut_Game/Options.txt";
            StreamReader reader = new StreamReader(path);
            string MUvolume = reader.ReadLine();
            string volume = reader.ReadLine();
            reader.Close();
            return new returnValues(int.Parse(MUvolume), int.Parse(volume));
        }
        catch
        {
            createDefault();

            return new returnValues(20,50);
        }
    }

    private void createDefault()
    {
        var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        Directory.CreateDirectory(appdata + "/GetMeOut_Game");
        string path = appdata + "/GetMeOut_Game/Options.txt";
        StreamWriter writer = new StreamWriter(path, false);

        writer.WriteLine(20);
        writer.WriteLine(50);
        writer.Close();
    }

    public int readMusic()
    {
        try
        {
            var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string path = appdata + "/GetMeOut_Game/Options.txt";
            StreamReader reader = new StreamReader(path);
            string MUvolume = reader.ReadLine();
            string volume = reader.ReadLine();
            string difficulty = reader.ReadLine();
            reader.Close();
            return int.Parse(MUvolume);
        }
        catch
        {
            createDefault();

            return 20;
        }

    }

    public int readMain()
    {
        try
        {
            var appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            string path = appdata + "/GetMeOut_Game/Options.txt";
            StreamReader reader = new StreamReader(path);
            string MUvolume = reader.ReadLine();
            string volume = reader.ReadLine();
            string difficulty = reader.ReadLine();
            reader.Close();
            return int.Parse(volume);
        }
        catch
        {
            createDefault();

            return 50;
        }

    }

    public class returnValues
    {
        public int MU_Volume;
        public int MA_Volume;

        public returnValues(int MUSIC, int MAINVOL)
        {
            MA_Volume = MAINVOL;
            MU_Volume = MUSIC;
        }
    }
}
