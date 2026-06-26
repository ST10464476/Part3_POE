using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Part3_POE.Models;
using Part3_POE.Services;

namespace Part3_POE
{
    public partial class MainWindow : Window
    {
        RespondingClass service = new RespondingClass();
        private AudioPlayer _player;
        private ArtClass _artClass;

        ResponseDelegate del;

        public MainWindow()
        {
            _player = new AudioPlayer();
            _artClass = new ArtClass();
            InitializeComponent();
              AddMessage($"{_artClass.DisplayArt()}", false);
            _player.Greeting();
             AddMessage("👋 Welcome to CyberBot! ", false);

            del = service.Responding;
        }

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = User_Input.Text.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
                return;

           
            AddMessage(userMessage, true);

            
            AddMessage("Thinking...", false);

            await Task.Delay(1500);

          
            if (chatPanel.Children.Count > 0)
                chatPanel.Children.RemoveAt(chatPanel.Children.Count - 1);

            
            string botReply = del(userMessage);

            AddMessage(botReply, false);

            User_Input.Clear();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WindowGame game = new WindowGame();
            game.Show();
        }

        public void AddMessage(string message, bool isUser)
        {
            Border bubble = new Border
            {
                Background = isUser ? Brushes.LightBlue : Brushes.LightGray,
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 5),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 250
            };

            TextBlock text = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.NoWrap, 
                FontFamily = new System.Windows.Media.FontFamily("Consolas")
            };

            bubble.Child = text;

            chatPanel.Children.Add(bubble);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
             AddMessage($"{_artClass.DisplayArt()}", false);
             AddMessage("👋 Welcome to CyberBot! ", false);
        }

    }
}