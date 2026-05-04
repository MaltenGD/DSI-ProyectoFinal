using System.Collections.Generic;
using UnityEngine;

namespace Lab6_namespace
{
    public static class JsonHelperIndividuo
    {
        [System.Serializable]
        private class Wrapper<T>
        {
            public List<T> Individuos;
        }

        public static string ToJson<T>(List<T> lista, bool pretty = false)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Individuos = lista;
            return JsonUtility.ToJson(wrapper, pretty);
        }

        public static List<T> FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Individuos;
        }
    }
}