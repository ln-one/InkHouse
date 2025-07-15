using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InkHouse.Models;
using InkHouse.Services;

namespace InkHouse.ViewModels
{
    public partial class AdminSeatManagementViewModel : ViewModelBase
    {
        private readonly SeatService _seatService;

        [ObservableProperty]
        private ObservableCollection<Seat> _seats = new();

        [ObservableProperty]
        private string _newSeatNumber = string.Empty;

        [ObservableProperty]
        private Seat? _selectedSeat;

        [ObservableProperty] private int _totalSeats;
        [ObservableProperty] private int _freeSeats;
        [ObservableProperty] private int _reservedSeats;
        [ObservableProperty] private int _occupiedSeats;

        public IAsyncRelayCommand RefreshCommand { get; }
        public IAsyncRelayCommand AddSeatCommand { get; }
        public IAsyncRelayCommand<Seat> DeleteSeatCommand { get; }
        public IAsyncRelayCommand<(Seat, string)> ChangeStatusCommand { get; }

        public AdminSeatManagementViewModel(SeatService seatService)
        {
            _seatService = seatService;
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            AddSeatCommand = new AsyncRelayCommand(AddSeatAsync);
            DeleteSeatCommand = new AsyncRelayCommand<Seat>(DeleteSeatAsync);
            ChangeStatusCommand = new AsyncRelayCommand<(Seat, string)>(ChangeStatusAsync);
            _ = RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            try
            {
                var seats = await _seatService.GetAllSeatsAsync();
                Seats = new ObservableCollection<Seat>(seats);
                var stats = await _seatService.GetSeatStatisticsAsync();
                TotalSeats = stats.totalSeats;
                FreeSeats = stats.freeSeats;
                ReservedSeats = stats.reservedSeats;
                OccupiedSeats = stats.occupiedSeats;
            }
            catch (Exception ex)
            {
                ShowError("??????: " + ex.Message);
            }
        }

        private async Task AddSeatAsync()
        {
            if (string.IsNullOrWhiteSpace(NewSeatNumber))
            {
                ShowError("请输入座位编号");
                return;
            }
            try
            {
                await _seatService.AddSeatAsync(NewSeatNumber.Trim());
                NewSeatNumber = string.Empty;
                await RefreshAsync();
                ShowSuccess("添加成功");
            }
            catch (Exception ex)
            {
                ShowError("添加失败: " + ex.Message);
            }
        }

        private async Task DeleteSeatAsync(Seat? seat)
        {
            if (seat == null) return;
            try
            {
                await _seatService.DeleteSeatAsync(seat.Id);
                await RefreshAsync();
                ShowSuccess("????");
            }
            catch (Exception ex)
            {
                ShowError("????: " + ex.Message);
            }
        }

        private async Task ChangeStatusAsync((Seat seat, string newStatus) param)
        {
            var (seat, newStatus) = param;
            if (seat == null || string.IsNullOrWhiteSpace(newStatus)) return;
            try
            {
                await _seatService.UpdateSeatStatusAsync(seat.Id, newStatus);
                await RefreshAsync();
                ShowSuccess("?????");
            }
            catch (Exception ex)
            {
                ShowError("??????: " + ex.Message);
            }
        }
    }
} 