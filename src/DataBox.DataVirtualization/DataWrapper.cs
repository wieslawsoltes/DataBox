using System.ComponentModel;

namespace DataVirtualization
{
	public class DataWrapper<T> : INotifyPropertyChanged where T : class
	{
		private readonly int _index;
		private T? _data;

		public event PropertyChangedEventHandler? PropertyChanged;

		public DataWrapper(int index)
		{
			_index = index;
		}

		public int Index
		{
			get { return _index; }
		}

		public int ItemNumber
		{
			get { return _index + 1; }
		}

		public bool IsLoading
		{
			get { return Data == null; }
		}

		public T? Data
		{
			get { return _data; }
			internal set
			{
				_data = value;
				OnPropertyChanged(nameof(Data));
				OnPropertyChanged(nameof(IsLoading));
			}
		}

		public bool IsInUse
		{
			get { return PropertyChanged != null; }
		}

		private void OnPropertyChanged(string propertyName)
		{
			System.Diagnostics.Debug.Assert(GetType().GetProperty(propertyName) != null);
			var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
	}
}
