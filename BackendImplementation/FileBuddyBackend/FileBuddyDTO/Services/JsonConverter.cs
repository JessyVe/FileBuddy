﻿using Newtonsoft.Json;
using System;

namespace SharedRessources.Services
{
    public static class JsonConverter
    {
        /// <summary>
        /// Returns a json string representing the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetAsJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        /// <summary>
        /// Returns an object which is represented by the given json string. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Returns true if conversation the the given type is possible.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool TryParseJson<T>(string jsonString)
        {
            try
            {
                JsonConvert.DeserializeObject<T>(jsonString);
                return true;

            } catch(Exception ex)
            {
                // TODO: Implement logging
                return false;
            } 
        }
    }
}