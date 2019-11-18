﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using WebExpress.Settings;

namespace WebExpress
{
    /// <summary>
    /// Klasse zum loggen von Ereignissen in deine Log-Datei
    /// 
    /// Das Programm schreibt eine Vielzahl von Informationen in eine Ereignisprotokolldatei (Log). Das 
    /// Protokoll wird im Log-Verzeichnis gespeichert. Der Name besteht aus dem Datum und der Endung „.log“. 
    /// Der Aufbau ist so konzipiert, dass die Logdatei mit einem Texteditor gelesen und analysiert werden 
    /// kann. Fehlermeldungen und Hinweise werden im Protokoll persistent (dauernd) verfügbar gemacht, somit 
    /// eigenen sich die Ereignisprotokolldateien für die Fehleranalyse und zur Überprüfung der korrekten 
    /// Arbeitsweise des Programms.
    /// Das Protokoll ist in Tabellenform organisiert. In der ersten Spalte wird die Urzeit angegeben. 
    /// Die zweite Spalte legt die Ebene des Log-Eintrages fest. In der dritten Spalte wird die Funktion 
    /// genannt, welche den Eintrag produzierte. In der letzten Spalte wird ein Hinweis oder eine 
    /// Fehlerbeschreibung angegeben.
    /// </summary>
    /// <example>
    /// <b>Beispiel:</b><br>
    /// 08:26:30 Info      Program.Main                   Programmstart<br>
    /// 08:26:30 Info      Program.Main                   --------------------------------------------------<br>
    /// 08:26:30 Info      Program.Main                   Programmversion: 0.0.0.1<br>
    /// 08:26:30 Info      Program.Main                   Argumente: -test <br>
    /// 08:26:30 Info      Program.Main                   Konfigurationsversion: V1<br>
    /// 08:26:30 Info      Program.Main                   Verarbeitung: sequentiell<br>
    /// 08:26:30 Info      Program.Main                   Imagefile wurde importiert.<br>
    /// </example>
    public class Log
    {
        /// <summary>
        /// Aufzählung definiert die verschiedenen Log-Level
        /// </summary>
        public enum Level { Info, Warning, FatalError, Error, Exception, Debug, Seperartor };

        /// <summary>
        /// Aufzählungen zum LogModus
        /// </summary>
        public enum Modus { Append, Override };

        /// <summary>
        /// Liefert oder Setzt das Encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Logeintrag
        /// </summary>
        public class LogItem
        {
            /// <summary>
            /// Level des Eintrages
            /// </summary>
            private readonly Level m_level;

            /// <summary>
            /// Instanz (Ort)
            /// </summary>
            private readonly string m_instance;

            /// <summary>
            /// Nachricht
            /// </summary>
            private readonly string m_message;

            /// <summary>
            /// Timestamp
            /// </summary>
            private readonly DateTime m_timestamp;

            /// <summary>
            /// Konstruktor
            /// </summary>
            /// <param name="level">Level</param>
            /// <param name="instance">Modul/Funktion</param>
            /// <param name="message">Nachricht</param>
            public LogItem(Level level, string instance, string message, string timePattern)
            {
                m_level = level;
                m_instance = instance;
                m_message = message;
                m_timestamp = DateTime.Now;
                TimePattern = timePattern;
            }

            /// <summary>
            /// Konvertiert den Wert dieser Instanz in eine Zeichnkette
            /// </summary>
            /// <returns>Dieser String</returns>
            public override string ToString()
            {
                if (m_level != Level.Seperartor)
                {
                    return m_timestamp.ToString(TimePattern) + " " + m_level.ToString().PadRight(9, ' ') + " " + m_instance.PadRight(19, ' ').Substring(0, 19) + " " + m_message;
                }
                else
                {
                    return m_message;
                }
            }

            /// <summary>
            /// Liefert den Level des Eintrages
            /// </summary>
            public Level Level => m_level;

            /// <summary>
            /// Liefert die Instanz (Ort)
            /// </summary>
            public string Instance => m_instance;

            /// <summary>
            /// Liefert die Nachricht
            /// </summary>
            public string Message => m_message;

            /// <summary>
            /// Liefert den Timestamp
            /// </summary>
            public DateTime Timestamp => m_timestamp;


            /// <summary>
            /// Zeitmusterfür Logeinträge festlegen
            /// </summary>
            public string TimePattern { set; get; }
        };

        private static Log m_this = new Log();
        private string m_path;
        private Thread m_workerThread;
        private const int m_seperatorWidth = 130;

        /// <summary>
        /// Lebenszyklus des Workerthreads beenden
        /// </summary>
        private bool m_done = false;

        /// <summary>
        /// Warteschlange für noch nicht gespeicherte Einträge
        /// </summary>
        private readonly Queue<LogItem> m_queue = new Queue<LogItem>();

        /// <summary>
        /// Log-Kopie
        /// </summary>
        private StringBuilder m_logCopy = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Log()
        {
            Encoding = Encoding.UTF8;
            FilePattern = "yyyyMMdd";
            TimePattern = "yyyMMddHHmmss";
            LogModus = Log.Modus.Append;
        }

        /// <summary>
        /// Beginnt mit dem Loggen
        /// </summary>
        /// <param name="path">Der Pfad indem die Datei erstellt wird</param>
        public void Begin(string path, string name)
        {
            Filename = (path.EndsWith(Path.DirectorySeparatorChar.ToString()) ? path : path + Path.DirectorySeparatorChar) + name;

            m_path = path;
            m_logCopy = new StringBuilder();

            // Verzeichnis Überprüfen
            if (!Directory.Exists(m_path))
            {
                // Noch kein Log-Verzeichnis vorhanden -> erstellen
                Directory.CreateDirectory(m_path);
            }

            // Vorhandene Logdatei löschen wenn Modus überschreiben aktiv ist
            if (LogModus == Modus.Override)
            {
                try
                {
                    File.Delete(Filename);
                }
                catch
                {
                }
            }

            // Thread erstellen
            m_workerThread = new Thread(new ThreadStart(ThreadProc))
            {

                // Hintergrundthread
                IsBackground = true
            };

            // Ausführung des Workerthreads beginnen
            m_workerThread.Start();
        }

        /// <summary>
        /// Beginnt mit dem Loggen
        /// </summary>
        /// <param name="path">Der Pfad indem die Datei erstellt wird</param>
        public void Begin(string path)
        {
            Begin(path, DateTime.Today.ToString(FilePattern) + ".log");
        }

        /// <summary>
        /// Beginnt mit dem Loggen
        /// </summary>
        /// <param name="settings">Die Logeinstellungen</param>
        public void Begin(SettingLogItem settings)
        {
            Filename = settings.Filename;
            LogModus = (Modus)Enum.Parse(typeof(Modus), settings.Modus);
            Encoding = Encoding.GetEncoding(settings.Encoding);
            TimePattern = settings.Timepattern;

            Begin(settings.Path, Filename);
        }

        /// <summary>
        /// Fügt eine Nachricht dem Log hinzu.
        /// </summary>
        /// <param name="level">Das Level</param>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        protected virtual void Add(Level level, string instance, string message)
        {
            foreach (var l in message.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var item = new LogItem(level, instance, l, TimePattern);
                //Console.WriteLine(item.ToString().Length > m_seperatorWidth ? item.ToString().Substring(0, m_seperatorWidth - 3) + "..." : item.ToString());

                lock (m_queue)
                {
                    m_queue.Enqueue(item);

                    if (m_logCopy != null)
                    {
                        m_logCopy.AppendLine(item.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Fügt eine Nachricht dem Log hinzu.
        /// </summary>
        /// <param name="level">Das Level</param>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        protected virtual void Add(Level level, MethodBase instance, string message)
        {
            LogItem item = null;

            foreach (var l in message.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
#if DEBUG
                item = new LogItem(level, (instance != null) ? (instance.DeclaringType.Name + "." + instance.Name) : "", l, TimePattern);
#else
                if (level == Level.Debug) return;
                item = new LogItem(level, (instance != null) ? instance.DeclaringType.Name : "", l, TimePattern);
#endif

                //#if DEBUG
                //Console.WriteLine(item.ToString().Length > m_seperatorWidth ? item.ToString().Substring(0, m_seperatorWidth - 3) + "..." : item.ToString());
                //#endif

                lock (m_queue)
                {
                    m_queue.Enqueue(item);

                    if (m_logCopy != null)
                    {
                        m_logCopy.AppendLine(item.ToString());
                    }
                }

            }
        }

        /// <summary>
        /// Eine Trennlinie mit * Zeichen
        /// </summary>
        public void Seperator()
        {
            Seperator('*');
        }

        /// <summary>
        /// Eine Trennlinie mit benutzerdefinierten Zeichen
        /// </summary>
        /// <param name="sepChar">Die Nachricht</param>
        public void Seperator(char sepChar)
        {
            Add(Level.Seperartor, (string)null, "".PadRight(m_seperatorWidth, sepChar));
        }

        /// <summary>
        /// Logt eine Info-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Loggnachricht</param>
        public void Info(MethodBase instance, string message)
        {
            Add(Level.Info, instance, message);
        }

        /// <summary>
        /// Logt eine Info-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Loggnachricht</param>
        public void Info(string instance, string message)
        {
            Add(Level.Info, instance, message);
        }

        /// <summary>
        /// Logt eine Warnungs-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Warning(MethodBase instance, string message)
        {
            Add(Level.Warning, instance, message);

            WarningCount++;
        }

        /// <summary>
        /// Logt eine Warnungs-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Warning(string instance, string message)
        {
            Add(Level.Warning, instance, message);

            WarningCount++;
        }

        /// <summary>
        /// Logt eine Fehler-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Error(MethodBase instance, string message)
        {
            Add(Level.Error, instance, message);

            ErrorCount++;
        }

        /// <summary>
        /// Logt eine Fehler-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Error(string instance, string message)
        {
            Add(Level.Error, instance, message);

            ErrorCount++;
        }


        /// <summary>
        /// Logt eine Fehler-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void FatalError(MethodBase instance, string message)
        {
            Add(Level.FatalError, instance, message);

            ErrorCount++;
        }

        /// <summary>
        /// Logt eine Exception-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="ex">Ausnahmebeschreibung</param>
        public void Exception(MethodBase instance, Exception ex)
        {
            lock (m_queue)
            {
                Add(Level.Exception, instance, ex.Message.Trim());
#if DEBUG
                Add(Level.Exception, instance, ex.StackTrace != null ? ex.StackTrace.Trim() : ex.Message.Trim());
#endif
                ExceptionCount++;
                ErrorCount++;
            }
        }

        /// <summary>
        /// Logt eine Exception-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="ex">Ausnahmebeschreibung</param>
        public void Exception(string instance, string ex)
        {
            lock (m_queue)
            {
                Add(Level.Exception, instance, ex);

                ExceptionCount++;
                ErrorCount++;
            }
        }

        /// <summary>
        /// Logt eine Debug-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Debug(MethodBase instance, string message)
        {
            Add(Level.Debug, instance, message);
        }

        /// <summary>
        /// Logt eine Debug-Nachricht
        /// </summary>
        /// <param name="instance">Methode, die loggen möchte</param>
        /// <param name="message">Die Nachricht</param>
        public void Debug(string instance, string message)
        {
            Add(Level.Debug, instance, message);
        }

        /// <summary>
        /// Beendet das Logging
        /// </summary>
        public void Close()
        {
            m_done = true;

            // Dateischreiben vor nebenläufigen Zugriff schützen
            lock (m_path)
            {
                Flush();
            }
        }

        /// <summary>
        /// Bereinigt das Log
        /// </summary>
        public void Clear()
        {
            ErrorCount = 0;
            WarningCount = 0;
            ExceptionCount = 0;

            if (LogLines != null)
            {
                LogLines.Clear();
            }
        }

        /// <summary>
        /// Schreibt den Inhalt der Warteschlange in dem Log
        /// </summary>
        public void Flush()
        {
            var list = new List<LogItem>();

            // Warteschlange vor nebenläufigen Zugriff sperren
            lock (m_queue)
            {
                list.AddRange(m_queue);
                m_queue.Clear();
            }

            // Dateischreiben vor nebenläufigen Zugriff schützen
            if (list.Count > 0)
            {
                lock (m_path)
                {
                    using (var fs = new System.IO.FileStream(Filename, FileMode.Append))
                    {
                        using (var w = new System.IO.StreamWriter(fs, Encoding))
                        {
                            foreach (var item in list)
                            {
                                var str = item.ToString();
                                w.WriteLine(str);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Threadstart-Funktion
        /// </summary>
        private void ThreadProc()
        {
            while (!m_done)
            {
                Thread.Sleep(5000);

                // Dateischreiben vor nebenläufigen Zugriff schützen
                lock (m_path)
                {
                    Flush();
                }
            }

            m_workerThread = null;
        }

        /// <summary>
        /// Zeitmuster des Dateinamens festlegen
        /// </summary>
        public string FilePattern { set; get; }

        /// <summary>
        /// Zeitmusterfür Logeinträge festlegen
        /// </summary>
        public string TimePattern { set; get; }

        /// <summary>
        /// Liefert das aktuelle Log-Objekt 
        /// </summary>
        public static Log Current
        {
            get => m_this;
            protected set => m_this = value;
        }

        /// <summary>
        /// Liefert den Dateinamen des Log
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Liefert die Log-Einträge
        /// </summary>
        public StringBuilder LogLines => m_logCopy;

        /// <summary>
        /// Liefert oder setzt die Anzahl der Ausnahmen (Exceptions)
        /// </summary>
        public int ExceptionCount { get; protected set; }

        /// <summary>
        /// Liefert oder setzt die Anzahl der Fehler (Fehler + Exceptions)
        /// </summary>
        public int ErrorCount { get; protected set; }

        /// <summary>
        /// Liefert oder setzt die Anzahl der Warnungen
        /// </summary>
        public int WarningCount { get; protected set; }

        /// <summary>
        /// Prüft ob das Log zum Schreiben geöffnet wurde
        /// </summary>
        public bool IsOpen => m_workerThread != null;

        /// <summary>
        /// Setzt oder liefert den LogModus
        /// </summary>
        public Modus LogModus { get; set; }
    }
}
