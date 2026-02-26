using Microsoft.Extensions.DependencyInjection;
using Domain.Commands;
using Domain.Models;
using Domain.Queries;
using PansoyWpf.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace CabarlesWpf.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly POSStore _posStore;

        private string _ProductName;
        private int _Price;
        private int _Stock;
        
        private POS _selectedPOS;
        private bool _isEditMode;

        public MainViewModel(
            IServiceProvider serviceProvider,
            POSStore posStore)
        {
            _serviceProvider = serviceProvider;
            _posStore = posStore;

            AddCommand = new CabarlesWpf.Commands.RelayCommand(async _ => await AddMovie(), _ => !IsEditMode);
            EditCommand = new CabarlesWpf.Commands.RelayCommand(_ => EditMovie(), _ => SelectedMovie != null && !IsEditMode);
            UpdateCommand = new CabarlesWpf.Commands.RelayCommand(async _ => await UpdateMovie(), _ => IsEditMode);
            DeleteCommand = new CabarlesWpf.Commands.RelayCommand(async _ => await DeleteMovie(), _ => SelectedMovie != null);
            CancelCommand = new CabarlesWpf.Commands.RelayCommand(_ => CancelEdit(), _ => IsEditMode);

            LoadPOS();
        }

        public ObservableCollection<POS> POSs => _posStore.POS;

        public string Price        {
            get => _Price.ToString();
            set
            {
                if (int.TryParse(value, out int price))
                {
                    _Price = price;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for Price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        


        public string ProductName
        {
            get => _ProductName;
            set { _ProductName = value; OnPropertyChanged(); }
         }

        

        public int Stock
        {
            get => _Stock;
            set
            {
                if (value >= 0)
                {
                    _Stock = value;
                    OnPropertyChanged();
                }
                else
                {
                    MessageBox.Show("Stock cannot be negative.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
}


       

        public POS SelectedPOS
        {
            get => _selectedPOS;
            set
            {
                _selectedPOS = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelCommand { get; }

        private async void LoadPOS()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<GetAllPOSQuery, List<POS>>>();
                var pos = await handler.HandleAsync(new GetAllPOSQuery());
                _posStore.SetPOS(pos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddPOS()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Price) ||
                Year <= 0 || string.IsNullOrWhiteSpace(Stock))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateMovieCommand>>();

                var command = new CreateMovieCommand
                {
                    ProductName = ProductNam,
                    Price = Price,
                    Stock = Stock,
                    
                };

                await handler.ExecuteAsync(command);
                ClearFields();
                LoadPOS();
                MessageBox.Show("Movie added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? $"\n\nInner Exception: {ex.InnerException.Message}" : "";
                MessageBox.Show($"Error adding movie: {ex.Message}{innerMessage}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditPOS()
        { if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Price) ||
                Year <= 0 || string.IsNullOrWhiteSpace(Stock))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (SelectedPOS != null)
            {
                ProductName = SelectedPOS.ProductName;
                Price = SelectedPOS.Price;
                Stock = SelectedPOS.Stock;
                
                IsEditMode = true;
            }
        }

        private async Task UpdatePOS()
        {
            if (SelectedPOS == null) return;

            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Price) ||
                Year <= 0 || string.IsNullOrWhiteSpace(Stock))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<UpdatePOSCommand>>();

                var command = new UpdatePOSCommand
                {
                   ProductID = SelectePOS.ProductID,
                   ProductName = ProductName,
                    Price = Price,
                    Stock = Stock,
                    
                   
                };

                await handler.ExecuteAsync(command);
                ClearFields();
                IsEditMode = false;
                LoadPOS();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeletePOS()
        {
            if (SelectedPOS == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete '{SelectedPOS.ProductName}'?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<DeletePOSCommand>>();

                    var command = new DeletePOSCommand { Id = SelectedPOS.ProductID };
                    await handler.ExecuteAsync(command);
                    ClearFields();
                    LoadPOS();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting pos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelEdit()
        {
            ClearFields();
            IsEditMode = false;
        }

        private void ClearFields()
        {
            ProductName = int.Empty;
            Price = string.Empty;
            Strock = int.Empty;
   
            SelectedPOS = null;
        }
    }
}

