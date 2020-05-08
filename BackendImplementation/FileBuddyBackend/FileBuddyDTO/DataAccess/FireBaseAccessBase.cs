using Firebase.Database;
using SharedRessources.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedRessources.DataAccess
{
    /// <summary>
    /// Implements fundamental behavoirs and 
    /// properties to firebase communication.
    /// </summary>
    public class FireBaseAccessBase
    {
        protected static readonly log4net.ILog Log =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected FirebaseClient _firebaseClient;

        // TODO: Extract to settings
        private const string AuthSecret = "G9gFvZ3Jz4DKHczBMRNfRC7iNM2XWREyesbbk6kx";
        private const string BasePath = "https://filebuddy-403f3.firebaseio.com/";

        protected HashingEngine _userHashingEngine;
        protected HashingEngine _fileHashingEngine;

        protected FireBaseAccessBase()
        {
            InitializeFirebaseClient();
            _userHashingEngine = new UserHashingEngine();
            _fileHashingEngine = new FileHashingEngine();
        }

        private void InitializeFirebaseClient()
        {
            Log.Debug("Initializing Firebase client ...");
            try
            {
                _firebaseClient = new FirebaseClient(
                  BasePath,
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(AuthSecret)
                  });
            }
            catch (Exception ex)
            {
                Log.Error("Initialization of Firebase client failed!", ex);
            }
            Log.Debug("Finished initialization of Firebase client!");
        }

        protected T RetrieveFirstOrDefault<T>(IReadOnlyCollection<FirebaseObject<T>> data)
        {
            if (data.Count == 1)
            {
                var enumeration = data.GetEnumerator();

                if (enumeration.MoveNext())
                    return enumeration.Current.Object;
            }

            if (data.Count > 1)
                Log.Error("There is more than one object in the collection!");
            else if (data.Count < 1)
                Log.Error("Collection is empty!");

            return default;
        }

        protected IList<T> ConvertToObjectList<T>(IReadOnlyCollection<FirebaseObject<T>> data)
        {
            var list = new List<T>();
            if (data.Count < 1)
            {
                Log.Error("No data to retrieve detected.");
                return list;
            }

            var enumeration = data.GetEnumerator();
            while (enumeration.MoveNext())
            {
                var obj = enumeration.Current.Object;
                if (obj is T)
                {
                    list.Add(obj);
                }
                else
                {
                    Log.ErrorFormat("Unable to cast object to given type. ");
                }
            }
            return list;
        }

        protected IList<T> ConvertDictionaryToList<T>(IReadOnlyCollection<FirebaseObject<Dictionary<string, T>>> data)
        {
            var list = new List<T>();

            var enumeration = data.GetEnumerator();
            while (enumeration.MoveNext())
            {
                foreach(var key in enumeration.Current.Object.Keys)
                {
                    var obj = enumeration.Current.Object[key];       
                    list.Add(obj);                   
                }
            }
            return list;
        }
    }
}
