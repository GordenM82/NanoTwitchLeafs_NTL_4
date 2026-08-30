#if NTL4_MIGRATION
using log4net;
using NanoTwitchLeafs.Interfaces;
using NanoTwitchLeafs.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NanoTwitchLeafs.Controller
{
    /// <summary>
    /// Lokale, datenbankfreie Triggerablage für NTL 4.
    /// Jede Änderung wird atomar geschrieben; die vorherige Datei bleibt als
    /// Sicherung erhalten.
    /// </summary>
    public sealed class JsonTriggerController : IDatabaseController<TriggerSetting>
    {
        private readonly object _sync = new object();
        private readonly string _path;
        private readonly string _backupPath;
        private readonly ILog _logger = LogManager.GetLogger(typeof(JsonTriggerController));

        public JsonTriggerController(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _backupPath = path + ".backup";
            CreateTable();
        }

        public bool Exists => File.Exists(_path);

        public void CreateTable()
        {
            string directory = Path.GetDirectoryName(_path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public List<TriggerSetting> Load()
        {
            lock (_sync)
            {
                return ReadAll().Select(Clone).ToList();
            }
        }

        public void Save(TriggerSetting entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            lock (_sync)
            {
                List<TriggerSetting> triggers = ReadAll();
                if (entity.ID <= 0 || triggers.Any(item => item.ID == entity.ID))
                {
                    entity.ID = triggers.Count == 0 ? 1 : triggers.Max(item => item.ID) + 1;
                }

                triggers.Add(Clone(entity));
                WriteAll(triggers);
            }
        }

        public void Update(TriggerSetting entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            lock (_sync)
            {
                List<TriggerSetting> triggers = ReadAll();
                int index = triggers.FindIndex(item => item.ID == entity.ID);
                if (index < 0)
                {
                    throw new InvalidOperationException($"Trigger mit ID {entity.ID} wurde nicht gefunden.");
                }

                triggers[index] = Clone(entity);
                WriteAll(triggers);
            }
        }

        public void Delete(TriggerSetting entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            lock (_sync)
            {
                List<TriggerSetting> triggers = ReadAll();
                triggers.RemoveAll(item => item.ID == entity.ID);
                WriteAll(triggers);
            }
        }

        public void ClearTable()
        {
            lock (_sync)
            {
                WriteAll(new List<TriggerSetting>());
            }
        }

        private List<TriggerSetting> ReadAll()
        {
            if (!File.Exists(_path))
            {
                return new List<TriggerSetting>();
            }

            try
            {
                string json = File.ReadAllText(_path);
                return JsonConvert.DeserializeObject<List<TriggerSetting>>(json) ?? new List<TriggerSetting>();
            }
            catch (Exception exception)
            {
                _logger.Error($"Triggerdatei konnte nicht gelesen werden: {_path}", exception);
                throw new InvalidDataException(
                    "Die lokale Triggerdatei konnte nicht gelesen werden. Die Sicherungsdatei wurde nicht verändert.",
                    exception);
            }
        }

        private void WriteAll(List<TriggerSetting> triggers)
        {
            string temporaryPath = _path + ".tmp";
            string json = JsonConvert.SerializeObject(triggers, Formatting.Indented);
            File.WriteAllText(temporaryPath, json);

            if (File.Exists(_path))
            {
                File.Copy(_path, _backupPath, true);
            }

            File.Move(temporaryPath, _path, true);
        }

        private static TriggerSetting Clone(TriggerSetting trigger)
        {
            string json = JsonConvert.SerializeObject(trigger);
            return JsonConvert.DeserializeObject<TriggerSetting>(json);
        }
    }
}
#endif
