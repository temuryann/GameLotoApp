using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLotoApp
{
    /// <summary>
    /// Interaction logic for LotoWindow.xaml
    /// </summary>
    public partial class LotoWindow : Window
    {
        private List<Button[]> buttons = new List<Button[]>();
        private int countTickets;
        private int amount;
        private int winCount;
        private int resForAmount;

        public LotoWindow()
        {
            InitializeComponent();
            Run();
        }

        private void Run()
        {
            PlayerNameLbl.Content += $"Dear {Person.Name} !";
            PlayerWalletLbl.Content = Person.Amount + " $";
            PlayBtn.IsEnabled = false;
            TakeBtn.IsEnabled = false;
            AddButtons();
        }

        private bool IsValidForBuy()
        {
            int.TryParse(Person.Amount, out amount);

            if (!string.IsNullOrEmpty(TicketCountBox.Text))
            {
                int.TryParse(TicketCountBox.Text, out countTickets);

                if (amount >= countTickets * 100)
                    return true;
                else
                {
                    CountLbl.Foreground = Brushes.Red;
                    MessageBox.Show("Your money is not enough, please change count of tickets !");
                    CountLbl.Foreground = Brushes.Black;

                    return false;
                }
            }

            CountLbl.Foreground = Brushes.Red;
            MessageBox.Show("Please choose count");
            CountLbl.Foreground = Brushes.Black;

            return false;
        }

        private void BuyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidForBuy())
            {
                ChangeView();
                PlayBtn.IsEnabled = true;
            }
        }

        private void ChangeView()
        {
            MessageBox.Show("You successfully buy ticket");
            AddTicketCountInLabel();
            TicketCountLbl.Foreground = Brushes.Green;
            Person.Amount = (amount - countTickets * 100).ToString();
            PlayerWalletLbl.Content = Person.Amount + " $";
            PlayerWalletLbl.Foreground = Brushes.Red;
            amount -= countTickets * 100;
        }

        private void AddTicketCountInLabel()
        {
            if (!string.IsNullOrEmpty(TicketCountLbl.Content.ToString()))
                TicketCountLbl.Content = (Convert.ToInt32(TicketCountLbl.Content) + countTickets).ToString();
            else
                TicketCountLbl.Content = countTickets.ToString();
        }

        private void AddButtons()
        {
            buttons.Add(new Button[] { Btn1, Btn2 });
            buttons.Add(new Button[] { Btn3, Btn4 });
            buttons.Add(new Button[] { Btn5, Btn6 });
            buttons.Add(new Button[] { Btn7, Btn8 });
            buttons.Add(new Button[] { Btn9, Btn10 });

            DisEnableAllButtons();
        }

        private void EnableButtonsRow(int row, bool enable)
        {
            buttons[row][0].IsEnabled = enable;
            buttons[row][1].IsEnabled = enable;
        }

        private void DisEnableAllButtons()
        {
            foreach (Button[] item in buttons)
                foreach (Button it in item)
                    it.IsEnabled = false;
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Good Job Friend !");

            winCount = 0;
            countTickets--;
            TicketCountLbl.Content = countTickets.ToString();
            PlayBtn.IsEnabled = false;
            BuyBtn.IsEnabled = false;
            RandomPutAnswers();
            EnableButtonsRow(winCount, true);
        }

        private void TakeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (resForAmount != 0)
                amount += resForAmount;

            resForAmount = 0;
            MessageBox.Show($"Congratulations! your balance is {amount} $");
            Person.Amount = amount.ToString();
            PlayerWalletLbl.Content = Person.Amount + " $";
            PlayerWalletLbl.Foreground = Brushes.Green;
            EndGame();
        }

        private void RandomPutAnswers()
        {
            Random rnd = new Random();

            foreach (Button[] arrayButton in buttons)
            {
                for (int i = 0; i < arrayButton.Length;)
                {
                    if (rnd.Next(-5, 4) <= -1)
                    {
                        arrayButton[i].Tag = true;
                        arrayButton[i + 1].Tag = false;
                        break;
                    }
                    else
                    {
                        arrayButton[i + 1].Tag = true;
                        arrayButton[i].Tag = false;
                        break;
                    }
                }
            }
        }

        private void GameBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if ((bool)button.Tag == true)
            {
                button.Background = Brushes.Green;
                winCount++;
                TakeBtn.IsEnabled = true;
                resForAmount += 200 * winCount;
                MessageBox.Show($"Congratulations! you win {resForAmount}");

                if (winCount != buttons.Count)
                {
                    EnableButtonsRow(winCount, true);
                    EnableButtonsRow(winCount - 1, false);
                }
                else
                    TakeBtn_Click(null, null);
                button.Background = Brushes.LightBlue;
            }
            else
            {
                button.Background = Brushes.Red;
                MessageBox.Show($"I'm sorry you lost, you will succeed next time");
                resForAmount = 0;

                if (IsPlayerHomeless())
                    GoodByePlayer();

                EndGame();
                button.Background = Brushes.LightBlue;
            }
        }

        private void GoodByePlayer()
        {
            MessageBox.Show("Good bye broo, you are homeless !");
            this.Close();
        }

        private bool IsPlayerHomeless()
        {
            if (amount < 100 && countTickets == 0)
                return true;

            return false;
        }

        private void EndGame()
        {
            TakeBtn.IsEnabled = false;
            BuyBtn.IsEnabled = true;

            if (countTickets > 0)
                PlayBtn.IsEnabled = true;
            else
                PlayBtn.IsEnabled = false;

            DisEnableAllButtons();
        }
    }
}
