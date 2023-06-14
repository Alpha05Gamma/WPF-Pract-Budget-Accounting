using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.budgetBox.PreviewTextInput += new TextCompositionEventHandler(textBox_PreviewTextInput);
            try
            {                
                Finance.finances = JSON.Deserialize<ObservableCollection<Finance>>(FilePath);
                foreach(Finance finance in Finance.finances)
                {
                    int c = 0;
                    foreach(string str in temes)
                    {
                        if(str == finance.type)
                        {
                            c++;
                        }
                    }
                    if(c == 0)
                    {
                        temes.Add(finance.type);
                    }
                }

                temeSelector.ItemsSource = temes;
            }
            catch 
            {
                MessageBox.Show("Не удалось загрузить записи из памяти");
            }
            DateTime dateTime = DateTime.Now;
            datePicker.SelectedDate = dateTime.Date;
        }
        Finance Finance;
        string FilePath = JSON.FilePath;
        List<string> temes = new List<string>();

        private void Datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.SelectedItem =  null;
            PopulateDataGridByDay(datePicker.SelectedDate.Value);
        }

        private void PopulateDataGridByDay(DateTime selectedDate)
        {
            var financesByDay = Finance.finances.Where(f => f.date == selectedDate);
            DataGrid.ItemsSource = financesByDay;
            countBudget();
        }

        private void countBudget() 
        {
            var financesByDay = Finance.finances.Where(f => f.date == datePicker.SelectedDate.Value);

            try
            {
                double buff = Convert.ToDouble(budgetBox.Text);
                foreach (var f in financesByDay)
                {
                    buff = buff + f.money;
                }
                if(buff < 0) { buff = 0; }
                resultBox.Text = "Итог: " + buff.ToString();
            }
            catch(Exception ex) 
            {
                
                resultBox.Text = "Итог: " + "Вычисление итога не удалось, проверьте бюджет";
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialog = new DialogWindow();
            if (dialog.ShowDialog()==true)
            {
                int c = 0;
                foreach(string str in temes)
                {
                    if(str == dialog.teme) { c ++; }                    
                }      
                if(c == 0)
                {
                    temes.Add(dialog.teme);
                }
                temeSelector.ItemsSource = temes;
            }
            else
            {
                MessageBox.Show("Тема не была добавлена");
            }
        }
        

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            createfinance();
        }
        private int createfinance()
        {
            try
            {
                double buff = Convert.ToDouble(FinMoney.Text);
            }
            catch
            {
                MessageBox.Show("Сумма траты не являтся числом");
                return 0;
            }
            try
            {
                Finance finance01 = new Finance(FinName.Text, temeSelector.SelectedItem.ToString(), Convert.ToDouble(FinMoney.Text), datePicker.DisplayDate);
                Finance.finances.Add(finance01);
                PopulateDataGridByDay(datePicker.SelectedDate.Value);
                JSON.Serialize(Finance.finances, FilePath);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message); 
                return 0;
            }
            return 1;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Finance finance = (Finance)DataGrid.SelectedItem;
                Finance.finances.Remove(finance);
                DataGrid.SelectedItem = null;
                createfinance();
            }
            catch
            {

            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataGrid.SelectedItem != null)
            {
                Finance finance = (Finance)DataGrid.SelectedItem;
                showItem(finance);
            }            
        }
        private void showItem(Finance finance)
        {
            try
            {
                FinName.Text = finance.name;
                FinMoney.Text = finance.money.ToString();
                temeSelector.SelectedItem = finance.type;
            }
            catch
            {

            }
            
        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            try
            {
                if(((TextBox)sender).Text.Length > 2)
                {
                    double test = Convert.ToDouble(((TextBox)sender).Text);
                }
                countBudget();
            }
            catch
            {
                MessageBox.Show("Введенное значение не число");
            }
        }

        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Finance finance = (Finance)DataGrid.SelectedItem;
            Finance.finances.Remove(finance);
            DataGrid.SelectedItem = null;
            PopulateDataGridByDay(datePicker.SelectedDate.Value);
        }

        private void BudgetCount_Click(object sender, RoutedEventArgs e)
        {
            countBudget();
        }
    }
}
