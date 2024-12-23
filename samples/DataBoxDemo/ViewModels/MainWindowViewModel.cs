using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DataBoxDemo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly SourceList<ItemViewModel> _itemsSourceList;
    private ReadOnlyObservableCollection<ItemViewModel>? _items;
    private readonly Subject<IComparer<ItemViewModel>> _comparerSubject;
    private IDisposable? _subscription;
    private bool _isSortingEnabled;

    public ReadOnlyObservableCollection<ItemViewModel>? Items => _items;

    [Reactive]
    public partial ItemViewModel? SelectedItem { get; set; }
    
    [Reactive]
    public partial ListSortDirection? SortingStateColumn1 { get; set; }
    
    [Reactive]
    public partial ListSortDirection? SortingStateColumn2 { get; set; }
    
    [Reactive]
    public partial ListSortDirection? SortingStateColumn3 { get; set; }
    
    [Reactive]
    public partial ListSortDirection? SortingStateColumn4 { get; set; }
    
    [Reactive]
    public partial ListSortDirection? SortingStateColumn5 { get; set; }
    
    public ICommand SortCommand { get; }

    public ICommand AddItemCommand { get; }

    public ICommand InsertItemCommand { get; }

    public ICommand RemoveItemCommand { get; }

    public ICommand SelectFirstItemCommand { get; }

    public MainWindowViewModel()
    {
        var totalItems = 10_000;
        var rand = new Random();
        var items = new List<ItemViewModel>();

        for (var i = 0; i < totalItems; i++)
        {
            items.Add(CreateItem(i));
        }

        ItemViewModel CreateItem(int index)
        {
            return new ItemViewModel(
                $"Item {index}-1",
                $"Item {index}-2",
                $"Item {index}-3",
                rand.NextDouble() > 0.5,
                index,
                //rand.Next(24, 100));
                double.NaN);
        }

        _itemsSourceList = new SourceList<ItemViewModel>();
        _itemsSourceList.AddRange(items);

        _comparerSubject = new Subject<IComparer<ItemViewModel>>();
        _isSortingEnabled = false;
        SortingStateColumn5 = ListSortDirection.Ascending;
        EnableSort(x => x.Column5, SortingStateColumn5);

        SortCommand = ReactiveCommand.CreateFromTask<string?>(async sortMemberPath =>
        {
            await Task.Run(() =>
            {
                Sort(sortMemberPath);
            });
        });

        InsertItemCommand = ReactiveCommand.Create(() =>
        {
            if (_items is null)
            {
                return;
            }
            var index = _items.Count;
            var item = CreateItem(index);
            _itemsSourceList.Insert(0, item);
        });

        AddItemCommand = ReactiveCommand.Create(() =>
        {
            if (_items is null)
            {
                return;
            }
            var index = _items.Count;
            var item = CreateItem(index);
            _itemsSourceList.Add(item);
        });

        RemoveItemCommand = ReactiveCommand.Create<ItemViewModel?>((item) =>
        {
            if (item is not null)
            {
                _itemsSourceList.Remove(item);
            }
        });

        SelectFirstItemCommand = ReactiveCommand.Create(() =>
        {
            if (_items is null)
            {
                return;
            }
            SelectedItem = _items.FirstOrDefault();
        });
    }

    private IObservable<IChangeSet<ItemViewModel>> GetSortObservable(IComparer<ItemViewModel> comparer)
    {
        return _itemsSourceList!
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Sort(comparer, comparerChanged: _comparerSubject)
            .Bind(out _items);
    }

    private IObservable<IChangeSet<ItemViewModel>> GetDefaultObservable()
    {
        return _itemsSourceList!
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _items);
    }

    private void EnableSort(Func<ItemViewModel, IComparable> expression, ListSortDirection? listSortDirection)
    {
        var sortExpressionComparer = listSortDirection == ListSortDirection.Ascending
            ? SortExpressionComparer<ItemViewModel>.Ascending(expression)
            : SortExpressionComparer<ItemViewModel>.Descending(expression);

        if (!_isSortingEnabled)
        {
            _subscription?.Dispose();
            _subscription = GetSortObservable(sortExpressionComparer).Subscribe();
            _isSortingEnabled = true;
            this.RaisePropertyChanged(nameof(Items));
        }
        else
        {
            _comparerSubject.OnNext(sortExpressionComparer);
        }
    }

    private void DisableSort()
    {
        if (_isSortingEnabled)
        {
            _subscription?.Dispose();
            _subscription = GetDefaultObservable().Subscribe();
            _isSortingEnabled = false;
            this.RaisePropertyChanged(nameof(Items));
        }
    }

    private void Sort(string? sortMemberPath)
    {
        switch (sortMemberPath)
        {
            case null:
                DisableSort();
                break;
            case "Column1":
                EnableSort(x => x.Column1, SortingStateColumn1);
                break;
            case "Column2":
                EnableSort(x => x.Column2, SortingStateColumn2);
                break;
            case "Column3":
                EnableSort(x => x.Column3, SortingStateColumn3);
                break;
            case "Column4":
                EnableSort(x => x.Column4, SortingStateColumn4);
                break;
            case "Column5":
                EnableSort(x => x.Column5, SortingStateColumn5);
                break;
        }
    }
}
