using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Functions
{
    public class MB
    {
        #region Enums
        public enum Farbe
        {
            Gruen,
            Rot,
            Schwarz,
            Weiß,
            Blau,
            Hellblau,
            Coral,
            Transparent
        }

        public enum Pointer
        {
            Minute,
            Hour
        }
        #endregion //Enums

        #region Structures
        #endregion //Structures

        #region Variables
        #endregion //Variables

        #region Constructors
        public MB()
        {

        }
        #endregion //Constructors

        #region Public
        /// <summary>
        /// Überprüft ob ein bestimmter Prozess läuft
        /// </summary>
        /// <param name="process">Gesuchter Prozess</param>
        /// <returns>Prozess läuft</returns>
        public bool isProcess_Running(string process)
        {
            bool isRunning = false;

            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.ToLower().Trim(' ') == process.ToLower().Trim(' '))
                {
                    isRunning = true;
                    break;
                }
            }

            return isRunning;
        }

        /// <summary>
        /// Anpingen einer IP Adresse
        /// </summary>
        /// <param name="ip">IP Adresse</param>
        /// <returns>Ping Ergebnis</returns>
        public IPStatus PingIP(string ip)
        {
            IPStatus status = IPStatus.Unknown;

            Ping sender = new Ping();

            PingOptions options = new PingOptions();
            options.DontFragment = true;

            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;

            try
            {
                PingReply reply = sender.Send(ip, timeout, buffer, options);

                status = reply.Status;
            }
            catch (Exception e)
            {
                status = IPStatus.Unknown;
            }

            return status;
        }

        #region String-Operations
        /// <summary>
        /// Erzeugt ein Byte-Array aus einem Hex-String
        /// </summary>
        /// <param name="hexString">Hex-Code</param>
        /// <returns>Byte-Array</returns>
        public byte[] Get_BytesFromHexString(string hexString)
        {
            byte[] buffer = (from x in Enumerable.Range(0, hexString.Length)
                             where x % 2 == 0
                             select Convert.ToByte(hexString.Substring(x, 2), 16)).ToArray();

            return buffer;
        }

        /// <summary>
        /// Erzeugt einen Hex-String aus einem Byte-Array
        /// </summary>
        /// <param name="buffer">Byte-Array</param>
        /// <returns>Hex-String</returns>
        public string Get_HexStringFromBytes(byte[] buffer)
        {
            StringBuilder hexString = new StringBuilder(buffer.Length);

            for (int i = 0; i < buffer.Length - 1; i++)
                hexString.Append(buffer[i].ToString("X2"));

            return hexString.ToString();
        }

        /// <summary>
        /// Vergleicht zwei Eingangsstrings auf Gleichheit
        /// </summary>
        /// <param name="a">Text a</param>
        /// <param name="b">Text b</param>
        /// <returns>a == b ?</returns>
        public bool Compare_Text(string a, string b)
        {
            return string.Equals(a, b);
        }

        /// <summary>
        /// Vergleicht den Inhalt von zwei Byte-Arrays auf Gleichheit
        /// </summary>
        /// <param name="a">Array a</param>
        /// <param name="b">Array b</param>
        /// <returns>a == b ?</returns>
        public bool Compare_Bytes(byte[] a, byte[] b)
        {
            bool equal = false;

            string arrA = Encoding.UTF8.GetString(a);
            string arrB = Encoding.UTF8.GetString(b);

            equal = string.Equals(arrA, arrB);

            return equal;
        }
        #endregion //String-Operations

        #region Colorization
        /// <summary>
        /// Ändert die Farbeigenschaft. Zur Auswahl steht eine Reihe vordefinierter Farben.
        /// </summary>
        /// <param name="farbe"></param>
        /// <returns>Brush Color</returns>
        public Brush SetColor(Farbe farbe)
        {
            //Grün    : #FF00C300   Rot     : #FFFF0000     Schwarz : #FF000000     Weiß        : #FFFFFFFF
            //Blau    : #FF013498   Hellblau: #330080FF     Coral   : #FFFF7F50     Transparent : #00000000
            SolidColorBrush brush = new SolidColorBrush();
            Color color = new Color();

            switch (farbe)
            {
                case Farbe.Gruen:
                    color = (Color)ColorConverter.ConvertFromString("#FF00C300");
                    break;
                case Farbe.Rot:
                    color = (Color)ColorConverter.ConvertFromString("#FFFF0000");
                    break;
                case Farbe.Schwarz:
                    color = (Color)ColorConverter.ConvertFromString("#FF000000");
                    break;
                case Farbe.Weiß:
                    color = (Color)ColorConverter.ConvertFromString("#FFFFFFFF");
                    break;
                case Farbe.Blau:
                    color = (Color)ColorConverter.ConvertFromString("#FF013498");
                    break;
                case Farbe.Hellblau:
                    color = (Color)ColorConverter.ConvertFromString("#330080FF");
                    break;
                case Farbe.Coral:
                    color = (Color)ColorConverter.ConvertFromString("#FFFF7F50");
                    break;
                case Farbe.Transparent:
                    color = (Color)ColorConverter.ConvertFromString("#00000000");
                    break;
                default:
                    color = (Color)ColorConverter.ConvertFromString("#FF000000");
                    break;
            }

            brush.Color = color;

            return brush;
        }

        /// <summary>
        /// Ändert die Farbeigenschaft.
        /// </summary>
        /// <param name="hexColor">Hex-Farbcode: alpha rgb</param>
        /// <returns>Brush Color</returns>
        public Brush SetColor(string hexColor)
        {
            //Grün    : #FF00C300   Rot     : #FFFF0000     Schwarz : #FF000000     Weiß        : #FFFFFFFF
            //Blau    : #FF013498   Hellblau: #330080FF     Coral   : #FFFF7F50     Transparent : #00000000
            SolidColorBrush brush = new SolidColorBrush();
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);

            brush.Color = color;

            return brush;
        }
        #endregion //Colorization

        #region Math
        /// <summary>
        /// Umrechnen von Bogenmaß in Grad
        /// </summary>
        /// <param name="rad">Bogenmaß</param>
        /// <returns>Grad</returns>
        public double Rad2Deg(double rad)
        {
            return 180 / Math.PI * rad;
        }

        /// <summary>
        /// Umrechnen von Grad in Bogenmaß
        /// </summary>
        /// <param name="deg">Grad</param>
        /// <returns>Bogenmaß</returns>
        public double Deg2Rad(double deg)
        {
            return Math.PI * deg / 180;
        }

        /// <summary>
        /// Dreht den errechneten Winkel um den Mittelpunkt und einem zweiten Punkt in
        /// ein Karthesisches Koordinatensystem.
        /// </summary>
        /// <param name="angle">Winkel</param>
        /// <param name="midpoint">Obejekt Mittelpunkt</param>
        /// <param name="here">Punkt im Fenster</param>
        /// <returns>Korrigierter Winkel</returns>
        public double AngleCorrection(double angle, Point midpoint, Point here)
        {
            double _angle = angle;

            if (here.X == midpoint.X && here.Y < midpoint.Y)
            { _angle = 270; }
            else if (here.X > midpoint.X && here.Y == midpoint.Y)
            { _angle = 360; }
            else if (here.X == midpoint.X && here.Y > midpoint.Y)
            { _angle = 90; }
            else if (here.X < midpoint.X && here.Y == midpoint.Y)
            { _angle = 180; }
            else if (here.X < midpoint.X && here.Y < midpoint.Y)
            { _angle += 180; }
            else if (here.X < midpoint.X && here.Y > midpoint.Y)
            { _angle += 180; }
            else if (here.X > midpoint.X && here.Y < midpoint.Y)
            { _angle += 360; }
            else if (here.X > midpoint.X && here.Y > midpoint.Y)
            { _angle += 0; }
            _angle = Math.Abs(_angle - 360);

            return _angle;
        }

        /// <summary>
        /// Rechnet den Winkel um in eine Zeitangabe.
        /// </summary>
        /// <param name="_mid">Bezug Mittelpunkt</param>
        /// <param name="_pointer">Zeiger</param>
        /// <param name="_count">Skaleneinteilung</param>
        /// <param name="_arrow">Zeigertyp</param>
        /// <returns>Zeitangabe [Minute / Stunde]</returns>
        public int Calc_TimeFromAngle(Point _mid, Line _pointer, int _count, Pointer _arrow)
        {
            Point P = new Point(_pointer.X2, _pointer.Y2);

            double offset = 360 / _count;
            double m = (_pointer.Y2 - _mid.Y) / (_pointer.X2 - _mid.X);
            double α = Rad2Deg(Math.Atan(m));

            α = AngleCorrection(α, _mid, P);
            α = RotateTransform(α, -90, true);

            switch (_arrow)
            {
                case Pointer.Hour:
                    return Convert.ToInt16(Math.Round(α / offset, 0));
                case Pointer.Minute:
                    return Convert.ToInt16(Math.Round(α / _count * 10, 0));
                default:
                    return Convert.ToInt16(Math.Round(α, 0));
            }
        }

        /// <summary>
        /// Ausgangswinkel α um einen Rotationswinkel drehen.
        /// Hinweis: Der Rotationswinkel ist vorzeichenbehaftet.
        /// Das Ergebnis kann mit der Funktion gespiegelt werden.
        /// </summary>
        /// <param name="_α">Ausgangswinkel</param>
        /// <param name="_rotAngle">Drehwinkel</param>
        /// <param name="_transform">Spiegeln?</param>
        /// <returns>Winkel α</returns>
        public double RotateTransform(double _α, double _rotAngle, bool _transform)
        {
            double α;

            //Drehen um Winkel
            α = _α + _rotAngle <= 0 ? _α + _rotAngle : _α + _rotAngle - 360;

            //Spiegeln
            if (_transform)
                α = Math.Abs(α);

            return α;
        }
        #endregion //Math

        #region File-IO
        /// <summary>
        /// Speichert Daten an den angegebenen Dateipfad.
        /// </summary>
        /// <param name="directoryPath">Verzeichnis</param>
        /// <param name="fileName">Dateiname</param>
        /// <param name="extension">Datei Erweiterung</param>
        /// <param name="content">Datei Inhalt</param>
        public void SaveTo(string directoryPath, string fileName, string extension, string content)
        {
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string path = $"{directoryPath}\\{fileName}.{extension}";

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);
            sw.Close();

            fs.Close();

            sw.Dispose();
            fs.Dispose();
        }

        /// <summary>
        /// Speichert Daten an den angegebenen Dateipfad.
        /// </summary>
        /// <param name="path">Dateipfad</param>
        /// <param name="content">Datei Inhalt</param>
        public void SaveTo(string path, string content)
        {
            if (!Directory.Exists(Get_DirectoryFromPath(path)))
                Directory.CreateDirectory(Get_DirectoryFromPath(path));

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);
            sw.Close();

            fs.Close();

            sw.Dispose();
            fs.Dispose();
        }

        /// <summary>
        /// Liest Daten aus der angegebenen Datei.
        /// </summary>
        /// <param name="directoryPath">Verzeichnis</param>
        /// <param name="fileName">Dateiname</param>
        /// <param name="extension">Datei Erweiterung</param>
        /// <returns></returns>
        public string ReadFrom(string directoryPath, string fileName, string extension)
        {
            string content = string.Empty;

            string path = $"{directoryPath}\\{fileName}.{extension}";

            if(File.Exists(path))
            {
                FileStream fs = new FileStream(path , FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                while (sr.Peek() != -1)
                    content += $"{sr.ReadLine()}";

                sr.Close();
                fs.Close();

                sr.Dispose();
                fs.Dispose();
            }

            return content;
        }

        /// <summary>
        /// Liest Daten aus der angegebenen Datei.
        /// </summary>
        /// <param name="path">Dateipfad</param>
        /// <returns>Datei Inhalt</returns>
        public string ReadFrom(string path)
        {
            string content = string.Empty;

            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);

                while (sr.Peek() != -1)
                    content += $"{sr.ReadLine()}";

                sr.Close();
                fs.Close();

                sr.Dispose();
                fs.Dispose();
            }

            return content;
        }
        #endregion //File-IO

        #region Encoding
        /// <summary>
        /// Erstellt echte Zufallswerte.
        /// </summary>
        /// <param name="count">Legt die Anzahl der ASCII Zeichen fest</param>
        /// <param name="min">Legt die kleinste ASCII Zeichen fest</param>
        /// <param name="max">Legt das größte ASCII Zeichen fest</param>
        /// <param name="NoUse">Auszuschließende ASCII Zeichen</param>
        /// <returns></returns>
        public byte[] Random_Values(int count, int min, int max, [Optional] byte[] NoUse)
        {
            byte[] RV = new byte[count];

            try
            {
                RNGCryptoServiceProvider rcsp = new RNGCryptoServiceProvider();
                rcsp.GetBytes(RV);

                double divisor = 256F / (max - min + 1);
                if (min > 0 || max < 255)
                {
                    for (int i = 0; i < count; i++)
                    {
                        RV[i] = (byte)((RV[i] / divisor) + min);

                        if (NoUse != null)
                        {
                            foreach (byte by in NoUse)
                            {
                                if (RV[i] == by)
                                    RV[i] = (byte)((RV[i] / divisor) + min + 1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RV = new byte[32] { 32 + 1, 32 + 2, 32 + 3, 32 + 4, 32 + 5, 32 + 6, 32 + 7, 32 + 8, 32 + 9, 32 + 10, 32 + 11, 32 + 12, 32 + 13, 32 + 14, 32 + 15, 32 + 16, 32 + 17, 32 + 18, 32 + 19, 32 + 20, 32 + 21, 32 + 22, 32 + 23, 32 + 24, 32 + 25, 32 + 26, 32 + 27, 32 + 28, 32 + 29, 32 + 30, 32 + 31, 32 + 32 };
            }

            return RV;
        }

        /// <summary>
        /// Erzeugt einen Hashwert aus der Zeichenfolge
        /// </summary>
        /// <param name="value">Text</param>
        /// <param name="isHexString">Gibt an ob der Text Hexadezimal codiert ist</param>
        /// <returns>Hash</returns>
        public string Get_Hash(string value, bool isHexString = false)
        {
            string hash = string.Empty;

            if(isHexString)
                hash = Get_HexStringFromBytes(new MD5CryptoServiceProvider().ComputeHash(Get_BytesFromHexString(value)));
            else
                hash = Get_HexStringFromBytes(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value)));

            return string.Empty;
        }

        /// <summary>
        /// Erzeugt einen Hashwert aus der Zeichenfolge
        /// </summary>
        /// <param name="value">Text</param>
        /// <returns>Hash</returns>
        public string Get_Hash(byte[] value)
        {
            return Get_HexStringFromBytes(new MD5CryptoServiceProvider().ComputeHash(value));
        }
        #endregion //Encoding

        #endregion //Public

        #region Private
        /// <summary>
        /// Extrahiert den Verzeichnispfad aus der Prfadangabe
        /// </summary>
        /// <param name="path">Dateipfad</param>
        /// <returns></returns>
        string Get_DirectoryFromPath(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }
        #endregion //Private
    }
}