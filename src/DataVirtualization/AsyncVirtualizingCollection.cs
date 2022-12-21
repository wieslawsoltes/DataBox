using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace DataVirtualization
{
	/// <summary>
	/// Derived VirtualizingCollection, performing loading asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection</typeparam>
	public class AsyncVirtualizingCollection<T> : VirtualizingCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncVirtualizingCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="itemsProvider">The items provider.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageTimeout">The page timeout.</param>
		public AsyncVirtualizingCollection(IItemsProvider<T> itemsProvider, int pageSize, int pageTimeout)
			: base(itemsProvider, pageSize, pageTimeout)
		{
			SynchronizationContext = SynchronizationContext.Current;
		}

		#region SynchronizationContext

        /// <summary>
		/// Gets the synchronization context used for UI-related operations. This is obtained as
		/// the current SynchronizationContext when the AsyncVirtualizingCollection is created.
		/// </summary>
		/// <value>The synchronization context.</value>
		protected SynchronizationContext? SynchronizationContext { get; }

        #endregion

		#region INotifyCollectionChanged

		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		public event NotifyCollectionChangedEventHandler? CollectionChanged;

		/// <summary>
		/// Raises the <see cref="E:CollectionChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
            CollectionChanged?.Invoke(this, e);
        }

		/// <summary>
		/// Fires the collection reset event.
		/// </summary>
		private void FireCollectionReset()
		{
			var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			OnCollectionChanged(e);
		}

		#endregion

		#region INotifyPropertyChanged

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
            PropertyChanged?.Invoke(this, e);
        }

		/// <summary>
		/// Fires the property changed event.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		private void FirePropertyChanged(string propertyName)
		{
			var e = new PropertyChangedEventArgs(propertyName);
			OnPropertyChanged(e);
		}

		#endregion

		#region IsLoading

		private bool _isLoading;

		/// <summary>
		/// Gets or sets a value indicating whether the collection is loading.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this collection is loading; otherwise, <c>false</c>.
		/// </value>
		public bool IsLoading
		{
			get
			{
				return _isLoading;
			}
			set
			{
				if (value != _isLoading)
				{
					_isLoading = value;
					FirePropertyChanged(nameof(IsLoading));
				}
			}
		}

		private bool _isInitializing;

		public bool IsInitializing
		{
			get
			{
				return _isInitializing;
			}
			set
			{
				if (value != _isInitializing)
				{
					_isInitializing = value;
					FirePropertyChanged(nameof(IsInitializing));
				}
			}
		}
		#endregion

		#region Load overrides

		/// <summary>
		/// Asynchronously loads the count of items.
		/// </summary>
		protected override void LoadCount()
		{
			if (Count == 0)
			{
				IsInitializing = true;
			}
			ThreadPool.QueueUserWorkItem(LoadCountWork);
		}

		/// <summary>
		/// Performed on background thread.
		/// </summary>
		/// <param name="args">None required.</param>
		private void LoadCountWork(object? args)
		{
			var count = FetchCount();
			SynchronizationContext?.Send(LoadCountCompleted, count);
		}

		/// <summary>
		/// Performed on UI-thread after LoadCountWork.
		/// </summary>
		/// <param name="args">Number of items returned.</param>
		protected virtual void LoadCountCompleted(object? args)
		{
            if (args is int newCount)
            {
                TakeNewCount(newCount);
            }
			IsInitializing = false;
		}

		private void TakeNewCount(int newCount)
		{
			if (newCount != Count)
			{
				Count = newCount;
				EmptyCache();
				FireCollectionReset();
			}
		}

		/// <summary>
		/// Asynchronously loads the page.
		/// </summary>
		/// <param name="pageIndex">The page index.</param>
		/// <param name="pageLength">The page length.</param>
		protected override void LoadPage(int pageIndex, int pageLength)
		{
			IsLoading = true;
			ThreadPool.QueueUserWorkItem(LoadPageWork, new[] { pageIndex, pageLength });
		}

		/// <summary>
		/// Performed on background thread.
		/// </summary>
		/// <param name="state">Index of the page to load.</param>
		private void LoadPageWork(object? state)
		{
            if (state is int[] args)
            {
                var pageIndex = args[0];
                var pageLength = args[1];
                var dataItems = FetchPage(pageIndex, pageLength, out var overallCount);
                SynchronizationContext?.Send(LoadPageCompleted, new object[] { pageIndex, dataItems, overallCount });
            }
		}

		/// <summary>
		/// Performed on UI-thread after LoadPageWork.
		/// </summary>
		/// <param name="state">object[] { int pageIndex, IList(T) page }</param>
		private void LoadPageCompleted(object? state)
		{
            if (state is object[] args)
            {
                var pageIndex = (int)args[0];
                var dataItems = (IList<T>)args[1];
                var newCount = (int)args[2];
                TakeNewCount(newCount);

                PopulatePage(pageIndex, dataItems);
            }
			IsLoading = false;
		}

		#endregion
	}
}
