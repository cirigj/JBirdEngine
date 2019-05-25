using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdLib {

	namespace DataTracking {

        /// <summary>
        /// Interface for making a class have trackable data.
        /// GetData function implementation must be public and return the data you want to track with a DataTracker.
        /// Subscribe to a properly-typed DataTracker to have it automatically track the data from something with this interface.
        /// </summary>
        /// <typeparam name="D">The data type of what you want to track.</typeparam>
        public interface ITrackableData<D> {

            /// <summary>
            /// Implementation of this function should return the data you want to track.
            /// </summary>
            /// <returns>Data that will be tracked by a DataTracker.</returns>
            D GetData ();

        }

        /// <summary>
        /// A class that will track data of any object that susbcribes to it.
        /// Subscribed objects must inherit from ITrackableData<D>, where D is the type of the data being tracked.
        /// </summary>
        /// <typeparam name="T">Type of the class that uses the ITrackableData<D> interface.</typeparam>
        /// <typeparam name="D">Type of the data to be tracked.</typeparam>
        [System.Serializable]
        public class DataTracker<T, D> where T : UnityEngine.Object, ITrackableData<D> {

            /// <summary>
            /// Class used by the DataTracker to keep track of the data history from subscribed objects.
            /// </summary>
            /// <typeparam name="T">Type of the class that uses the ITrackableData<D> interface.</typeparam>
            /// <typeparam name="D">Type of the data to be tracked.</typeparam>
			protected class History<T, D> where T : UnityEngine.Object, ITrackableData<D> {

                public T trackedItem;
                private List<D> _history;
                public List<D> history {
                    get { return _history; }
                }

                /// <summary>
                /// Initializes a History object to start keeping track of the specified item.
                /// </summary>
                /// <param name="item">The item to track.</param>
                public History (T item) {
                    trackedItem = item;
                    Initialize();
                }

                /// <summary>
                /// Private initializer for History class.
                /// </summary>
                void Initialize () {
                    _history = new List<D>();
                }

            }

            private List<T> _trackedItems;
            /// <summary>
            /// A list of the items subscribed to this DataTracker.
            /// </summary>
            public List<T> trackedItems {
                get { return new List<T>(_trackedItems); }
            }
            /// <summary>
            /// The number of seconds between each time the DataTracker records data from its subscribers.
            /// </summary>
            public float dataTrackingTimestep;
            private float timestep;
            /// <summary>
            /// Max length of the list of recorded data.
            /// </summary>
            public int maxHistoryLength;
            private List<History<T, D>> _historyList;

            /// <summary>
            /// Creates an instance of a DataTracker.
            /// </summary>
            public DataTracker () {
                Initialize();
            }

            /// <summary>
            /// Creates and instance of a DataTracker.
            /// </summary>
            /// <param name="maxHist">Max length of the list of recorded data.</param>
            /// <param name="tStep">The number of seconds between each time the DataTracker records data from its subscribers.</param>
            public DataTracker (int maxHist, float tStep) {
                maxHistoryLength = maxHist;
                dataTrackingTimestep = tStep;
                Initialize();
            }
            
            /// <summary>
            /// Private initialization function.
            /// </summary>
            void Initialize () {
                _trackedItems = new List<T>();
                _historyList = new List<History<T, D>>();
                if (dataTrackingTimestep < 0f) {
                    dataTrackingTimestep = 0f;
                }
                if (maxHistoryLength < 1) {
                    maxHistoryLength = 1;
                }
            }

            /// <summary>
            /// Adds the specified item as a subscriber to this DataTracker.
            /// </summary>
            /// <param name="item">Item to add as a subscriber.</param>
            public void Subscribe (T item) {
                _trackedItems.Add(item);
                _historyList.Add(new History<T, D>(item));
            }

            /// <summary>
            /// DataTracker update function. Make sure to call this from a MonoBehaviour's Update() function or it won't actually do anything.
            /// If this DataTracker has a dataTrackingTimestep of 0, it will record the data whenver the Update function is called. This allows for a lazy update implementation (useful for turn-based).
            /// </summary>
            public void Update () {
                timestep += Time.deltaTime;
                if (timestep >= dataTrackingTimestep) {
                    timestep -= dataTrackingTimestep;
                    FetchData();
                }
            }

            /// <summary>
            /// Internal function to fetch the data from all subscribers.
            /// </summary>
            void FetchData () {
                foreach (History<T, D> historyItem in _historyList) {
                    UpdateHistoryData(historyItem);
                }
            }

            /// <summary>
            /// Internal function to update a History instance.
            /// </summary>
            /// <param name="historyItem">History instance to update.</param>
            void UpdateHistoryData (History<T, D> historyItem) {
                if (historyItem.history.Count >= maxHistoryLength) {
                    while (historyItem.history.Count > maxHistoryLength - 1) {
                        historyItem.history.RemoveAt(0);
                    }
                }
                historyItem.history.Add(historyItem.trackedItem.GetData());
            }

            /// <summary>
            /// Returns the history recorded from the specified item as a list of data.
            /// </summary>
            /// <param name="item">Item to get the history from.</param>
            /// <returns>A list of data history recorded by this DataTracker.</returns>
            public List<D> GetItemDataHistory (T item) {
                List<D> dataList = new List<D>();
                foreach (History<T, D> historyItem in _historyList) {
                    if (historyItem.trackedItem == item) {
                        dataList = historyItem.history;
                        return dataList;
                    }
                }
                Debug.LogErrorFormat("DataTracker<{0}>: Item {1} could not be found!", typeof(T).ToString(), item);
                return dataList;
            }

        }

    }

}
