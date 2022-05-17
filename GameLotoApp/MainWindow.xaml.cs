using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GameLotoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LotoWindow lotoWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidAmount(AmountTxt.Text))
            {
                ContinueBtn.IsEnabled = false;
                AmountTxt.IsReadOnly = true;
                NameTxt.IsReadOnly = true;
                PourProgressBar();
            }
            else
            {
                AmountTxt.Foreground = Brushes.Red;
                MessageBox.Show("Wrong Input, Please Change Your Amount");
                AmountTxt.Foreground = Brushes.Black;
            }
        }

        private bool IsValidAmount(string amount)
        {
            int am;
            int.TryParse(amount, out am);
            if (amount[0] != '0' && am >= 100)
                return true;

            return false;
        }

        private async void PourProgressBar()
        {
            Person.Name = NameTxt.Text;
            Person.Amount = AmountTxt.Text;

            do
            {
                await Task.Delay(10);
                ContinueProgressBar.Value++;
            } while (ContinueProgressBar.Value != 100);

            MessageBox.Show("Information Saved Successfully !");
            ShowLotoWindow();
        }

        private async void ShowLotoWindow()
        {
            lotoWindow = new LotoWindow();
            this.Close();
            lotoWindow.Show();
            await Task.Delay(500);
            MessageBox.Show($"Hi {Person.Name} you can win 200$ in the first round, and it will increase by 200$ in the next rounds");
        }

        private void NameTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsValidInput(e.Text, "^[a-zA-Z ]*$"))
                e.Handled = true;
        }

        private bool IsValidInput(string previewInput, string pattern)
        {
            if (Regex.IsMatch(previewInput, pattern))
                return true;

            return false;
        }

        private void AmountTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsValidInput(e.Text, "^[0-9]*$"))
                e.Handled = true;
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && ContinueBtn.IsEnabled == true)
                ContinueBtn_Click(null, null);
        }
    }
}
