// Comment out the following line to not show JDebug logs.
#define SHOW_MESSAGES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JBirdEngine {

    /// <summary>
    /// Helper functions for debugging.
    /// </summary>
    public static class JDebug {

        public static bool showMessages {
            get {
                #if SHOW_MESSAGES
                return true;
                #else
                return false;
                #endif
            }
        }

        public static void Log (this Object obj, string message) {
            Debug.Log(MessageFormat(obj, message));
        }

        public static void Warning (this Object obj, string message) {
            Debug.LogWarning(MessageFormat(obj, message));
        }

        public static void Error (this Object obj, string message) {
            Debug.LogError(MessageFormat(obj, message));
        }

        public static string MessageFormat (this Object obj, string message) {
            return MessageFormat(message, obj.GetType());
        }

        public static string MessageFormat (string message, System.Type type) {
            return string.Format("{0}: {1}", type.Name, message);
        }

        public static T NullCheck<T> (this Object script, ref T obj, string varName = "variable") where T : Object {
            if (showMessages && !obj) {
                Debug.LogWarningFormat("{0}: {1}", script.GetType().Name, varName == "variable" ? "Variable is null!" : string.Format("Varaible '{0}' is null!", varName));
            }
            return obj;
        }

        public static T NullCheck<T> (ref T obj, string varName = "variable") where T : Object {
            if (showMessages && !obj) {
                Debug.LogWarning(varName == "variable" ? "Variable is null!" : string.Format("Variable '{0}' is null!", varName));
            }
            return obj;
        }

        public static T NullCheck<T> (ref T obj) where T : Object {
            if (showMessages && !obj) {
                Debug.LogWarning("Variable is null!");
            }
            return obj;
        }

        public static T NullCheckMessage<T> (this Object script, ref T obj, string message) where T : Object {
            if (showMessages && !obj) {
                Debug.LogWarning(MessageFormat(script, message));
            }
            return obj;
        }

        public static T NullCheckMessage<T> (ref T obj, string message) where T : Object {
            if (showMessages && !obj) {
                Debug.LogWarning(message);
            }
            return obj;
        }

    }
}