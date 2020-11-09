using Microsoft.EntityFrameworkCore.Internal;
using Neighbor.Core.Domain.Models.Finance;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Neighbor.Mobile.Models
{
    public class MonthlyBalanceModel : MonthlyBalance, INotifyPropertyChanged
    {
        private readonly MonthlyBalance _model;
        private bool _turnOnIncomeView;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool TurnOnIncomeView
        {
            get => _turnOnIncomeView;
            set
            {
                if(_turnOnIncomeView == value)
                {
                    return;
                }

                _turnOnIncomeView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TurnOnIncomeView)));

                if (_turnOnIncomeView)
                {
                    this.BalanceAmount = _model.IncomeAmount + _model.ExpenseAmount;
                }
                else
                {
                    this.BalanceAmount = _model.AverageIncomeAmount + _model.ExpenseAmount;
                }

                if(BalanceAmount >= 0)
                {
                    BalanceAmountColor = Color.Green;
                }
                else
                {
                    BalanceAmountColor = Color.Red;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BalanceAmount)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BalanceAmountColor)));
            }
        }

        public Color Color { get; set; }

        public Color BalanceAmountColor { get; set; }

        public MonthlyBalanceModel(MonthlyBalance model, bool turnOnIncomeView)
        {
            _model = model;
            TurnOnIncomeView = turnOnIncomeView;
            var colors = new[] { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Brown, Color.Magenta };

            var colorRandom = new Random();

            this.Color = colors[colorRandom.Next(0, colors.Length)];
            this.IncomeAmount = model.IncomeAmount;
            this.AverageIncomeAmount = model.AverageIncomeAmount;
            this.BalanceAmount = model.AverageIncomeAmount + model.ExpenseAmount;
            this.ExpenseAmount = model.ExpenseAmount;
            this.MonthName = model.MonthName;
            this.MonthNo = model.MonthNo;
            this.TotalIncomeAmount = model.TotalIncomeAmount;
            this.Year = model.Year;

            if (this.BalanceAmount >= 0)
            {
                this.BalanceAmountColor = Color.Green;
            }
            else
            {
                this.BalanceAmountColor = Color.Red;
            }
        }
    }
}
