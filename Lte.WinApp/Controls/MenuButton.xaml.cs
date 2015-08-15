using System.Windows;
using System.Windows.Controls;

namespace Lte.WinApp.Controls
{
    /// <summary>
    /// MenuButton.xaml 的交互逻辑
    /// </summary>
    public partial class MenuButton : UserControl
    {
        public MenuButton()
        {
            InitializeComponent();
        }

        public Button Button
        {
            get { return Me; }
        }


        public string ButtonTag
        {
            get { return (string)GetValue(ButtonTagProperty); }
            set { SetValue(ButtonTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTagProperty =
            DependencyProperty.Register("ButtonTag", typeof (string), typeof (MenuButton),
                new PropertyMetadata("", ButtonTagChanged));

        private static void ButtonTagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton c = (MenuButton)d;
            Button theButton = c.Me;
            theButton.Tag = e.NewValue.ToString();
        }


        public string ButtonTitle
        {
            get { return (string)GetValue(ButtonTitleProperty); }
            set { SetValue(ButtonTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonTitleProperty =
            DependencyProperty.Register("ButtonTitle", typeof(string), typeof(MenuButton), 
            new PropertyMetadata("", ButtonTitleChanged));

        private static void ButtonTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton c = (MenuButton)d;
            Label theLabel = c.MyTitle;
            theLabel.Content = e.NewValue.ToString();
        }


        public string ButtonContent
        {
            get { return (string)GetValue(ButtonContentProperty); }
            set { SetValue(ButtonContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register("ButtonContent", typeof(string), typeof(MenuButton), 
            new PropertyMetadata("", ButtonContentChanged));

        private static void ButtonContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuButton c = (MenuButton)d;
            TextBlock theBlock = c.MyContent;
            theBlock.Text = e.NewValue.ToString();
        }
    }
}
