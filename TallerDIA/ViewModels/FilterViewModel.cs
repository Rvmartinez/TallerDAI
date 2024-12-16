using System;
using System.Collections.ObjectModel;
using TallerDIA.ViewModels;

namespace TallerDIA.Utils;

public abstract class FilterViewModel<T> : ViewModelBase
{
    private String _FilterText = "";
    private int _SelectedFilterMode = 0;
    public abstract ObservableCollection<String> _FilterModes { get; }
    public ObservableCollection<String> FilterModes
    {
        get => _FilterModes;
    }

    public int SelectedFilterMode
    {
        get => _SelectedFilterMode;
        set
        {
            SetProperty(ref _SelectedFilterMode, value); OnPropertyChanged("FilteredItems");
        }
    }

    public String FilterText
    {
        get => _FilterText;
        set { SetProperty(ref _FilterText, value); OnPropertyChanged("FilteredItems"); }
    }
    public abstract ObservableCollection<T> FilteredItems { get; }

}