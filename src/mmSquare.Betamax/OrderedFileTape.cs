using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace mmSquare.Betamax
{
    public class OrderedFileTape : Tape, TapeObserver
    {
        private string _tapeFolder;

        private long _position;

        private readonly XmlObjectSerializer _serializer;

        public OrderedFileTape()
            : this(TapeRootPath)
        {
        }

        public OrderedFileTape(string tapeFolder)
        {
            if (string.IsNullOrEmpty(tapeFolder))
                throw new ArgumentNullException("tapeFolder");

            _tapeFolder = tapeFolder;

            _serializer = new NetDataContractSerializer();

            _position = long.MinValue;
        }

        private const string TapeRootPath = @"RecordedCalls";
        private const string ResponseFileTypeName = @"response";
        private const string RequestFileTypeName = @"request";

        public void RecordResponse(object returnValue, Type recordedType, string methodName)
        {
            RecordResponse(returnValue, recordedType, methodName, GetToken());
        }

        public void RecordResponse(object returnValue, Type recordedType, string methodName, TapeToken token)
        {
            RecordObject(returnValue, recordedType, methodName, ResponseFileTypeName, token.Value);
        }

        public void RecordRequest(object arguments, Type recordedType, string methodName)
        {
            RecordRequest(arguments, recordedType, methodName, GetToken());
        }

        public void RecordRequest(object arguments, Type recordedType, string methodName, TapeToken token)
        {
            RecordObject(arguments, recordedType, methodName, RequestFileTypeName, token.Value);
        }

        private void RecordObject(object objectToRecord, Type reflectedType, string methodName, string fileTypeName, string token)
        {
            var path = GetPath(reflectedType, methodName, true);
            SerialiseObject(path, objectToRecord, String.Format("{0}-{1}.xml", token, fileTypeName));
        }

        private void SerialiseObject(string path, object @object, string fileName)
        {
            string tapeLocation = Path.Combine(path, fileName);

            Debug.WriteLine("Writing to tape at: " + tapeLocation);

            using (Stream stream = new FileStream(tapeLocation, FileMode.Create, FileAccess.Write))
            {
                _serializer.WriteObject(stream, @object);
                stream.Close();
            }
        }

        private object DeserialiseObject(Stream openRead)
        {
            using (openRead)
            {
                return _serializer.ReadObject(openRead);
            }
        }

        private string GetPath(Type reflectedType, string methodName, bool forceCrate)
        {
            var directory = new DirectoryInfo(Path.Combine(_tapeFolder, Path.Combine(reflectedType.ToString(), methodName)));

            if (forceCrate && !directory.Exists)
            {
                directory.Create();
            }
            return directory.FullName;
        }

        public object Playback(Type recordedType, string methodName)
        {
            var path = GetPath(recordedType, methodName, false);
            var di = new DirectoryInfo(path);

            if (!di.Exists)
            {
                throw new Exception("Expected tape location does not exist: " + di.FullName);
            }

            var files = from file in di.GetFiles(string.Format("*-{0}.xml", ResponseFileTypeName)) 
                        select new {  f = file, stamp = this.GetTicks(file.Name) };

            var filteredFiles = from file in files
                                orderby file.stamp 
                                where file.stamp > _position
                                select file;
                         
            var selectedFile = filteredFiles.FirstOrDefault();

            _position = selectedFile.stamp;

            if (selectedFile != null)
            {
                return DeserialiseObject(selectedFile.f.OpenRead());
            }

            throw new Exception(string.Format("Unable to find a saved response on tape at {0} at the current position", di.FullName));
        }

        public TapeToken GetToken()
        {
            var stamp = (long)(DateTime.UtcNow - new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            return new TapeToken()
            {
                Value = String.Format("{0:x16}", stamp)
            };
        }

        private long GetTicks(String fileName)
        {
             var x16 = fileName.Substring(0,  fileName.IndexOf('-'));
             var result = Int64.Parse(x16, System.Globalization.NumberStyles.HexNumber);
             return result;
        }

        public void NotifyEject(string newTapeLocation)
        {
            _tapeFolder = newTapeLocation;
        }


        public void Erase()
        {
            if (Directory.Exists(_tapeFolder))
                Directory.Delete(_tapeFolder, true);
        }
    }
}